using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;
using static JourneyRewards;
using System;

public class JourneyRewards : MonoBehaviour
{
    public MainMenu menu;
    public Image journeyBar;
    public Button[] journeyBtns;
    public GameObject[] tics;
    public GameObject[] covers;
    public GameObject popup;
    public TextMeshProUGUI popupText;
    public RewardSystem[] rewards;
    int rewardRank;
    void Start()
    {
        rewardRank = PlayerPrefs.GetInt("PlayerRank") - 1;
        journeyBar.fillAmount = rewardRank / 10;
        foreach (Button btn in journeyBtns)
        {
            btn.interactable = false;
        }
        if (PlayerPrefs.GetInt("RankReward" + rewardRank) != 1)
        {
            journeyBtns[rewardRank].transform.DOScale(1.3f, 1).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
            journeyBtns[rewardRank].onClick.AddListener(GiveReward);
            journeyBtns[rewardRank].interactable = true;
        }
        journeyBar.fillAmount = (float)rewardRank / 10;
        for (int i = 0; i <= rewardRank; i++)
        {
            if(i<rewardRank)tics[i].SetActive(true);
            if (PlayerPrefs.GetInt("RankReward" + rewardRank) == 1) tics[i].SetActive(true);
            covers[i].SetActive(false);
        }
    }

    public void GiveReward()
    {
        menu.UITouchedInactive();
        journeyBtns[rewardRank].interactable = false;
        PlayerPrefs.SetInt("RankReward" + rewardRank, 1);
        journeyBtns[rewardRank].transform.DOPause();
        journeyBtns[rewardRank].transform.localScale = Vector3.one;
        tics[rewardRank].SetActive(true);
        ShowPopup();
    }
    void ShowPopup()
    {
        popup.SetActive(true);
        popupText.text = rewards[rewardRank].statement.ToUpper();
        switch (rewards[rewardRank].rewards)
        {
            case Rewards.Coins:
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + rewards[rewardRank].quantity);
                break;
            case Rewards.XP:
                PlayerPrefs.SetInt("PlayerXP", PlayerPrefs.GetInt("PlayerXP") + rewards[rewardRank].quantity);
                break;
            case Rewards.Dummy:
                PlayerPrefs.SetInt("Env" ,rewardRank+1);
                break;
            case Rewards.Character:
                break;
            case Rewards.XPxCoin:
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + rewards[rewardRank].quantity);
                PlayerPrefs.SetInt("PlayerXP", PlayerPrefs.GetInt("PlayerXP") + 15);
                break;
        }
        menu.FirstTimeThingsDone();
    }

    public enum Rewards
    {
        Coins,
        XP,
        XPxCoin,
        Character,
        Dummy

    }
}
[Serializable]
public class RewardSystem
{
    public Rewards rewards;
    public int quantity;
    public int index;
    public string statement;
}