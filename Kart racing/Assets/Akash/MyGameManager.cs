using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager instance;
    [Header("Player Spawned")]
    //public List<GameObject> player_Spawned;
    public GameObject player_Spawned;
    public PlayerAllRef playerAllRef_Script;
    public Kart player_Kart_Script;
    [Header("Enemies Spawned")]
    public List<GameObject> Enemies_Spawned;
    [Header("CutScene Related")]
    public Camera mainKartCamera;
    public GameObject cutSceneCamera_Go;
    [Header("Canvas & Input Related")]
    public GameObject canvas_Input;
    public InputManager inputManager;
    [Header("Camera Follow Related")]
    public CameraFollowSmoothKart cameraFollowSmoothKart;
    [Header("Waypoints Manager")]
    public WaypointManager waypointManager;

    [Header("GameOver Panel")]
    public GameObject gameOverPanel;
    public bool isPlayerExploded;
    [Header("Lap Counter Player")]
    public LapCounter playerLapCounter;
    public List<LapCounter> enemiesLapCounter;
    [Header("Btns test")]
    public UnityEngine.UI.Button damageTest_Btn;
    public UnityEngine.UI.Button resetHealth_Btn;
    [Header("Player 1st Spwan Point")]
    public Transform player1st_SpawmPoint;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

    }
    #region stop all character for cutscene
    public void onPlayerAdded_StopPlayer()
    {
        /* foreach (var player in player_Spawned)
         {
             player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
         }*/
        player_Spawned.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        playerAllRef_Script = player_Spawned.gameObject.GetComponent<PlayerAllRef>();
        //cameraFollowSmoothKart.target = player_Spawned[0].transform;
        cameraFollowSmoothKart.target = player_Spawned.transform;
        Debug.Log("onPlayerAdded stop the player at start");
    }
    public void onEnemiesAdded_StopEnemies()
    {
        foreach (var enemy in Enemies_Spawned)
        {
            enemy.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            enemy.gameObject.GetComponent<Kart>().active = false;
        }
        Debug.Log("onEnemyAdded stop the enemy at start");
    }
    #endregion stop all character for cutscene
    public void call_playStart_CutScene()
    {
        Debug.Log("call_playStart_CutScene");
        StartCoroutine(playStart_CutScene());
    }

    public IEnumerator playStart_CutScene()
    {
        enable_Disable_InputCanvas(false);
        mainKartCamera.enabled = false;
        cutSceneCamera_Go.SetActive(true);
        yield return new WaitForSeconds(10f);
        // setCameraPlayEffect();
        
        // // ✅ ADD THIS:
        // if (GameController.instance != null)
        // {
        //     GameController.instance.Initialize();
        // }

    }
    public void enable_Disable_InputCanvas(bool isEnable)
    {
        canvas_Input.SetActive(isEnable);
    }
    /* public void setCameraPlayEffect()
     {

       //  CameraPlay.FadeInOutCS();
         //CameraPlay.BlackWhite_ON();
         CameraPlay.MangaFlash();

         Debug.Log("setCameraPlayEffect");
     }*/
    public void startGame()
    {
        mainKartCamera.enabled = true;
        foreach (var enemy in Enemies_Spawned)
        {
            enemy.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            enemy.gameObject.GetComponent<Kart>().active = true;
            enemiesLapCounter.Add(enemy.gameObject.GetComponent<LapCounter>());
        }

        player_Spawned.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        enable_Disable_InputCanvas(true);
        playerLapCounter = player_Spawned.gameObject.GetComponent<LapCounter>();
        player_Kart_Script = player_Spawned.gameObject.GetComponent<Kart>();
        damageTest_Btn.onClick.AddListener(onClickDamage_Test);
        resetHealth_Btn.onClick.AddListener(onClickResetHealth_Test);
        Debug.Log("StartGame Called");
    }
    void onClickDamage_Test()
    {
        player_Kart_Script.takeDamage_HealthSlider(0.2f);
    }
    void onClickResetHealth_Test()
    {
        player_Kart_Script.resetHealth();
    }
    [Header("NOS custom Boost")]
    public float boostToAdd;
    public float boostForce;

    public float boostToRemove;
    public float boostForceRemove;

    public float fovValue;
    public float fovValueReset;

    public void call_showGameOverPanel()
    {
        StartCoroutine(showGameOverPanel());
    }
    public IEnumerator showGameOverPanel()
    {
        yield return new WaitForSeconds(2f);
        if (isPlayerExploded)
        {
          gameOverPanel.SetActive(true);
        }
    }
    public void callSpin()
    {
        if (PlayerAllRef.instance != null)
        {
            PlayerAllRef.instance.kartRef_Script.SpinOutCustom(Kart.SpinAxis.Roll, 1);
            Debug.Log("callSSpinOut");
        }
        
        
    }
    public void callBoost()
    {
        if (PlayerAllRef.instance != null)
        {
            PlayerAllRef.instance.kartRef_Script.AddBoostCustom(boostToAdd, boostForce);
        }
        callDoFov();
        setCameraPlayEffect();
        playParticles();

        Debug.Log("CallAddBoostCustom");
    }
    public void CancelBoost()
    {
        if (PlayerAllRef.instance != null)
        {
             PlayerAllRef.instance.kartRef_Script.AddBoostCustom(boostToRemove, boostForceRemove);
        }
       
        callDoFovReset();
        stopParticles();
        unsetCameraPlayEffect();
        StartCoroutine(stopBoost()); 
        Debug.Log("CallCancelBoostCustom");
    }
    public IEnumerator stopBoost()
    {
        inputManager.SetBrakeMobile(1f);
        yield return new WaitForSeconds(0.1f);
        inputManager.SetBrakeMobile(0f);
    }
    public void callDoFov()
    {
        if (KartCamera.instance != null)
        {
            KartCamera.instance.DoFov(fovValue);
        }
      
        Debug.Log("callDoFov");

    }
    public void callDoFovReset()
    {
        if (KartCamera.instance != null)
        {
            KartCamera.instance.DoFov(fovValueReset);
        }
        Debug.Log("callDoFov");

    }
    public void setCameraPlayEffect()
    {
        //KartCamera.instance.cameraPlay.
        //  CameraPlay.MangaFlash(1, 2, 3, 3);
        //CameraPlay.MangaFlash(0.5f, 0.5f, 1f, 2, Color.grey);
        if (KartCamera.instance != null)
        {
            KartCamera.instance.playSpeedParticles();
        }

        Debug.Log("setCameraPlayEffect");

    }
    public void unsetCameraPlayEffect()
    {
        //KartCamera.instance.cameraPlay.
        //  CameraPlay.MangaFlash(1, 2, 3, 3);
        //CameraPlay.MangaFlash(0.5f, 0.5f, 1f, 2, Color.grey);
        if (KartCamera.instance != null)
        {
            KartCamera.instance.stopSpeedParticles();
        }

        Debug.Log("unsetCameraPlayEffect");

    }
    public void playParticles()
    {
        if (PlayerAllRef.instance != null)
        {

        }
        PlayerAllRef.instance.kartRef_Script.playCustomParticles();
        Debug.Log("playParticles");
    }
    public void stopParticles()
    {
        if (PlayerAllRef.instance != null) {
        
        }
        PlayerAllRef.instance.kartRef_Script.stopCustomParticles();
        Debug.Log("stopParticles");

    }
    

}
