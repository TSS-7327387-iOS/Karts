using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
//using UnityEngine.InputSystem.EnhancedTouch;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using MoreMountains.NiceVibrations;
using Random = UnityEngine.Random;

using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SmoothLoading loading;
    public GameObject mainMenu,playerProfile,matchPlayerScreen;
    public Button playBtn;
    public GameObject journeyNotification,journeyHand;
    //public Sprite[] mPScreenPics;
    public GameObject[] mPScreenPics;
   // public GameObject[] mPScreenPics;
    public Image mPImage;
    public Image settingsPanel,generalBtn,supportBtn;
    public Sprite settingsG, settingsS, greenBtn,purpleBtn;
    public GameObject generalSettings, supportSettings;
    public TextMeshProUGUI nameInputText,nameTextHolder;
    public AudioSource audioSource, audioSourceSound;
    public Slider sound, bgm;
    public TextMeshProUGUI playerRankMain,playerRankProfile,firstPlaceProfile,topThreeProfile,topFiveProfile,totalBattles;
    public Text coinsText;
    public Image profileRankBar, profileRankBarMain;
    public CharacterSelection characterSelection;
    public Toggle viberator;
    public int[] sceneIndices; // Assign scene build indices in the Inspector
    private int currentIndex;
    public GameObject ModeOffPopup;
    public GameObject Sound;
    public GameObject DifficultyPanel;
    public static MainMenu inst;

    [Header("Settings")]
    [SerializeField] private GameObject musicBtnOn;
    [SerializeField] private GameObject musicBtnOff;
    [SerializeField] private GameObject soundBtnOn;
    [SerializeField] private GameObject soundBtnOff;
    [SerializeField] private GameObject vibrationBtnOn;
    [SerializeField] private GameObject vibrationBtnOff;


    [SerializeField] private GameObject modeSelectionPanel;
    
    public TMP_InputField[] lapInputFields;
    
    public TMP_InputField nameChangeInput; // drag your “Change Name” input here in Inspector
    public TextMeshProUGUI savedNameDisplay;
    public GameObject editNamePanel;
    
    [Header("Lap Selector")]
    public TextMeshProUGUI[] lapCountTexts; // One for each environment popup
    private int[] lapCounts = new int[5];
    
    [Header("Gameplay Environments")]
    public GameObject[] gameplayEnvironments;
    
    [Header("Skybox Manager")]
    public SceneSelectorManager skyboxManager;

    private const int minLap = 1;
    private const int maxLap = 5;
    
    [Header("First Play Settings")]
    [Tooltip("0-based index of the environment to show the very first time the player ever plays.")]
    [SerializeField] private int firstPlayIndex = 5;

// PlayerPrefs key to track that the forced first-play environment has already been used.
    private const string FirstPlayDoneKey = "MM_FirstPlayDone";

    private readonly string[] lapKeys = {
        "Laps_Snow", "Laps_Mountain", "Laps_Desert", "Laps_Space", "Laps_Circuit"
    };
    

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        Sound.SetActive(AudioHandler.Instance.soundT.isOn);
        
        if (!PlayerPrefs.HasKey("PlayerRank"))
        {
            PlayerPrefs.SetInt("PlayerRank", 1);
            mainMenu.SetActive(false);
            playerProfile.SetActive(true);
            characterSelection.PlayPopupSound();
            PlayerPrefs.SetInt("PlayerUnlocked" + 0, 1);
        }
        UpdateUserData();
        viberation();

        InitializeSettings();
        //--------------Test----------
        // if (!PlayerPrefs.HasKey("Coin"))
        // PlayerPrefs.SetInt("Coin", 500000);
        
        string savedName = PlayerPrefs.GetString("PlayerName", "Player");
        savedNameDisplay.text =  savedName;
        InitLapCounts();
        
      //  coinsText.text = CurrencyManager.instance.GetSavedCoins().ToString();
        
       // currentIndex = PlayerPrefs.GetInt("SceneIndex", 0);
       
       TssAdsManager._Instance.admobInstance.ShowLeftBanner();
       TssAdsManager._Instance.admobInstance.ShowRightBanner();
       TssAdsManager._Instance.admobInstance.ShowRecBanner();
    }
    private void OnEnable()
    {
        CurrencyManager.coinsChangedEvent += UpdateCoinsDisplay;
       journeyNotification.SetActive(PlayerPrefs.GetInt("RankReward" + (PlayerPrefs.GetInt("PlayerRank") - 1)) != 1);
    }

    public void coinshowtext()
    {
        coinsText.text = CurrencyManager.instance.GetSavedCoins().ToString();
    }

    public void rateus()
    {
        Application.OpenURL("https://sites.google.com/view/privacy-policy-wali-murtaza/home");
    }

    public void moregames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=7098810490512159845");
    }
    
    public void InitLapCounts()
    {
        for (int i = 0; i < lapCounts.Length; i++)
        {
            lapCounts[i] = 2; // default
            if (lapCountTexts[i] != null)
                lapCountTexts[i].text = lapCounts[i].ToString();
        }
    }

    public void IncreaseLap(int index)
    {
        if (index >= lapCounts.Length || index >= lapCountTexts.Length) return;

        lapCounts[index]++;
        if (lapCounts[index] > maxLap)
            lapCounts[index] = minLap;

        if (lapCountTexts[index] != null)
            lapCountTexts[index].text = lapCounts[index].ToString();
    }

    public void DecreaseLap(int index)
    {
        if (index >= lapCounts.Length || index >= lapCountTexts.Length) return;

        lapCounts[index]--;
        if (lapCounts[index] < minLap)
            lapCounts[index] = maxLap;

        if (lapCountTexts[index] != null)
            lapCountTexts[index].text = lapCounts[index].ToString();
    }
    
    public void FirstTimeThingsDone()
    {
        journeyHand.SetActive(false);
        playBtn.interactable = true;
        journeyNotification.SetActive(false);
    }
    private void OnDisable()
    {
        CurrencyManager.coinsChangedEvent -= UpdateCoinsDisplay;
    }
 
    public void viberation()
    {
        PlayerPrefs.SetInt("Viberation",viberator.isOn?1:0);
    }
    public void UITouched()
    {
        if (AudioHandler.Instance.soundT.isOn)
        {
            audioSourceSound.Play();
        }
    }
    
    private void UpdateCoinsDisplay(int amount)
    {
        coinsText.text = amount.ToString();
    }
    
    public void Intshow()
    {
        TssAdsManager._Instance.ShowInterstitial("Interstitial shown");
    }
    
    public void SaveNewName()
    {
        string newName = nameChangeInput.text.Trim();
        if (!string.IsNullOrEmpty(newName))
        {
            PlayerPrefs.SetString("PlayerName", newName);
            savedNameDisplay.text =  newName;
        }

        CloseEditpanel();
    }
    public void UITouchedInactive()
    {
        if (PlayerPrefs.GetInt("Viberation") == 1)
        {
            audioSourceSound.Play();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, false, this);
        }
    }

    public void ShowEditpanel()
    {
        editNamePanel.SetActive(true);
    }
    public void CloseEditpanel()
    {
        editNamePanel.SetActive(false);
    }
    
    public void difficultyShow()
    {
        // laodingpanel.SetActive(true);
        // StartCoroutine("loadingshow");
        DifficultyPanel.SetActive(true);
    }
    public void difficultyClose()
    {
        DifficultyPanel.SetActive(false);
    }

    public IEnumerator loadingshow()
    {
        yield return new WaitForSeconds(3f);
        laodingpanel.SetActive(false);
    }
    
    public void SaveLapInputs()
    {
        // string[] keys = { "Laps_Snow", "Laps_Mountain", "Laps_Desert", "Laps_Space", "Laps_Circuit" };
        //
        // for (int i = 0; i < keys.Length; i++)
        // {
        //     int laps = 2; // Default
        //     if (lapInputFields[i] != null && !string.IsNullOrWhiteSpace(lapInputFields[i].text))
        //     {
        //         if (int.TryParse(lapInputFields[i].text, out int parsed))
        //         {
        //             laps = Mathf.Clamp(parsed, 1, 99); // Optional clamp
        //         }
        //     }
        //     PlayerPrefs.SetInt(keys[i], laps);
        // }
        
        for (int i = 0; i < lapKeys.Length; i++)
        {
            PlayerPrefs.SetInt(lapKeys[i], lapCounts[i]);
        }
    }

    IEnumerator SetRandomSceneAndImage()
    {
       
       
       // yield return new WaitForSecondsRealtime(4f);
       //
       // if (mPScreenPics.Length == 0 || gameplayEnvironments.Length == 0) yield break;
       //
       // // ✅ Pick random environment index
       // int randomIdx = Random.Range(0, mPScreenPics.Length);
       //
       // // Deactivate all preview images first
       // foreach (GameObject pic in mPScreenPics)
       // {
       //     pic.SetActive(false);
       // }
       //
       // // Activate the selected image in Main Menu
       // if (randomIdx < mPScreenPics.Length && mPScreenPics[randomIdx] != null)
       // {
       //     mPScreenPics[randomIdx].SetActive(true);
       //     Image img = mPScreenPics[randomIdx].GetComponent<Image>();
       //     if (img != null)
       //     {
       //         mPImage.sprite = img.sprite;
       //     }
       // }
       //
       // // ✅ Save the environment index for GameplaySceneInitializer
       // PlayerPrefs.SetInt("SceneToLoad", randomIdx);
       // PlayerPrefs.Save();
       //
       // // ✅ Also change the Main Menu skybox immediately for preview
       // if (skyboxManager != null)
       // {
       //     skyboxManager.ActivateSkybox(randomIdx);
       // }
       
       yield return new WaitForSecondsRealtime(4f);

       if (mPScreenPics == null || mPScreenPics.Length == 0) yield break;
       if (gameplayEnvironments == null || gameplayEnvironments.Length == 0) yield break;

       // Decide which index to use.
       int chosenIdx;
       bool firstPlayDone = PlayerPrefs.GetInt(FirstPlayDoneKey, 0) == 1;

       if (!firstPlayDone)
       {
           // Force the configured first-play index (clamped for safety).
           chosenIdx = Mathf.Clamp(firstPlayIndex, 0, mPScreenPics.Length - 1);

           // Mark that we've now shown the first-play environment so future calls randomize.
           PlayerPrefs.SetInt(FirstPlayDoneKey, 1);
           PlayerPrefs.Save();
       }
       else
       {
           // Subsequent plays: pick randomly from the available previews.
           chosenIdx = Random.Range(0, mPScreenPics.Length);
       }

       // Deactivate all preview images first.
       foreach (GameObject pic in mPScreenPics)
       {
           if (pic != null) pic.SetActive(false);
       }

       // Activate the selected image in Main Menu + copy its sprite to the large preview.
       if (mPScreenPics[chosenIdx] != null)
       {
           mPScreenPics[chosenIdx].SetActive(true);
           var img = mPScreenPics[chosenIdx].GetComponent<Image>();
           if (img != null)
           {
               mPImage.sprite = img.sprite;
           }
       }

       // Save the environment index so GameplaySceneInitializer knows which to show.
       PlayerPrefs.SetInt("SceneToLoad", chosenIdx);
       PlayerPrefs.Save();

       // Update the Main Menu skybox immediately for preview.
       if (skyboxManager != null)
       {
           skyboxManager.ActivateSkybox(chosenIdx);
       }

       // Optionally remember locally (if you care about it elsewhere in MainMenu).
       currentIndex = chosenIdx;
    }
       
    
    
    private Coroutine setSceneRoutine;

   
    
    void UpdateUserData()
    {
        //nameTextHolder.text = PlayerPrefs.GetString("PlayerName", "WALI007");
        nameTextHolder.text = PlayerPrefs.GetString("PlayerName", "write your name");
        audioSource.volume = PlayerPrefs.GetFloat("Music", 1);
        sound.value = PlayerPrefs.GetFloat("Sound", 1);
        bgm.value = PlayerPrefs.GetFloat("Music", 1);
        
        coinsText.text = CurrencyManager.instance.GetSavedCoins().ToString();
        //coinsText.text = CurrencyManager.instance.GetCoins().ToString();
        //coinsText.text = PlayerPrefs.GetInt("Coin").ToString();
        
        
        
        playerRankMain.text = playerRankProfile.text = PlayerPrefs.GetInt("PlayerRank").ToString();
        firstPlaceProfile.text = PlayerPrefs.GetInt("PlayerTop1").ToString();
        topThreeProfile.text = PlayerPrefs.GetInt("PlayerTop3").ToString();
        topFiveProfile.text = PlayerPrefs.GetInt("PlayerTop5").ToString();
        totalBattles.text = PlayerPrefs.GetInt("PlayerFights").ToString();
        profileRankBar.fillAmount = (float)PlayerPrefs.GetInt("PlayerXP", 1) / 100;
        profileRankBarMain.fillAmount = (float)PlayerPrefs.GetInt("PlayerXP", 1) / 100;
    }
    public void LoadGameplay()//Calling from main menu play btn
    {
       modeSelectionPanel.SetActive(true);
    }

    public void envShow()
    {
       // StartSetRandomSceneAndImage();
       
       foreach (GameObject pic in mPScreenPics)
       {
           pic.SetActive(false);
       }
       
       // Stop and restart the coroutine to ensure full reset
       if (setSceneRoutine != null)
       {
           StopCoroutine(setSceneRoutine);
       }
       
       setSceneRoutine = StartCoroutine(SetRandomSceneAndImage());
       
       
    }
    
    public void SoundOff(bool val)
    {
        Sound.SetActive(val);
    }
    
    
    public void ShowImage()
    {
        mPImage.transform.parent.gameObject.SetActive(true);
    }
    public void LaodGameplayAfterWait()
    {
        SaveLapInputs();
        
        if (setSceneRoutine != null)
        {
            StopCoroutine(setSceneRoutine);
        }
        
        Invoke(nameof(LoadingGameplay), 0.5f);
        
        // yield return new WaitForSeconds(3f);
        // SceneManager.LoadScene(sceneIndices[randomIdx]);
        
    }

    public GameObject laodingpanel;
    void LoadingGameplay()
    {
       
        matchPlayerScreen.SetActive(false);
        laodingpanel.SetActive(true);
        //TssAdsManager._Instance.ShowInterstitial("Going into gameplay");


       // FlowOrigin.MarkComingFromMainMenu();
        StartCoroutine(sceneload());
        // ✅ Load the single Gameplay scene (NOT multiple environments)
        // SceneManager.LoadScene("Gameplay Karts"); // Replace with actual scene name
    }
    
    

    public IEnumerator sceneload()
    {
       
      
       yield return new WaitForSeconds(3f);
       SceneManager.LoadScene("Gameplay Karts"); // Replace with actual scene name
       
       
       // int sceneToLoad = PlayerPrefs.GetInt("SceneToLoad", -1);
       //
       // SceneManager.LoadScene(sceneToLoad);
    }
    public void CancelLoadGameplay()
    {
        matchPlayerScreen.SetActive(false);
        CancelInvoke(nameof(LoadingGameplay));
    }

    public void PopupShow()
    {
        ModeOffPopup.SetActive(true);
    }
    public void PopupClose()
    {
        ModeOffPopup.SetActive(false);
    }
    
    public void SettingsClicked(bool isGeneral)
    {
        
        generalSettings.SetActive(isGeneral);
        supportSettings.SetActive(!isGeneral);
    }
    public void SetPlayerName()
    {
        // string cleanedName = nameInputText.text.Trim();
        //
        // if (string.IsNullOrEmpty(cleanedName))
        // {
        //     cleanedName = "Player"; // fallback to default name if empty
        // }
        //
        // PlayerPrefs.SetString("PlayerName", cleanedName);
        // PlayerPrefs.Save();
        //
        // Debug.Log("Player name saved: " + cleanedName);
        
        string cleanedName = nameInputText.text.Trim();

        if (string.IsNullOrEmpty(cleanedName))
        {
            cleanedName = "Player"; // fallback to default name if empty
        }

        PlayerPrefs.SetString("PlayerName", cleanedName);
        PlayerPrefs.Save();

        Debug.Log("Player name saved: " + cleanedName);

        // ✅ Also update the always-visible display
        savedNameDisplay.text = "Your Name: " + cleanedName;
    }

    public void sounds()
    {
        audioSourceSound.volume = sound.value;
        PlayerPrefs.SetFloat("Sound", sound.value);
    }
    public void BGM()
    {
        audioSource.volume = bgm.value;
        PlayerPrefs.SetFloat("Music", bgm.value);
    }


    private void InitializeSettings()
    {
        if(!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetFloat("Sound", 1);

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetFloat("Music", 1);

        if (!PlayerPrefs.HasKey("Viberation"))
            PlayerPrefs.SetInt("Viberation", 1);


        if(PlayerPrefs.GetFloat("Sound") == 1)
        {
          //  soundBtnOff.SetActive(false);
         //   soundBtnOn.SetActive(true);

            audioSourceSound.volume = 1;
        }
        else
        {
         //   soundBtnOff.SetActive(true);
         //   soundBtnOn.SetActive(false);

            audioSourceSound.volume = 0;
        }

        if (PlayerPrefs.GetFloat("Music") == 1)
        {
         //   musicBtnOff.SetActive(false);
          //  musicBtnOn.SetActive(true);

            audioSource.volume = 1;
        }
        else
        {
         //   musicBtnOff.SetActive(true);
           // musicBtnOn.SetActive(false);

            audioSource.volume = 0;
        }

        if (PlayerPrefs.GetInt("Viberation") == 1)
        {
           // vibrationBtnOff.SetActive(false);
            //vibrationBtnOn.SetActive(true);
        }
        else
        {
          //  vibrationBtnOff.SetActive(true);
          //  vibrationBtnOn.SetActive(false);
        }

    }

    public void SoundEnableDisable(bool IsEnable)
    {
        if (IsEnable)
        {
            soundBtnOff.SetActive(false);
            soundBtnOn.SetActive(true);

            PlayerPrefs.SetFloat("Sound", 1);
            audioSourceSound.volume = 1;
        }
        else
        {
            soundBtnOff.SetActive(true);
            soundBtnOn.SetActive(false);

            PlayerPrefs.SetFloat("Sound", 0);
            audioSourceSound.volume = 0;
        }
    }


    public void MusicEnableDisable(bool IsEnable)
    {
        if (IsEnable)
        {
            musicBtnOff.SetActive(false);
            musicBtnOn.SetActive(true);

            PlayerPrefs.SetFloat("Music", 1);
            audioSource.volume = 1;
        }
        else
        {
            musicBtnOff.SetActive(true);
            musicBtnOn.SetActive(false);

            PlayerPrefs.SetFloat("Music", 0);
            audioSource.volume = 0;
        }
    }

    public void VibrationEnableDisable(bool IsEnable)
    {
        if (IsEnable)
        {
            vibrationBtnOff.SetActive(false);
            vibrationBtnOn.SetActive(true);

            PlayerPrefs.SetFloat("Viberation", 1);
        }
        else
        {
            vibrationBtnOff.SetActive(true);
            vibrationBtnOn.SetActive(false);

            PlayerPrefs.SetInt("Viberation", 0);
        }
    }

}
