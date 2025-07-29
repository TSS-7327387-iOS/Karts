
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class GameManager : MonoBehaviour
{
    public EnemyManager enemyManager;
    public UIManager ui;
    public float gameTime;
    public Player[] playersObj;
    public Transform[] playerSpwanPoints;
    public ThirdPersonController thirdPersonController;
    /*public delegate void PlayerPickedBall(Transform target);
    public event PlayerPickedBall BallPicked;*/
    public static GameManager Instance;
    public PickUp Ball;
    public Player player;
    public Dictionary<Character, int> scores;
    public bool ballTaken = false;
    public bool playerHasBalls=false;
    public bool gameAlive=false;
    public CameraController cameraController;
    public PowerUpsManager powerUpsManager;
    public GameObject bots;
    public bool spwanBots;
    public AudioSource tictic;
    public GameObject env;
    public GameObject powerVfx;
    public GameObject powerVfxGiant;


    private void Awake()
    {
        Instance = this;
        env.SetActive(true);
        InitializePlayers();
    }
    void InitializePlayers()
    {
        player = Instantiate(playersObj[PlayerPrefs.GetInt("Player", 0)], thirdPersonController.transform);
        player.transform.localPosition = Vector3.zero;
        thirdPersonController.player = player;
        thirdPersonController.animator = player.animator;
        player.controller = thirdPersonController;
        cameraController.player = player.camPosition;
        player.power.moveController = thirdPersonController;
        scores = new Dictionary<Character, int>
        {
            { player, 0 }
        };
    }
    private void Start()
    {
        
        thirdPersonController.transform.position = playerSpwanPoints[Random.Range(0,playerSpwanPoints.Length-1)].position;
        thirdPersonController.gameObject.SetActive(true);
        ui.ShowGameTime(gameTime);
        //BallPicked += BallIsPicked;
        //SpwanPickUps
    }
    public void StartGame()
    {
        enemyManager.GameStarted();
        gameAlive = true;
        if(spwanBots) bots.SetActive(true);
        Ball.gameObject.SetActive(true);
    }
    public void Viberate()
    {
        if (PlayerPrefs.GetInt("Viberation") == 1)
            MMVibrationManager.Haptic(HapticTypes.SoftImpact, false);
        print("djhfgdjhfgd");
    }
    private void Update()
    {
        if (gameAlive)
        {
            if (gameTime > 0)
            {
                gameTime -= Time.deltaTime;
                ui.ShowGameTime(gameTime);
                if(gameTime < 11 && !AudioManager.inst.isLastSecondsSoundPlaying)
                {
                    ui.ShowRedTimer();
                    AudioManager.inst.LastSecondsAudio(true);
                }
            }
            else
            {
                AudioManager.inst.LastSecondsAudio(false);
                EndMatch();
            }
            if (ballTaken)
            {
                if (playerHasBalls)
                {

                   // if (!scores.ContainsKey(player)) scores.Add(player, 0);
                    scores[player]++;
                    ui.SetScoreBoard(player._name);
                }
                else
                {
                   /* if (scores.ContainsKey(enemyManager.enemyWithBall))
                    {*/
                        scores[enemyManager.enemyWithBall]++;
                        ui.SetScoreBoard(enemyManager.enemyWithBall._name);
                   /* }
                    else
                        scores.Add(enemyManager.enemyWithBall, 1);*/
                }
            }
        }
    }
    #region Ball methods
    [HideInInspector]
    public Transform ballHolder;
    public void BallIsPicked(Transform target)
    {
        ballHolder = target;
        Ball.indi.transform.parent = target;
        Ball.indi.transform.localPosition = Vector3.zero;
        Ball.indi.onScreenSpriteHide = true;
        enemyManager.chganeEnemiesState(target);
        ballTaken = true;
    }

    public void BallDropped()
    {
        enemyManager.enemyWithBall = null;
        Ball.indi.transform.parent = Ball.transform;
        Ball.indi.transform.localPosition = Vector3.zero;
        Ball.indi.onScreenSpriteHide = false;
        enemyManager.chganeEnemiesStateToFindBall();
        ballTaken = false;
        if (Ball.gameObject.layer != 0)
        {
            SetLayerRecursively(Ball.gameObject,0);
        }
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform t in obj.transform)
        {
            SetLayerRecursively(t.gameObject, newLayer);
        }
    }
    #endregion

    public void PlayerPickedBall(Transform holder)
    {
        Ball.transform.parent = holder;
        player.canPlayerPick = false;
        // GameManager.Instance.Ball.GetComponent<Rigidbody>().isKinematic=true;
        Ball.transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        playerHasBalls = true;
    }
    public void playerDroppedBall(Vector3 dir)
    {
        Ball.transform.parent = null;
        Ball.rb.isKinematic = false;
        Ball.rb.AddForce(dir * 2, ForceMode.VelocityChange);
        Ball.setCollision(); //.coll(true);
        ballTaken = false;
        playerHasBalls = false;
        Ball.holdedByPlayer = false;
        if (Ball.gameObject.layer != 0)
        {
            SetLayerRecursively(Ball.gameObject, 0);
        }
        player.ActivateCrown(false);

        //My Change
        //player.canPlayerPick = true;
    }
    public void BotDeath(BotAI bot)
    {
        enemyManager.BotDied(bot);
        if (player.bot == bot)
        {
            player.bot = null;
            player.attackBot = false;
            player.kickBot = false;
        }
    }

    public void PlayerDied()
    {
        ////thirdPersonController.gameObject.SetActive(false);

        //MyChange
        player.animator.SetTrigger("Death");
        player.animator.SetBool("BotAttack", false);
        UIManager.Instance.EnableDisablePlayerControls(false);
        CancelInvoke();

        print("_________________________ Player Dead ________________");

        if (playerHasBalls && ballTaken)
        {
            
            playerDroppedBall(Vector3.forward);
            Ball.gameObject.SetActive(true);
            player.defultTrophy.SetActive(false);
        }

        

        Invoke(nameof(RespwanPlayer),5);
    }
    void RespwanPlayer()
    {
        //MyChange
        thirdPersonController.gameObject.SetActive(false);

        ui.ShutoffWarning();
        player.transform.parent.position = playerSpwanPoints[Random.Range(0, playerSpwanPoints.Length - 1)].position;
        player.isAlive = true;
        thirdPersonController.gameObject.SetActive(true);
        player.ResetHealth();


        //MyChange

        //player.animator.SetTrigger("Idle");
        player.animator.SetBool("Idle", true);
        UIManager.Instance.EnableDisablePlayerControls(true);
        thirdPersonController.transform.position = playerSpwanPoints[Random.Range(0, playerSpwanPoints.Length - 1)].position;
        player.animator.SetBool("Idle", false);
    }

    public void EndMatch()
    {
        gameAlive = false;
        CancelInvoke();
        ui.GameOver();
        enemyManager.GameStopped();
        bots.SetActive(false);
        Ball.indi.gameObject.SetActive(false);
    }

    public void RefillPower(Vector3 position)
    {
        print("Refill Power");
        Instantiate(powerVfx, position, Quaternion.identity);
        Invoke(nameof(FillPowerBar), 1.5f);
    }

    public void FillPowerBar()
    {
        UIManager.Instance.PowerBtn.fillAmount += 0.2f;
        print("__________________ fill __________________");
    }

    public void RefillPowerGiant(Vector3 position)
    {
        print("Refill power giant");
        Instantiate(powerVfxGiant, position, Quaternion.identity);
        Invoke(nameof(FillPowerBarGiant), 1.5f);
    }

    public void FillPowerBarGiant()
    {
        UIManager.Instance.PowerBtn.fillAmount += 0.35f;
        print("__________________ fill __________________");
    }
}
