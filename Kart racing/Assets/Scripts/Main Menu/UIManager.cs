using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using static JourneyRewards;

public class UIManager : MonoBehaviour
{
    public Image AttackPowerBtn;
    public Image PowerBtn;
    public GameObject Warning;
    public TextMeshProUGUI WarningText,playerCount;
    public ScoreUpdater scoreUpdater;
    public Transform coinsAnim;
    public TextMeshProUGUI  coinsText, ScoreHolder, timer;
    public ThirdPersonController thirdPerson;
    Ability specialAbility;
    public static UIManager Instance;
    GameManager gameManager;
    public GameObject gameoverScreen;
    public PlayerRankInfo playerRankBtn;
    public Transform playerRankParent;
    public Sprite[] rankPositions;
    IEnumerable<Character> temp;
    public Image mineBtn;
    public TextMeshProUGUI mineBtnText;
    public SmoothLoading loadingScreen;
    public CharacterUnlock characterUnlock;
    public GameObject statsScreen;

    public GameObject playerControl;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        specialAbility = gameManager.player.specialPower;
        UpdateCoins(PlayerPrefs.GetInt("Coin"));
        temp = new List<Character>();
        UpdateMinesStack(0);
    }
    private void Update()
    {
        ////if (thirdPerson.chargerTime != specialAbility.rechargeTime)
        ////{
        ////    PowerBtn.fillAmount = 1 - Mathf.Clamp01(thirdPerson.chargerTime / specialAbility.rechargeTime);
        ////}
        ////else PowerBtn.fillAmount = 1;
        if (gameManager.ballTaken) CheckHighScore();
    }
    public void ShowGameTime(float time)
    {
        float mins = Mathf.FloorToInt(time/60);
        float secs = Mathf.FloorToInt(time%60);
        timer.text = string.Format("{0:00}:{1:00}",mins,secs);
    }
    public void ShowRedTimer()
    {
        timer.GetComponent<DOTweenAnimation>().DOPlay();
    }
    public void UpdateCoins(int i)
    {
        coinsAnim.DOPunchScale(new Vector3(0.5f, 0.33f, 0), 0.5f).SetEase(Ease.Linear).OnComplete(() => coinsAnim.localScale = Vector3.one);
        coinsText.text = i.ToString();
    }
    public void SetScoreBoard(string name)
    {
        ScoreHolder.text = name + " got trophy";
    }
    void CheckHighScore()
    {
        var sortedKeys = gameManager.scores.OrderByDescending(x => x.Value).Select(x => x.Key).Take(3);
        int i = 0;
       // if (!sortedKeys.SequenceEqual(temp))
       // {
            foreach (var key in sortedKeys)
            {
            int sc = gameManager.scores[key];
                if (sc > 0 || sortedKeys.ElementAt(0)==key) scoreUpdater.DoMagic(i++, key.playerProfile.name,sc);
            }
        //    temp = new List<Character>();
           // temp = sortedKeys.ToList();
       // }
        /*var highscore=gameManager.scores.Aggregate((x, y) => x.Value > y.Value ? x : y);
        return highscore.Key + string.Format(" Scored: {0:00}" , highscore.Value);*/
    }

    bool newCharacterUnlocked = false;
    int playerScore;
    int playerPos;
    int playerLastXP;
    public int TopFiveCoins,TopThreeCoins,TopCoins;
    bool giveCoins=false; int coinReward;
    public void GameOver()
    {
        var sortedKeys = gameManager.scores.OrderByDescending(x => x.Value);
        gameoverScreen.SetActive(true);
        playerScore = gameManager.scores[gameManager.player];
        playerLastXP = PlayerPrefs.GetInt("PlayerXP");
        for (int i = 0; i < sortedKeys.Count(); i++)
        {
            Instantiate(playerRankBtn, playerRankParent).UpdateInfo(rankPositions[i], sortedKeys.ElementAt(i).Key.playerProfile.name, sortedKeys.ElementAt(i).Key.playerProfile.rank.ToString(),sortedKeys.ElementAt(i).Value.ToString(),(float)i/3);
            if (!sortedKeys.ElementAt(i).Key.isEnemy) playerPos = i+1;
        }
        if (playerScore >= sortedKeys.ElementAt(4).Value)
        {
            PlayerPrefs.SetInt("PlayerXP", PlayerPrefs.GetInt("PlayerXP")+10);
            PlayerPrefs.SetInt("PlayerTop5", PlayerPrefs.GetInt("PlayerTop5") + 1);
            coinReward=TopFiveCoins;
            if (playerScore >= sortedKeys.ElementAt(2).Value)
            {
                PlayerPrefs.SetInt("PlayerXP", PlayerPrefs.GetInt("PlayerXP") + 20);
                PlayerPrefs.SetInt("PlayerTop3", PlayerPrefs.GetInt("PlayerTop3") + 1);
                coinReward = TopThreeCoins;
                if (playerScore > sortedKeys.ElementAt(1).Value)
                {
                    PlayerPrefs.SetInt("PlayerXP", PlayerPrefs.GetInt("PlayerXP") + 30);
                    PlayerPrefs.SetInt("PlayerTop1", PlayerPrefs.GetInt("PlayerTop1") + 1);
                    coinReward = TopCoins;
                }
            }
            giveCoins = true;
        }
        if (PlayerPrefs.GetInt("PlayerXP") > 99)
        {
            PlayerPrefs.SetInt("PlayerRank", PlayerPrefs.GetInt("PlayerRank")+1);
            PlayerPrefs.SetInt("PlayerXP", 1);
            newCharacterUnlocked = true;
        }
        positionText.transform.parent.gameObject.SetActive(giveCoins);
        PlayerPrefs.SetInt("PlayerFights", PlayerPrefs.GetInt("PlayerFights") + 1);
    }
    public TextMeshProUGUI scoreT, PosCoins,EarnedCoins,TotalCoins, positionText;
    public Image xpBar;
    public AudioClip statsCounter,statsCounter2;
    public void ShowStats()
    {
        if (newCharacterUnlocked)
        {
            characterUnlock.ShowCharacterInfo(PlayerPrefs.GetInt("PlayerRank") - 1);
            characterUnlock.gameObject.SetActive(true);
            newCharacterUnlocked = false;
        }
        else
        {
            characterUnlock.gameObject.SetActive(false);
            statsScreen.SetActive(true);
            scoreT.text = "00";
            StartCoroutine(Stats());
        }
    }
    IEnumerator Stats()
    {
        /*if (PlayerPrefs.GetInt("PlayerXP") < 1)
            playerLastXP = 0;*/
        xpBar.fillAmount = (float)playerLastXP / 100;
        yield return new WaitForSeconds(0.5f);
        AudioManager.inst.PlayPopup(statsCounter, true);
        for (int i =0; i <= playerScore;i++)                   //Score
        {
            scoreT.text = i.ToString();
            if (i > 111 ) i += playerScore/10;
            yield return new WaitForFixedUpdate();
        }
        AudioManager.inst.PausePopup();

        yield return new WaitForSeconds(0.75f);
        AudioManager.inst.UnPausePopup();
        for (int i = 0; i <= playerPos; i++)         //Position
        {
            positionText.text = i.ToString();
            yield return new WaitForFixedUpdate();
        }
        positionText.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutBounce);
        AudioManager.inst.PausePopup();

        yield return new WaitForSeconds(0.75f);
        AudioManager.inst.UnPausePopup();

        if (giveCoins)
        {
            for (int i = 0; i <= coinReward; i++)       //pOSTION COINS
            {
                PosCoins.text = i.ToString();
                if (i > 111) i += coinReward / 10;
                yield return new WaitForFixedUpdate();
            }
            AudioManager.inst.PausePopup();
            yield return new WaitForSeconds(0.75f);
            AudioManager.inst.UnPausePopup();
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin")+coinReward);
        }
        
        int gameplayCoins = GameManager.Instance.player.sessionCoins;
        for (int i = 0; i <= gameplayCoins; i++)       //Collected COINS
        {
            EarnedCoins.text = i.ToString();
            if (i > 111) i += gameplayCoins / 10;
            yield return new WaitForFixedUpdate();
        }
        AudioManager.inst.PausePopup();
        yield return new WaitForSeconds(0.75f);
        AudioManager.inst.UnPausePopup();

        for (int i = GameManager.Instance.player.beforeSessionCoins; i <= PlayerPrefs.GetInt("Coin"); i++)       //Total COINS
        {
            TotalCoins.text = i.ToString();
            if (i > 111) i += PlayerPrefs.GetInt("Coin") / 10;
            yield return new WaitForFixedUpdate();
        }

        AudioManager.inst.PausePopup();
        yield return new WaitForSeconds(1);

        AudioManager.inst.PlayPopup(statsCounter2, false);
        float ii;
        if (PlayerPrefs.GetInt("PlayerXP") < 20)
            ii = 1f;
        else
            ii = (float)PlayerPrefs.GetInt("PlayerXP", 1) / 100;
        while (xpBar.fillAmount < ii)                               //XPbar
        {
            xpBar.fillAmount += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        AudioManager.inst.PausePopup();
    }
    public void RestartLevel()
    {
       // loadingScreen.sceneNum = SceneManager.GetActiveScene().buildIndex;
        loadingScreen.gameObject.SetActive(true);
    }
    public void UpdateMinesStack(int val)
    {
        mineBtn.enabled = val != 0;
        mineBtnText.text = val.ToString();
    }

    #region Death
    int deadTimer =5;
    public void GiveWarning()
    {
        Warning.SetActive(true);
        InvokeRepeating(nameof(DeathTimer),0,1);
    }
    public void ShutoffWarning()
    {
        Warning.SetActive(false);
    }
    void DeathTimer()
    {
        WarningText.text = deadTimer.ToString();
        deadTimer--;
        if (deadTimer == 0)
        {
            deadTimer = 5;
            CancelInvoke();
        }
    }
    #endregion


    public void EnableDisablePowerButton(bool IsEnable)
    {
        if (IsEnable)
        {
            Color col = AttackPowerBtn.color;
            col.a = 1;
            AttackPowerBtn.color = col;

            col = PowerBtn.color;
            col.a = 1;
            PowerBtn.color = col;
        }
        else
        {
            Color col = AttackPowerBtn.color;
            col.a = 0.5f;
            AttackPowerBtn.color = col;

            col = PowerBtn.color;
            col.a = 0.5f;
            PowerBtn.color = col;
        }
    }

    public void EnableDisablePlayerControls(bool IsEnable)
    {
        Image joystick = playerControl.transform.GetChild(0).GetComponent<Image>();
        Image knob = joystick.transform.GetChild(0).GetComponent<Image>();

        if (IsEnable)
        {

            joystick.enabled = true;
            knob.enabled = true;

        }
        else
        {
            joystick.enabled = false;
            knob.enabled = false;
        }
    }
}