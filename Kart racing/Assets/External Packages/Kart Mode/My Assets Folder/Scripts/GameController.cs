 using System;
 using System.Collections;
 using System.Collections.Generic;
 using System.Linq;
 using PowerslideKartPhysics;
 using TMPro;
 using UnityEngine;
 using UnityEngine.UI;
 using DG.Tweening;
 using UnityEngine.SceneManagement;

 public class GameController : MonoBehaviour
{
//     #region DataFields
//
//     public GameObject env;
//     [Header("UI")] public GameObject wrongDirWarning;
//     public TextMeshProUGUI currentLapText;
//     [SerializeField] TextMeshProUGUI totalLapText;
//     public TextMeshProUGUI currentPlayerRankText;
//     public List<PowerUp> collectedPowerUps = new List<PowerUp>();
//     public Button[] powerUpButtons;
//
//
//     [Space(5f)] [Header("References")] [SerializeField]
//     Karts playerKart;
//
//     public LevelData levelData;
//     [SerializeField] KartCamera kartCamera;
//     [SerializeField] KartCamera kartCameraRear;
//     [SerializeField] UIControl uIControl;
//     [SerializeField] BasicWaypoint basicWaypoint; //Waypoint Reference For AI Karts
//     [SerializeField] MinimapManager minimapManager;
//
//     [Space(5f)] [Header("Debug")] [SerializeField]
//     int currentPlayerKart;
//
//     public GameObject player;
//     public GameObject[] opponentKarts;
//     private Kart kart;
//     public WaypointManager waypointManager;
//
//     public List<ItemData> itemData;
//     public ItemCaster itemCaster;
//     public int powerBtnCount = 0;
//     
//    // public SmoothLoading loading; // Reference to your loading screen or loader
//     public int[] sceneIndices;
//     public GameObject[] nextLevelPreviewImages;
//     private int selectedSceneIndex = -1;
//     
//     
//     #endregion
//
//     public GameObject Pause;
//     public GameObject finishPanel;
//     public GameObject finishPanel2;
//
//     public TextMeshProUGUI[] rankTexts;
//     public TextMeshProUGUI[] rankTextsPanel2;
//
//     private List<int> activePowerUpIndexes = new List<int>();
//     private int maxPowerUpsOnScreen = 3;
//     // private Queue<int> powerUpQueue = new Queue<int>(); 
//
//     //private Dictionary<int, ItemData> buttonItemMap = new Dictionary<int, ItemData>();
//
//     [Header("Podium Setup")]
//     public Transform[] podiumSlots = new Transform[5]; // Where ranked karts appear
//     public GameObject[] playerPreviewPrefabs; // Player kart prefabs for podium
//     public GameObject[] enemyPreviewPrefabs;  // Enemy kart prefabs for podium
//
//     private GameObject[] podiumPreviewInstances = new GameObject[5];
//     
//     [Header("Sprite Podium Setup")]
//     public Sprite[] playerPreviewSprites;
//     public Sprite[] enemyPreviewSprites;
//     public Transform[] spriteDisplayPositions; // UI Image GameObjects with Image component
//     public Transform[] spriteDisplayPositionsPanel2;
//
//     private GameObject[] podiumSpriteInstances = new GameObject[5];
//     public static GameController instance;
//     public GameObject sound;
//     
//     [Header("Loading Screen")]
//     //[SerializeField] private SmoothLoading smoothLoading; // Drag your SmoothLoading prefab/UI in Inspector
//     [SerializeField] public GameObject loadingScreen; 
//     
//     [Header("Player Image Display")]
//     public Sprite[] playerImages;          // Match order with kartObject
//     public Image playerImageDisplayPoint;
//
//     public Text Cointext;
//     
//     public Text savedCoinsText;
//     public Text collectedCoinsText;
//     public Text totalCoinsText;
//     public Text totalCoinsText2; 
//     public Text bonusCoinsText;
//     
//   //  public GameObject[] levelGameObjects; // assign level "containers" here
//     private int selectedLevelIndex = -1;
//     
//     private bool hasBeenInitialized = false;
//    // private int selectedSubIndex = 0;  // Local sub-level index
//     [Header("Level Objects")]
//     public GameObject[] levelGameObjects; // old local sub-level containers
//    // private int selectedLevelIndex = -1;
//
//     // === LoadNextLevel Environment & Skybox Control ===
//     [Header("Next-Level Flow (LoadNextLevel)")]
//     public GameObject[] nextLevelEnvironments;  // root env objects to activate on next level
//    // public GameObject[] nextLevelPreviewImages; // UI preview images for GameOver panel
//     public Material[] nextLevelSkyboxes;        // skyboxes matching envs
//
//     // PlayerPrefs keys
//     private const string NEXT_ENV_KEY = "NextLevelEnvIndex";
//
//     // Local sub-level support (if you still want to keep it)
//     private const string NEXT_SUB_KEY = "NextSubIndex";
//     private int selectedSubIndex = -1;
//
//     #region Methods
//
//     private void Awake()
//     {
//         instance = this;
//         
//        
//         
//         Time.timeScale = 1f;
//         ActivateNextLevelEnvFromPrefs();
//        
//        // if(env) env.SetActive(true);
//     }
//
//     private void Start()
//     {
//         Time.timeScale = 1f;
//         Initialize();
//         LapCounter.finalRankList.Clear();
//         CurrencyManager.instance.ResetLevelCoins();
//         
//         TssAdsManager._Instance.admobInstance.ShowLeftBanner();
//         TssAdsManager._Instance.admobInstance.ShowRightBanner();
//
//     }
//
//     public void Initialize()
//     {
//        //  SpawnPlayerKart();
//        //  SpawnOpponentKarts();
//        //  kartCamera.Initialize(kart);
//        //  kartCameraRear.Initialize(kart);
//        //  uIControl.Initialize(kart);
//        //  minimapManager.InitMinimapData(player.transform, opponentKarts); 
//        //  
//        //  minimapManager.SetupIcons();
//        // // totalLapText.text = levelData.totalLaps.ToString();
//        //
//        //  levelData.totalLaps = GetLapsForCurrentEnvironment();
//        //  totalLapText.text = levelData.totalLaps.ToString();
//        
//        ////////////////////////--------------------------------------------------------//////////////////////////////////////
//        
//        Debug.Log("GameController: Initialize() called.");
//        
//        if (hasBeenInitialized)
//        {
//            Debug.Log("GameController: Clearing old karts before reinitializing.");
//        
//            if (player != null)
//            {
//                Destroy(player);
//                if (MyGameManager.instance != null)
//                    MyGameManager.instance.player_Spawned = null;
//            }
//        
//            if (opponentKarts != null)
//            {
//                foreach (var kart in opponentKarts)
//                {
//                    if (kart != null) Destroy(kart);
//                }
//        
//                if (MyGameManager.instance != null)
//                    MyGameManager.instance.Enemies_Spawned.Clear();
//            }
//        
//            opponentKarts = null;
//        }
//        
//        hasBeenInitialized = true;
//        
//        SpawnPlayerKart();
//        SpawnOpponentKarts();
//        kartCamera.Initialize(kart);
//        kartCameraRear.Initialize(kart);
//        uIControl.Initialize(kart);
//        minimapManager.InitMinimapData(player.transform, opponentKarts); 
//        minimapManager.SetupIcons();
//        
//        levelData.totalLaps = GetLapsForCurrentEnvironment();
//        totalLapText.text = levelData.totalLaps.ToString();
//        
//        Debug.Log("Environment Index: " + PlayerPrefs.GetInt("SceneToLoad", 0));
//        Debug.Log("Loaded Laps: " + levelData.totalLaps);
//        
//       
//     }
//
//     private void SpawnPlayerKart()
//     {
//         // player = Instantiate(playerKart.kartObject[PlayerPrefs.GetInt("Player")], playerKart.spawnTransform.position,
//         //     playerKart.spawnTransform.rotation);
//         // kart = player.GetComponent<Kart>();
//         //
//         // //Set Player Bool for Lap Counter
//         // player.GetComponent<LapCounter>().isPlayer = true;
//         // itemCaster = player.GetComponent<ItemCaster>();
//         // MyGameManager.instance.player_Spawned = player;
//         // Debug.Log("SpawnPlayerKart called :added player to MyGameManager");
//         // MyGameManager.instance.onPlayerAdded_StopPlayer();
//         
//         int selectedIndex = PlayerPrefs.GetInt("Player");
//     
//         player = Instantiate(playerKart.kartObject[selectedIndex],
//             playerKart.spawnTransform.position,
//             playerKart.spawnTransform.rotation);
//
//         kart = player.GetComponent<Kart>();
//
//         // Set Player Bool for Lap Counter
//         player.GetComponent<LapCounter>().isPlayer = true;
//         itemCaster = player.GetComponent<ItemCaster>();
//         MyGameManager.instance.player_Spawned = player;
//         Debug.Log("SpawnPlayerKart called :added player to MyGameManager");
//         MyGameManager.instance.onPlayerAdded_StopPlayer();
//
//         // NEW: Set player image based on selected kart index
//         if (playerImages != null && selectedIndex >= 0 && selectedIndex < playerImages.Length && playerImageDisplayPoint != null)
//         {
//             playerImageDisplayPoint.sprite = playerImages[selectedIndex];
//             playerImageDisplayPoint.gameObject.SetActive(true);
//         }
//     }
//     
//    
//
//     private void SpawnOpponentKarts()
//     {
//         opponentKarts = new GameObject[levelData.totalOpponentKarts];
//
//         List<GameObject> tempKarts = levelData.opponentKarts.ToList();
//         List<Transform> tempSpawnpoints = levelData.opponentKartsSpawnPoints.ToList();
//
//         for (int i = 0; i < levelData.totalOpponentKarts; i++)
//         {
//             int randKart = UnityEngine.Random.Range(0, tempKarts.Count);
//             int randSpawnPoint = UnityEngine.Random.Range(0, tempSpawnpoints.Count);
//
//             opponentKarts[i] = Instantiate(tempKarts[randKart], tempSpawnpoints[randSpawnPoint].position,
//                 tempSpawnpoints[randSpawnPoint].rotation);
//             MyGameManager.instance.Enemies_Spawned.Add(opponentKarts[i]);
//             Debug.Log("SpawnOpponentKarts called :added enemies to MyGameManager");
//             //Assigning Waypoint Reference to AI Karts
//             opponentKarts[i].GetComponent<BasicWaypointFollowerDrift>().targetPoint = basicWaypoint;
//             opponentKarts[i].SetActive(true);
//
//             tempKarts.Remove(tempKarts[randKart]);
//             tempSpawnpoints.Remove(tempSpawnpoints[randSpawnPoint]);
//         }
//
//         tempKarts.Clear();
//         tempSpawnpoints.Clear();
//         MyGameManager.instance.onEnemiesAdded_StopEnemies();
//         MyGameManager.instance.call_playStart_CutScene();
//     }
//
//     public void Intshow()
//     {
//         TssAdsManager._Instance.ShowInterstitial("Interstitial shown");
//     }
//     
//     private int GetLapsForCurrentEnvironment()
//     {
//         // int sceneIndex = SceneManager.GetActiveScene().buildIndex;
//         //
//         // switch (sceneIndex)
//         // {
//         //     case 2: return PlayerPrefs.GetInt("Laps_Snow", 2);
//         //     case 3: return PlayerPrefs.GetInt("Laps_Mountain", 2);
//         //     case 4: return PlayerPrefs.GetInt("Laps_Desert", 2);
//         //     case 5: return PlayerPrefs.GetInt("Laps_Space", 2);
//         //     case 6: return PlayerPrefs.GetInt("Laps_Circuit", 2);
//         //     default: return 2; // fallback if scene is unrelated
//         // }
//         
//         int envIndex = PlayerPrefs.GetInt("SceneToLoad", 0);
//
//         // Match exactly the same order as lapKeys in MainMenu
//         string[] lapKeys = {
//             "Laps_Snow", "Laps_Mountain", "Laps_Desert", "Laps_Space", "Laps_Circuit"
//         };
//
//         if (envIndex < 0 || envIndex >= lapKeys.Length)
//         {
//             Debug.LogWarning("Invalid environment index, defaulting to 2 laps");
//             return 2; // default
//         }
//
//         return PlayerPrefs.GetInt(lapKeys[envIndex], 2);
//     }
//     
//     public void BGMOff(bool val)
//     {
//         sound.SetActive(val);
//     }
//
//     public void pauseGame()
//     {
//         Pause.SetActive(true);
//         Time.timeScale = 0;
//     }
//
//     public void UnpauseGame()
//     {
//         Pause.SetActive(false);
//         Time.timeScale = 1;
//         TssAdsManager._Instance.ShowInterstitial("Un-Puased");
//     }
//     
//     public GameObject CanvasOffMap;
//
//     public void ShowFinishPanel()
//     {
//       finishPanel.SetActive(true);
//       CanvasOffMap.SetActive(false);
//
//       CurrencyManager.instance.MergeCoins();
//
//       int finalPosition = LapCounter.finalRankList.FindIndex(name =>
//       {
//           string playerName = PlayerPrefs.GetString("PlayerName", "Player").Trim();
//           return name == playerName || (name.Contains("Player") && name.Contains("(Clone)"));
//       }) + 1; // 1-based rank
//
//       int lapCount = levelData.totalLaps;
//       int bonusCoins = 0;
//
//       switch (finalPosition)
//       {
//           case 1:
//               bonusCoins = lapCount * 1000;
//               break;
//           case 2:
//               bonusCoins = lapCount * 500;
//               break;
//           case 3:
//               bonusCoins = lapCount * 250;
//               break;
//       }
//
//       CurrencyManager.instance.AddBonusCoins(bonusCoins);
//
//       int saved = CurrencyManager.instance.GetSavedCoins(); // after merge
//       int collected = CurrencyManager.instance.GetLevelCoins();
//       int total = saved;
//
//       int oldSaved = saved - collected;
//
//       savedCoinsText.text = oldSaved.ToString(); // before this level
//       collectedCoinsText.text = collected.ToString();
//       totalCoinsText.text = oldSaved.ToString(); // start from oldSaved
//
//       // ✅ Set bonus coins text explicitly
//       bonusCoinsText.text = bonusCoins.ToString();
//
//       StartCoroutine(AnimateTotalCoins(oldSaved, total));
//
//       ShowRankings();
//       ShowKartsAccordingToRanking();
//       Time.timeScale = 1;
//
//       TssAdsManager._Instance.ShowInterstitial("Game over");
//     }
//     
//     private IEnumerator AnimateTotalCoins(int from, int to)
//     {
//         
//         yield return new WaitForSeconds(1f);
//         
//         float duration = 1.2f;
//         float elapsed = 0f;
//         int lastValue = from;
//
//         while (elapsed < duration)
//         {
//             elapsed += Time.unscaledDeltaTime; // <<<< Use unscaled time here
//             float t = Mathf.Clamp01(elapsed / duration);
//             int current = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
//             totalCoinsText.text = current.ToString();
//
//             if (current != lastValue)
//             {
//                 // Animate bounce effect
//                 totalCoinsText.transform.DOKill();
//                 totalCoinsText.transform.localScale = Vector3.one * 1.3f;
//                 totalCoinsText.transform.DOScale(1f, 0.2f)
//                     .SetEase(Ease.OutBack)
//                     .SetUpdate(true); // <<<< Ensure DOTween also ignores timeScale
//                 lastValue = current;
//             }
//
//             yield return null;
//         }
//
//         totalCoinsText.text = to.ToString(); // final value
//     }
//
//     void ShowRankings()
//     {
//
//         
//         
//         string playerName = PlayerPrefs.GetString("PlayerName", "Player").Trim();
//
//         for (int i = 0; i < rankTexts.Length; i++)
//         {
//             string rawName = i < LapCounter.finalRankList.Count ? LapCounter.finalRankList[i] : "DNF";
//
//             // Fallback
//             if (string.IsNullOrEmpty(rawName)) rawName = "DNF";
//
//             // If this is the player, use the custom name exactly
//             bool isPlayer = rawName == playerName;
//
//             // If this slot contains the player but raw name is the GO name, force swap
//             if (rawName != playerName && rawName.Contains("Player") && rawName.Contains("(Clone)"))
//             {
//                 rawName = playerName;
//                 isPlayer = true;
//             }
//
//             rankTexts[i].text = rawName;
//             rankTexts[i].color = isPlayer ? Color.green : Color.white;
//             rankTexts[i].gameObject.SetActive(true);
//
//             if (rankTextsPanel2 != null && i < rankTextsPanel2.Length)
//             {
//                 rankTextsPanel2[i].text = rawName;
//                 rankTextsPanel2[i].color = isPlayer ? Color.green : Color.white;
//                 rankTextsPanel2[i].gameObject.SetActive(true);
//             }
//         }
//     }
//     
//     public void ShowKartsAccordingToRanking()
//     {
//        
//             for (int i = 0; i < podiumSlots.Length; i++)
// {
//     if (i >= LapCounter.finalRankList.Count) break;
//
//     string rankedName = LapCounter.finalRankList[i];
//     GameObject prefabToSpawn = null;
//     Sprite spriteToDisplay = null;
//
//     string playerName = PlayerPrefs.GetString("PlayerName", "Player");
//     bool isPlayer = rankedName == playerName;
//
//     if (isPlayer)
//     {
//         int playerIndex = PlayerPrefs.GetInt("Player", 0);
//
//         if (playerIndex >= 0 && playerIndex < playerPreviewPrefabs.Length)
//             prefabToSpawn = playerPreviewPrefabs[playerIndex];
//
//         if (playerIndex >= 0 && playerIndex < playerPreviewSprites.Length)
//             spriteToDisplay = playerPreviewSprites[playerIndex];
//     }
//     else
//     {
//         for (int j = 0; j < opponentKarts.Length; j++)
//         {
//             if (opponentKarts[j].name.Contains(rankedName))
//             {
//                 if (j < enemyPreviewPrefabs.Length)
//                     prefabToSpawn = enemyPreviewPrefabs[j];
//
//                 if (j < enemyPreviewSprites.Length)
//                     spriteToDisplay = enemyPreviewSprites[j];
//                 break;
//             }
//         }
//     }
//
//     if (prefabToSpawn != null)
//     {
//         GameObject instance = Instantiate(prefabToSpawn, podiumSlots[i].position, podiumSlots[i].rotation);
//         instance.SetActive(true);
//         instance.transform.SetParent(podiumSlots[i], false);
//         instance.transform.localPosition = Vector3.zero;
//         instance.transform.localRotation = Quaternion.identity;
//         instance.AddComponent<Slowspin>();
//         podiumPreviewInstances[i] = instance;
//     }
//
//     if (spriteToDisplay != null && i < spriteDisplayPositions.Length)
//     {
//         Image img = spriteDisplayPositions[i].GetComponent<Image>();
//         if (img != null)
//         {
//             img.sprite = spriteToDisplay;
//             img.gameObject.SetActive(true);
//         }
//     }
//
//     if (spriteToDisplay != null && i < spriteDisplayPositionsPanel2.Length)
//     {
//         Image img2 = spriteDisplayPositionsPanel2[i].GetComponent<Image>();
//         if (img2 != null)
//         {
//             img2.sprite = spriteToDisplay;
//             img2.gameObject.SetActive(true);
//         }
//     }
//     }
//
//             
//     }
//
//     public void SwitchCamera(bool IsSwitchCamera)
//     {
//         if (IsSwitchCamera)
//         {
//             kartCameraRear.gameObject.SetActive(true);
//             kartCamera.gameObject.SetActive(false);
//         }
//         else
//         {
//             kartCamera.gameObject.SetActive(true);
//             kartCameraRear.gameObject.SetActive(false);
//         }
//     }
//
//     public void UpdatePowerUpUI(string name, ItemData _itemData)
//     {
//         if (_itemData.IsItemAddedAtBtn) return;
//         
//         int targetIndex = -1;
//         
//         //If limit is full, reuse the oldest button
//         if (activePowerUpIndexes.Count >= maxPowerUpsOnScreen)
//         {
//             targetIndex = activePowerUpIndexes[0];
//             RemovePowerUpButton(targetIndex);
//             activePowerUpIndexes.RemoveAt(0);
//         }
//         else
//         {
//             // Find first inactive button
//             for (int i = 0; i < powerUpButtons.Length; i++)
//             {
//                 if (!powerUpButtons[i].gameObject.activeSelf)
//                 {
//                     targetIndex = i;
//                     break;
//                 }
//             }
//         }
//         
//         if (targetIndex != -1)
//         {
//             PowerUp powerUp = collectedPowerUps.FirstOrDefault(p => p.powerName == name);
//             if (powerUp != null)
//             {
//                 Button btn = powerUpButtons[targetIndex];
//                 btn.gameObject.SetActive(true);
//                 btn.GetComponent<Image>().sprite = powerUp.powerBG;
//         
//                 // Start selection animation
//                 StartCoroutine(AnimatePowerUpSelection(btn.transform.GetChild(1).GetComponent<Image>(),
//                     powerUp.powerSprite));
//         
//                 btn.onClick.RemoveAllListeners();
//                 int capturedIndex = targetIndex;
//                 btn.onClick.AddListener(() => UsePowerUp(name, capturedIndex));
//         
//                 activePowerUpIndexes.Add(capturedIndex);
//                 _itemData.IsItemAddedAtBtn = true;
//             }
//         }
//
//         
//         
//
//     }
//
//     private IEnumerator AnimatePowerUpSelection(Image spriteTarget, Sprite finalSprite)
//     {
//         float duration = 0.6f; // Total flicker duration
//         float interval = 0.1f; // Time between sprite changes
//         float timer = 0f;
//
//         // Get all available power sprites
//         List<Sprite> availableSprites = collectedPowerUps.Select(p => p.powerSprite).ToList();
//
//         while (timer < duration)
//         {
//             Sprite randomSprite = availableSprites[UnityEngine.Random.Range(0, availableSprites.Count)];
//             spriteTarget.sprite = randomSprite;
//
//             timer += interval;
//             yield return new WaitForSeconds(interval);
//         }
//
//         // End with the correct one
//         spriteTarget.sprite = finalSprite;
//     }
//
//     void UsePowerUp(string name, int index)
//     {
//         ItemData foundItem = itemData.FirstOrDefault(item => item.itemName == name);
//         
//         if (foundItem != null)
//         {
//             itemCaster.item = foundItem._item;
//             itemCaster.ammo = foundItem._ammo;
//         
//             if (foundItem.itemName == "Homing Item")
//             {
//                 StartCoroutine(UseAllBulletAmmo());
//             }
//             else
//             {
//                 itemCaster.Cast();
//             }
//         
//             itemData.Remove(foundItem);
//         }
//         
//         RemovePowerUpButton(index);
//         activePowerUpIndexes.Remove(index); // Proper cleanup
//         
//         // Try adding another available item
//         foreach (var item in itemData)
//         {
//             if (!item.IsItemAddedAtBtn)
//             {
//                 UpdatePowerUpUI(item.itemName, item);
//                 break;
//             }
//         }
//
//         
//     }
//     
//     private void ActivateEnvironment(int index)
//     {
//        
//         if (levelGameObjects == null || index < 0 || index >= levelGameObjects.Length) return;
//
//         for (int i = 0; i < levelGameObjects.Length; i++)
//         {
//             if (levelGameObjects[i] != null)
//                 levelGameObjects[i].SetActive(i == index);
//         }
//     }
//
//     public void GameoverHide()
//     {
//        
//         
//         
//         //////////////////////////////-------------------------------------------------------------///////////////////////////////////////////////////
//         
// //         finishPanel.SetActive(false);
// //         finishPanel2.SetActive(true);
// //         
// //         if (levelGameObjects == null || levelGameObjects.Length == 0)
// //         {
// //             Debug.LogWarning("GameController: No levelGameObjects assigned (GameoverHide).");
// //             return;
// //         }
// //         
// //         // Pick random next level
// //         // selectedLevelIndex = UnityEngine.Random.Range(0, levelGameObjects.Length);
// //         // PlayerPrefs.SetInt("NextLevelIndex", selectedLevelIndex);
// //         // PlayerPrefs.Save();
// //         
// //         selectedLevelIndex = UnityEngine.Random.Range(0, levelGameObjects.Length);
// //
// // // Persist for reload
// //         PlayerPrefs.SetInt("NextLevelIndex", selectedLevelIndex);
// //
// // // Also sync SceneToLoad so GetLapsForCurrentEnvironment() uses the right laps key next session
// //         PlayerPrefs.SetInt("SceneToLoad", selectedLevelIndex);
// //         PlayerPrefs.Save();
// //         
// //         // Preview images (if provided)
// //         if (nextLevelPreviewImages != null && nextLevelPreviewImages.Length > 0)
// //         {
// //             foreach (GameObject go in nextLevelPreviewImages)
// //                 if (go != null) go.SetActive(false);
// //         
// //             if (selectedLevelIndex < nextLevelPreviewImages.Length && nextLevelPreviewImages[selectedLevelIndex] != null)
// //             {
// //                 var previewGO = nextLevelPreviewImages[selectedLevelIndex];
// //                 previewGO.SetActive(true);
// //                 DOTween.Restart(previewGO);
// //             }
// //         }
// //         
// //         // Show coin summary UI (unchanged)
// //         if (bonusCoinsText != null) bonusCoinsText.gameObject.SetActive(true);
// //         if (totalCoinsText2 != null) totalCoinsText2.gameObject.SetActive(true);
// //         
// //         int collected = CurrencyManager.instance.GetLevelCoins();
// //         int bonus = 0;
// //         int.TryParse(bonusCoinsText.text, out bonus);
// //         
// //         int oldSaved = CurrencyManager.instance.GetSavedCoins() - collected;
// //         int firstTotal = oldSaved + collected;
// //         int finalTotal = firstTotal + bonus;
// //         
// //         totalCoinsText2.text = firstTotal.ToString();
// //         
// //         StartCoroutine(AnimateBonusMerge(firstTotal, finalTotal, finalTotal));
// //         
// //         ShowKartsAccordingToRanking();
//
//
//         //////////////////////////////////=========================================//////////////////////
//
//         finishPanel.SetActive(false);
//         finishPanel2.SetActive(true);
//
//         // Safety: preview arrays required
//         if (nextLevelEnvironments == null || nextLevelEnvironments.Length == 0)
//         {
//             Debug.LogWarning("GameController: nextLevelEnvironments not assigned, cannot pick next env.");
//             return;
//         }
//
//         // Random pick (index env + preview + skybox arrays same order rakho)
//         int nextEnvIndex = UnityEngine.Random.Range(0, nextLevelEnvironments.Length);
//
//         // Save for LoadNextLevel() scene reload
//         PlayerPrefs.SetInt(NEXT_ENV_KEY, nextEnvIndex);
//         PlayerPrefs.Save();
//         Debug.Log($"[GameController] NextLevelEnvIndex saved = {nextEnvIndex}");
//
//         // --- Preview Image UI ---
//         if (nextLevelPreviewImages != null && nextLevelPreviewImages.Length > 0)
//         {
//             foreach (GameObject go in nextLevelPreviewImages)
//                 if (go != null) go.SetActive(false);
//
//             if (nextEnvIndex < nextLevelPreviewImages.Length && nextLevelPreviewImages[nextEnvIndex] != null)
//             {
//                 var previewGO = nextLevelPreviewImages[nextEnvIndex];
//                 previewGO.SetActive(true);
//                 DOTween.Restart(previewGO);
//             }
//         }
//
//         // --- Coins summary (unchanged) ---
//         if (bonusCoinsText != null) bonusCoinsText.gameObject.SetActive(true);
//         if (totalCoinsText2 != null) totalCoinsText2.gameObject.SetActive(true);
//
//         int collected = CurrencyManager.instance.GetLevelCoins();
//         int bonus = 0;
//         int.TryParse(bonusCoinsText.text, out bonus);
//
//         int oldSaved = CurrencyManager.instance.GetSavedCoins() - collected;
//         int firstTotal = oldSaved + collected;
//         int finalTotal = firstTotal + bonus;
//
//         totalCoinsText2.text = firstTotal.ToString();
//         StartCoroutine(AnimateBonusMerge(firstTotal, finalTotal, finalTotal));
//
//         ShowKartsAccordingToRanking();
//         
//         
//         
//     }
//     
//     private IEnumerator AnimateBonusMerge(int from, int to, int finalValue)
//     {
//         yield return new WaitForSeconds(0.5f); // Slight delay before animating
//
//         // ✅ Just set bonusCoinsText directly, no animation
//         // It should already be set via bonusCoinsText.text = bonus.ToString();
//
//         // Delay before animating totalCoinsText2
//         yield return new WaitForSeconds(0.3f);
//
//         float duration = 1.2f;
//         float elapsed = 0f;
//         int lastValue = from;
//
//         while (elapsed < duration)
//         {
//             elapsed += Time.unscaledDeltaTime;
//             float t = Mathf.Clamp01(elapsed / duration);
//             int current = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
//
//             totalCoinsText2.text = current.ToString();
//
//             if (current != lastValue)
//             {
//                 totalCoinsText2.transform.DOKill();
//                 totalCoinsText2.transform.localScale = Vector3.one * 1.3f;
//                 totalCoinsText2.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
//                 lastValue = current;
//             }
//
//             yield return null;
//         }
//
//         totalCoinsText2.text = to.ToString();
//
//         // ✅ Save final coin count
//         PlayerPrefs.SetInt(CurrencyManager.instance._coinsPref, finalValue);
//         PlayerPrefs.Save();
//
//         // ✅ Optional event trigger
//        // CurrencyManager.coinsChangedEvent?.Invoke(finalValue);
//     }
//
//     void RemovePowerUpButton(int index)
//     {
//         
//         if (index >= 0 && index < powerUpButtons.Length)
//         {
//             powerUpButtons[index].gameObject.SetActive(false);
//             powerUpButtons[index].onClick.RemoveAllListeners();
//             powerUpButtons[index].GetComponent<Image>().sprite = null;
//         }
//         
//         // Optional: Reset IsItemAddedAtBtn on all items at this point
//         foreach (var item in itemData)
//         {
//             if (item.itemName == powerUpButtons[index].name)
//             {
//                 item.IsItemAddedAtBtn = false;
//             }
//         }
//         
//       
//     }
//
//     private IEnumerator UseAllBulletAmmo()
//     {
//         itemCaster.Cast();
//         yield return new WaitForSeconds(0.05f);
//
//         if (itemCaster.ammo != 0)
//         {
//             StartCoroutine(UseAllBulletAmmo());
//         }
//         else
//         {
//             itemCaster.item = null;
//         }
//     }
//     
//     public void LoadNextLevel()
//     { 
//       
//      
//       // int index = PlayerPrefs.GetInt("NextLevelIndex", -1);
//       // if (index < 0)
//       // {
//       //     Debug.LogWarning("LoadNextLevel: No NextLevelIndex set. Defaulting to 0.");
//       //     index = 0;
//       // }
//       //
//       // // Already set by GameoverHide → so here you just reload:
//       // CanvasOffMap.SetActive(true);
//       // Time.timeScale = 1f;
//       //
//       // if (loadingScreen != null)
//       //     loadingScreen.SetActive(true);
//       //
//       // StartCoroutine(sceneload());
//       // // Scene currentScene = SceneManager.GetActiveScene();
//       // // SceneManager.LoadScene(currentScene.buildIndex);
//       
//       ///////-----------------------------------///////////
//       
//       // int index = PlayerPrefs.HasKey("NextLevelIndex") ? PlayerPrefs.GetInt("NextLevelIndex") : 0;
//       //
//       // Debug.Log($"[GameController] Loading next level with env index {index}");
//
//       CanvasOffMap.SetActive(true);
//       Time.timeScale = 1f;
//
//       if (loadingScreen != null)
//           loadingScreen.SetActive(true);
//
//       StartCoroutine(sceneload());
//
//     }
//     
//     
//     private void ActivateNextLevelEnvFromPrefs()
//     {
//         if (!PlayerPrefs.HasKey(NEXT_ENV_KEY))
//         {
//             // No next-env set → Main Menu flow → GameplaySceneInitializer already did its job.
//             return;
//         }
//
//         int envIndex = PlayerPrefs.GetInt(NEXT_ENV_KEY, 0);
//
//         // Yeh zaroori hai: laps system GetLapsForCurrentEnvironment() SceneToLoad use karta hai.
//         PlayerPrefs.SetInt("SceneToLoad", envIndex);
//
//         PlayerPrefs.DeleteKey(NEXT_ENV_KEY); // one-shot consume
//         PlayerPrefs.Save();
//
//         // Activate env roots
//         if (nextLevelEnvironments != null && nextLevelEnvironments.Length > 0)
//         {
//             for (int i = 0; i < nextLevelEnvironments.Length; i++)
//             {
//                 if (nextLevelEnvironments[i] != null)
//                     nextLevelEnvironments[i].SetActive(i == envIndex);
//             }
//         }
//
//         // Apply skybox
//         if (nextLevelSkyboxes != null && envIndex >= 0 && envIndex < nextLevelSkyboxes.Length && nextLevelSkyboxes[envIndex] != null)
//         {
//             RenderSettings.skybox = nextLevelSkyboxes[envIndex];
//             DynamicGI.UpdateEnvironment();
//         }
//
//         Debug.Log($"[GameController] LoadNextLevel env activated index={envIndex}");
//     }
//
//     public IEnumerator sceneload()
//     {
//
//        //  yield return new WaitForSeconds(3f);
//        // // loadingScreen.SetActive(false);
//        //  Scene currentScene = SceneManager.GetActiveScene();
//        //  SceneManager.LoadScene(currentScene.buildIndex);
//        
//        yield return new WaitForSeconds(3f);
//        // Scene currentScene = SceneManager.GetActiveScene();
//        // SceneManager.LoadScene(currentScene.buildIndex);
//        SceneManager.LoadScene("Gameplay Karts");
//     }
//
//     private void ResetLevelToDefault(GameObject root)
//     {
//         
//         
//         foreach (Transform child in root.transform)
//         {
//             child.gameObject.SetActive(false);
//         }
//
//         // Then reactivate them (or do custom logic here)
//         foreach (Transform child in root.transform)
//         {
//             child.gameObject.SetActive(true);
//         }
//     }
//
//     public IEnumerator loadingshow()
//     {
//        
//         
//         yield return new WaitForSeconds(3f);
//         loadingScreen.SetActive(false);
//         finishPanel2.SetActive(false);
//         
//       
//     }
//
//     #endregion
// }
//
//
//
// [Serializable]
//     public class LevelData
//     {
//         public GameObject[] opponentKarts;
//         public Transform[] opponentKartsSpawnPoints;
//         public int totalOpponentKarts;
//         public int totalLaps;
//     }
//
//     [Serializable]
//     public class Karts
//     {
//         public GameObject[] kartObject;
//         public Transform spawnTransform;
//     }
//
//     [Serializable]
//     public class PowerUp
//     {
//         public string powerName;
//         public Sprite powerSprite;
//         public Sprite powerBG;
//     }
//
//     [Serializable]
//     public class ItemData
//     {
//         public string itemName;
//         public Item _item;
//         public int _ammo;
//
//         public bool IsItemAddedAtBtn;
//     }
    
///////////////////////////////////////////----------------------------------------//////////////////////////////////////////////////////////////////////

  #region DataFields

     public static GameController instance;

     public GameObject env;

     [Header("UI")]
     public GameObject wrongDirWarning;
     public TextMeshProUGUI currentLapText;
     [SerializeField] private TextMeshProUGUI totalLapText;
     public TextMeshProUGUI currentPlayerRankText;
     public List<PowerUp> collectedPowerUps = new List<PowerUp>();
     public Button[] powerUpButtons;

     [Space(5f)]
     [Header("References")]
     [SerializeField] private Karts playerKart;
     public LevelData levelData;
     [SerializeField] private KartCamera kartCamera;
     [SerializeField] private KartCamera kartCameraRear;
     [SerializeField] private UIControl uIControl;
     [SerializeField] private BasicWaypoint basicWaypoint; //Waypoint Reference For AI Karts
     [SerializeField] private MinimapManager minimapManager;

     [Space(5f)]
     [Header("Debug")]
     [SerializeField] private int currentPlayerKart;

     public GameObject player;
     public GameObject[] opponentKarts;
     private Kart kart;
     public WaypointManager waypointManager;

     public List<ItemData> itemData;
     public ItemCaster itemCaster;
     public int powerBtnCount = 0;

     // old sceneIndices (unused in new flow but kept for compat)
     public int[] sceneIndices;

     [Tooltip("Preview images shown on GameOver panel. Order must match nextLevelEnvironments & nextLevelSkyboxes.")]
     public GameObject[] nextLevelPreviewImages;

     private int selectedSceneIndex = -1;

     #endregion

     [Header("Pause / Finish Panels")]
     public GameObject Pause;
     public GameObject finishPanel;
     public GameObject finishPanel2;

     public TextMeshProUGUI[] rankTexts;
     public TextMeshProUGUI[] rankTextsPanel2;

     private List<int> activePowerUpIndexes = new List<int>();
     private int maxPowerUpsOnScreen = 3;

     [Header("Podium Setup")]
     public Transform[] podiumSlots = new Transform[5]; // Where ranked karts appear
     public GameObject[] playerPreviewPrefabs; // Player kart prefabs for podium
     public GameObject[] enemyPreviewPrefabs;  // Enemy kart prefabs for podium
     private GameObject[] podiumPreviewInstances = new GameObject[5];

     [Header("Sprite Podium Setup")]
     public Sprite[] playerPreviewSprites;
     public Sprite[] enemyPreviewSprites;
     public Transform[] spriteDisplayPositions; // UI Image GameObjects with Image component
     public Transform[] spriteDisplayPositionsPanel2;
     private GameObject[] podiumSpriteInstances = new GameObject[5];

     public GameObject sound;

     [Header("Loading Screen")]
     public GameObject loadingScreen;

     [Header("Player Image Display")]
     public Sprite[] playerImages;          // Match order with kartObject
     public Image playerImageDisplayPoint;

     public Text Cointext;
     public Text savedCoinsText;
     public Text collectedCoinsText;
     public Text totalCoinsText;
     public Text totalCoinsText2;
     public Text bonusCoinsText;

     [Header("Legacy Local Sub-Level Containers (OPTIONAL)")]
     [Tooltip("Old per-controller sub-level roots. Not used in LoadNext flow. Safe to leave empty.")]
    // public GameObject[] levelGameObjects;
     private int selectedLevelIndex = -1; // legacy

     private bool hasBeenInitialized = false;

     // === LoadNextLevel Environment & Skybox Control ===
     [Header("Next-Level Flow (LoadNextLevel)")]
   //  [Tooltip("Root env objects to activate on next level reload. Order MUST match GameplaySceneInitializer.environmentRoots.")]
     //public GameObject[] nextLevelEnvironments;
  //   [Tooltip("Skyboxes matching nextLevelEnvironments order.")]
    // public Material[] nextLevelSkyboxes;

     // PlayerPrefs keys
     private const string NEXT_ENV_KEY = "NextLevelEnvIndex"; // *** PATCH KEY ***

     // Legacy sub-level support
     private const string NEXT_SUB_KEY = "NextSubIndex"; // not used in new flow
     private int selectedSubIndex = -1;

     public GameObject CanvasOffMap;
     
     [SerializeField] private GameplaySceneInitializer gameplaySceneInitializer;

     #region Unity Lifecycle

     private void Awake()
     {
         instance = this;
         Time.timeScale = 1f;
        
         
         if (gameplaySceneInitializer == null && GameplaySceneInitializer.Instance != null)
         {
             gameplaySceneInitializer = GameplaySceneInitializer.Instance;
         }
     }

     private void Start()
     {
         // *** PATCH *** Override environment only if NEXT_ENV_KEY exists (LoadNext flow)
         //ActivateNextLevelEnvFromPrefs();
         
         StartCoroutine(DeferredActivateNextLevelEnvFromPrefs());

         Time.timeScale = 1f;
         Initialize();

         LapCounter.finalRankList.Clear();
         CurrencyManager.instance.ResetLevelCoins();

         TssAdsManager._Instance.admobInstance.ShowLeftBanner();
         TssAdsManager._Instance.admobInstance.ShowRightBanner();
     }

     #endregion

     #region Init & Spawn

     public void Initialize()
     {
         Debug.Log("GameController: Initialize() called.");

         if (hasBeenInitialized)
         {
             Debug.Log("GameController: Clearing old karts before reinitializing.");

             if (player != null)
             {
                 Destroy(player);
                 if (MyGameManager.instance != null)
                     MyGameManager.instance.player_Spawned = null;
             }

             if (opponentKarts != null)
             {
                 foreach (var k in opponentKarts)
                     if (k != null) Destroy(k);
                 if (MyGameManager.instance != null)
                     MyGameManager.instance.Enemies_Spawned.Clear();
             }

             opponentKarts = null;
         }

         hasBeenInitialized = true;

         SpawnPlayerKart();
         SpawnOpponentKarts();
         kartCamera.Initialize(kart);
         kartCameraRear.Initialize(kart);
         uIControl.Initialize(kart);
         minimapManager.InitMinimapData(player.transform, opponentKarts);
         minimapManager.SetupIcons();

         levelData.totalLaps = GetLapsForCurrentEnvironment();
         totalLapText.text = levelData.totalLaps.ToString();

         Debug.Log("Environment Index: " + PlayerPrefs.GetInt("SceneToLoad", 0));
         Debug.Log("Loaded Laps: " + levelData.totalLaps);
     }
     
     private IEnumerator DeferredActivateNextLevelEnvFromPrefs()
     {
         // wait one frame so GameplaySceneInitializer.Start() has run
         yield return null;
         ActivateNextLevelEnvFromPrefs();
     }

     private void SpawnPlayerKart()
     {
         int selectedIndex = PlayerPrefs.GetInt("Player");
         player = Instantiate(
             playerKart.kartObject[selectedIndex],
             playerKart.spawnTransform.position,
             playerKart.spawnTransform.rotation);

         kart = player.GetComponent<Kart>();

         // Set Player Bool for Lap Counter
         player.GetComponent<LapCounter>().isPlayer = true;
         itemCaster = player.GetComponent<ItemCaster>();
         MyGameManager.instance.player_Spawned = player;
         Debug.Log("SpawnPlayerKart called :added player to MyGameManager");
         MyGameManager.instance.onPlayerAdded_StopPlayer();

         // assign player image
         if (playerImages != null && selectedIndex >= 0 && selectedIndex < playerImages.Length && playerImageDisplayPoint != null)
         {
             playerImageDisplayPoint.sprite = playerImages[selectedIndex];
             playerImageDisplayPoint.gameObject.SetActive(true);
         }
     }

     private void SpawnOpponentKarts()
     {
         opponentKarts = new GameObject[levelData.totalOpponentKarts];

         List<GameObject> tempKarts = levelData.opponentKarts.ToList();
         List<Transform> tempSpawnpoints = levelData.opponentKartsSpawnPoints.ToList();

         for (int i = 0; i < levelData.totalOpponentKarts; i++)
         {
             int randKart = UnityEngine.Random.Range(0, tempKarts.Count);
             int randSpawnPoint = UnityEngine.Random.Range(0, tempSpawnpoints.Count);

             opponentKarts[i] = Instantiate(tempKarts[randKart], tempSpawnpoints[randSpawnPoint].position,
                 tempSpawnpoints[randSpawnPoint].rotation);
             MyGameManager.instance.Enemies_Spawned.Add(opponentKarts[i]);
             Debug.Log("SpawnOpponentKarts called :added enemies to MyGameManager");

             // Assigning Waypoint Reference to AI Karts
             opponentKarts[i].GetComponent<BasicWaypointFollowerDrift>().targetPoint = basicWaypoint;
             opponentKarts[i].SetActive(true);

             tempKarts.RemoveAt(randKart);
             tempSpawnpoints.RemoveAt(randSpawnPoint);
         }

         tempKarts.Clear();
         tempSpawnpoints.Clear();
         MyGameManager.instance.onEnemiesAdded_StopEnemies();
         MyGameManager.instance.call_playStart_CutScene();
     }

     #endregion

     #region Laps / Audio / Pause

     private int GetLapsForCurrentEnvironment()
     {
         // `SceneToLoad` drives which environment is active.
         int envIndex = PlayerPrefs.GetInt("SceneToLoad", 0);

         // Use the central utility; DO NOT auto-write default.
         int laps = LapPrefsUtility.Get(envIndex, 2);

         if (laps < 1)
         {
             Debug.LogWarning($"[GameController] Lap pref for env {envIndex} invalid ({laps}), using 2.");
             laps = 2;
         }

         return laps;
     }

     public void BGMOff(bool val)
     {
         sound.SetActive(val);
     }

     public void pauseGame()
     {
         Pause.SetActive(true);
         Time.timeScale = 0;
     }

     public void UnpauseGame()
     {
         Pause.SetActive(false);
         Time.timeScale = 1;
         TssAdsManager._Instance.ShowInterstitial("Un-Puased");
     }

     #endregion

     #region Finish / Ranking / Podium / Coins

     public void ShowFinishPanel()
     {
         finishPanel.SetActive(true);
         CanvasOffMap.SetActive(false);

         CurrencyManager.instance.MergeCoins();

         int finalPosition = LapCounter.finalRankList.FindIndex(name =>
         {
             string playerName = PlayerPrefs.GetString("PlayerName", "Player").Trim();
             return name == playerName || (name.Contains("Player") && name.Contains("(Clone)"));
         }) + 1; // 1-based rank

         int lapCount = levelData.totalLaps;
         int bonusCoins = 0;

         switch (finalPosition)
         {
             case 1: bonusCoins = lapCount * 1000; break;
             case 2: bonusCoins = lapCount * 500; break;
             case 3: bonusCoins = lapCount * 250; break;
         }

         CurrencyManager.instance.AddBonusCoins(bonusCoins);

         int saved = CurrencyManager.instance.GetSavedCoins(); // after merge
         int collected = CurrencyManager.instance.GetLevelCoins();
         int total = saved;

         int oldSaved = saved - collected;

         savedCoinsText.text = oldSaved.ToString(); // before this level
         collectedCoinsText.text = collected.ToString();
         totalCoinsText.text = oldSaved.ToString(); // start from oldSaved

         // set bonus
         bonusCoinsText.text = bonusCoins.ToString();

         StartCoroutine(AnimateTotalCoins(oldSaved, total));

         ShowRankings();
         ShowKartsAccordingToRanking();
         Time.timeScale = 1;

         TssAdsManager._Instance.ShowInterstitial("Game over");
     }

     private IEnumerator AnimateTotalCoins(int from, int to)
     {
         yield return new WaitForSeconds(1f);

         float duration = 1.2f;
         float elapsed = 0f;
         int lastValue = from;

         while (elapsed < duration)
         {
             elapsed += Time.unscaledDeltaTime;
             float t = Mathf.Clamp01(elapsed / duration);
             int current = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
             totalCoinsText.text = current.ToString();

             if (current != lastValue)
             {
                 totalCoinsText.transform.DOKill();
                 totalCoinsText.transform.localScale = Vector3.one * 1.3f;
                 totalCoinsText.transform.DOScale(1f, 0.2f)
                     .SetEase(Ease.OutBack)
                     .SetUpdate(true);
                 lastValue = current;
             }

             yield return null;
         }

         totalCoinsText.text = to.ToString();
     }

     private void ShowRankings()
     {
         string playerName = PlayerPrefs.GetString("PlayerName", "Player").Trim();

         for (int i = 0; i < rankTexts.Length; i++)
         {
             string rawName = i < LapCounter.finalRankList.Count ? LapCounter.finalRankList[i] : "DNF";
             if (string.IsNullOrEmpty(rawName)) rawName = "DNF";

             bool isPlayer = rawName == playerName;

             // If slot contains GO clone name, override with player name
             if (rawName != playerName && rawName.Contains("Player") && rawName.Contains("(Clone)"))
             {
                 rawName = playerName;
                 isPlayer = true;
             }

             rankTexts[i].text = rawName;
             rankTexts[i].color = isPlayer ? Color.green : Color.white;
             rankTexts[i].gameObject.SetActive(true);

             if (rankTextsPanel2 != null && i < rankTextsPanel2.Length)
             {
                 rankTextsPanel2[i].text = rawName;
                 rankTextsPanel2[i].color = isPlayer ? Color.green : Color.white;
                 rankTextsPanel2[i].gameObject.SetActive(true);
             }
         }
     }

     public void ShowKartsAccordingToRanking()
     {
         for (int i = 0; i < podiumSlots.Length; i++)
         {
             if (i >= LapCounter.finalRankList.Count) break;

             string rankedName = LapCounter.finalRankList[i];
             GameObject prefabToSpawn = null;
             Sprite spriteToDisplay = null;

             string playerName = PlayerPrefs.GetString("PlayerName", "Player");
             bool isPlayer = rankedName == playerName;

             if (isPlayer)
             {
                 int playerIndex = PlayerPrefs.GetInt("Player", 0);

                 if (playerIndex >= 0 && playerIndex < playerPreviewPrefabs.Length)
                     prefabToSpawn = playerPreviewPrefabs[playerIndex];

                 if (playerIndex >= 0 && playerIndex < playerPreviewSprites.Length)
                     spriteToDisplay = playerPreviewSprites[playerIndex];
             }
             else
             {
                 for (int j = 0; j < opponentKarts.Length; j++)
                 {
                     if (opponentKarts[j].name.Contains(rankedName))
                     {
                         if (j < enemyPreviewPrefabs.Length)
                             prefabToSpawn = enemyPreviewPrefabs[j];

                         if (j < enemyPreviewSprites.Length)
                             spriteToDisplay = enemyPreviewSprites[j];
                         break;
                     }
                 }
             }

             if (prefabToSpawn != null)
             {
                 GameObject instance = Instantiate(prefabToSpawn, podiumSlots[i].position, podiumSlots[i].rotation);
                 instance.SetActive(true);
                 instance.transform.SetParent(podiumSlots[i], false);
                 instance.transform.localPosition = Vector3.zero;
                 instance.transform.localRotation = Quaternion.identity;
                 instance.AddComponent<Slowspin>();
                 podiumPreviewInstances[i] = instance;
             }

             if (spriteToDisplay != null && i < spriteDisplayPositions.Length)
             {
                 Image img = spriteDisplayPositions[i].GetComponent<Image>();
                 if (img != null)
                 {
                     img.sprite = spriteToDisplay;
                     img.gameObject.SetActive(true);
                 }
             }

             if (spriteToDisplay != null && i < spriteDisplayPositionsPanel2.Length)
             {
                 Image img2 = spriteDisplayPositionsPanel2[i].GetComponent<Image>();
                 if (img2 != null)
                 {
                     img2.sprite = spriteToDisplay;
                     img2.gameObject.SetActive(true);
                 }
             }
         }
     }

     #endregion

     #region Camera

     public void SwitchCamera(bool IsSwitchCamera)
     {
         if (IsSwitchCamera)
         {
             kartCameraRear.gameObject.SetActive(true);
             kartCamera.gameObject.SetActive(false);
         }
         else
         {
             kartCamera.gameObject.SetActive(true);
             kartCameraRear.gameObject.SetActive(false);
         }
     }

     #endregion

     #region PowerUps

     public void UpdatePowerUpUI(string name, ItemData _itemData)
     {
         if (_itemData.IsItemAddedAtBtn) return;

         int targetIndex = -1;

         //Reuse oldest if full
         if (activePowerUpIndexes.Count >= maxPowerUpsOnScreen)
         {
             targetIndex = activePowerUpIndexes[0];
             RemovePowerUpButton(targetIndex);
             activePowerUpIndexes.RemoveAt(0);
         }
         else
         {
             // Find first inactive button
             for (int i = 0; i < powerUpButtons.Length; i++)
             {
                 if (!powerUpButtons[i].gameObject.activeSelf)
                 {
                     targetIndex = i;
                     break;
                 }
             }
         }

         if (targetIndex != -1)
         {
             PowerUp powerUp = collectedPowerUps.FirstOrDefault(p => p.powerName == name);
             if (powerUp != null)
             {
                 Button btn = powerUpButtons[targetIndex];
                 btn.gameObject.SetActive(true);
                 btn.GetComponent<Image>().sprite = powerUp.powerBG;

                 // selection animation
                 StartCoroutine(AnimatePowerUpSelection(
                     btn.transform.GetChild(1).GetComponent<Image>(),
                     powerUp.powerSprite));

                 btn.onClick.RemoveAllListeners();
                 int capturedIndex = targetIndex;
                 btn.onClick.AddListener(() => UsePowerUp(name, capturedIndex));

                 activePowerUpIndexes.Add(capturedIndex);
                 _itemData.IsItemAddedAtBtn = true;
             }
         }
     }

     private IEnumerator AnimatePowerUpSelection(Image spriteTarget, Sprite finalSprite)
     {
         float duration = 0.6f;
         float interval = 0.1f;
         float timer = 0f;

         List<Sprite> availableSprites = collectedPowerUps.Select(p => p.powerSprite).ToList();

         while (timer < duration)
         {
             Sprite randomSprite = availableSprites[UnityEngine.Random.Range(0, availableSprites.Count)];
             spriteTarget.sprite = randomSprite;

             timer += interval;
             yield return new WaitForSeconds(interval);
         }

         spriteTarget.sprite = finalSprite;
     }

     private void UsePowerUp(string name, int index)
     {
         ItemData foundItem = itemData.FirstOrDefault(item => item.itemName == name);

         if (foundItem != null)
         {
             itemCaster.item = foundItem._item;
             itemCaster.ammo = foundItem._ammo;

             if (foundItem.itemName == "Homing Item")
             {
                 StartCoroutine(UseAllBulletAmmo());
             }
             else
             {
                 itemCaster.Cast();
             }

             itemData.Remove(foundItem);
         }

         RemovePowerUpButton(index);
         activePowerUpIndexes.Remove(index);

         // Try adding another available item
         foreach (var item in itemData)
         {
             if (!item.IsItemAddedAtBtn)
             {
                 UpdatePowerUpUI(item.itemName, item);
                 break;
             }
         }
     }

     private void RemovePowerUpButton(int index)
     {
         if (index >= 0 && index < powerUpButtons.Length)
         {
             powerUpButtons[index].gameObject.SetActive(false);
             powerUpButtons[index].onClick.RemoveAllListeners();
             powerUpButtons[index].GetComponent<Image>().sprite = null;
         }

         // Optional reset (weak match by name)
         foreach (var item in itemData)
         {
             if (index >= 0 && index < powerUpButtons.Length &&
                 item.itemName == powerUpButtons[index].name)
             {
                 item.IsItemAddedAtBtn = false;
             }
         }
     }

     private IEnumerator UseAllBulletAmmo()
     {
         itemCaster.Cast();
         yield return new WaitForSeconds(0.05f);

         if (itemCaster.ammo != 0)
         {
             StartCoroutine(UseAllBulletAmmo());
         }
         else
         {
             itemCaster.item = null;
         }
     }

     #endregion

     #region GameOver -> Next Level Flow  (PATCHED)

     /// <summary>
     /// Called from GameOver "Next" button. Picks random next environment, saves index, shows preview + coins.
     /// </summary>
     public void GameoverHide()
     {
         // finishPanel.SetActive(false);
         // finishPanel2.SetActive(true);
         //
         // // Safety: env array required
         // if (nextLevelEnvironments == null || nextLevelEnvironments.Length == 0)
         // {
         //     Debug.LogWarning("GameController: nextLevelEnvironments not assigned, cannot pick next env.");
         //     return;
         // }
         //
         // // Random pick (index env + preview + skybox arrays same order rakho)
         // int nextEnvIndex = UnityEngine.Random.Range(0, nextLevelEnvironments.Length);
         //
         // // Save for LoadNextLevel() scene reload
         // PlayerPrefs.SetInt(NEXT_ENV_KEY, nextEnvIndex);
         // PlayerPrefs.Save();
         // Debug.Log($"[GameController] NextLevelEnvIndex saved = {nextEnvIndex}");
         //
         // // --- Preview Image UI ---
         // if (nextLevelPreviewImages != null && nextLevelPreviewImages.Length > 0)
         // {
         //     foreach (GameObject go in nextLevelPreviewImages)
         //         if (go != null) go.SetActive(false);
         //
         //     if (nextEnvIndex < nextLevelPreviewImages.Length && nextLevelPreviewImages[nextEnvIndex] != null)
         //     {
         //         var previewGO = nextLevelPreviewImages[nextEnvIndex];
         //         previewGO.SetActive(true);
         //         DOTween.Restart(previewGO);
         //     }
         // }
         //
         // // --- Coins summary (unchanged) ---
         // if (bonusCoinsText != null) bonusCoinsText.gameObject.SetActive(true);
         // if (totalCoinsText2 != null) totalCoinsText2.gameObject.SetActive(true);
         //
         // int collected = CurrencyManager.instance.GetLevelCoins();
         // int bonus = 0;
         // int.TryParse(bonusCoinsText.text, out bonus);
         //
         // int oldSaved = CurrencyManager.instance.GetSavedCoins() - collected;
         // int firstTotal = oldSaved + collected;
         // int finalTotal = firstTotal + bonus;
         //
         // totalCoinsText2.text = firstTotal.ToString();
         // StartCoroutine(AnimateBonusMerge(firstTotal, finalTotal, finalTotal));
         //
         // ShowKartsAccordingToRanking();
         
         finishPanel.SetActive(false);
    finishPanel2.SetActive(true);

    // Ensure we have an initializer reference
    if (gameplaySceneInitializer == null && GameplaySceneInitializer.Instance != null)
        gameplaySceneInitializer = GameplaySceneInitializer.Instance;

    if (gameplaySceneInitializer == null || gameplaySceneInitializer.environmentRoots == null || gameplaySceneInitializer.environmentRoots.Length == 0)
    {
        Debug.LogWarning("GameController: GameplaySceneInitializer/environmentRoots missing; cannot pick next env.");
        return;
    }

    int envCount   = gameplaySceneInitializer.environmentRoots.Length;
    int currentEnv = gameplaySceneInitializer.ActiveEnvironmentIndex;

    // pick a random that is NOT the current environment (unless only 1 exists)
    int nextEnvIndex;
    if (envCount <= 1)
    {
        nextEnvIndex = 0;
    }
    else
    {
        do
        {
            nextEnvIndex = UnityEngine.Random.Range(0, envCount);
        } while (nextEnvIndex == currentEnv);
    }

    // Save for LoadNextLevel() scene reload (consumed in ActivateNextLevelEnvFromPrefs)
    PlayerPrefs.SetInt(NEXT_ENV_KEY, nextEnvIndex);
    PlayerPrefs.Save();
    Debug.Log($"[GameController] NextLevelEnvIndex saved = {nextEnvIndex} (current={currentEnv})");

    // --- Preview Image UI ---
    // Expect nextLevelPreviewImages length == envCount (drag in Inspector to match GameplaySceneInitializer.environmentRoots order)
    if (nextLevelPreviewImages != null && nextLevelPreviewImages.Length > 0)
    {
        foreach (GameObject go in nextLevelPreviewImages)
            if (go != null) go.SetActive(false);

        if (nextEnvIndex < nextLevelPreviewImages.Length && nextLevelPreviewImages[nextEnvIndex] != null)
        {
            var previewGO = nextLevelPreviewImages[nextEnvIndex];
            previewGO.SetActive(true);
            DOTween.Restart(previewGO);
        }
        else
        {
            Debug.LogWarning($"[GameController] Missing preview image for env {nextEnvIndex}.");
        }
    }

    // --- Coins summary (unchanged) ---
    if (bonusCoinsText != null) bonusCoinsText.gameObject.SetActive(true);
    if (totalCoinsText2 != null) totalCoinsText2.gameObject.SetActive(true);

    int collected = CurrencyManager.instance.GetLevelCoins();
    int bonus = 0;
    int.TryParse(bonusCoinsText.text, out bonus);

    int oldSaved   = CurrencyManager.instance.GetSavedCoins() - collected;
    int firstTotal = oldSaved + collected;
    int finalTotal = firstTotal + bonus;

    totalCoinsText2.text = firstTotal.ToString();
    StartCoroutine(AnimateBonusMerge(firstTotal, finalTotal, finalTotal));

    ShowKartsAccordingToRanking();
     }

     private IEnumerator AnimateBonusMerge(int from, int to, int finalValue)
     {
         yield return new WaitForSeconds(0.5f);
         yield return new WaitForSeconds(0.3f);

         float duration = 1.2f;
         float elapsed = 0f;
         int lastValue = from;

         while (elapsed < duration)
         {
             elapsed += Time.unscaledDeltaTime;
             float t = Mathf.Clamp01(elapsed / duration);
             int current = Mathf.RoundToInt(Mathf.Lerp(from, to, t));

             totalCoinsText2.text = current.ToString();

             if (current != lastValue)
             {
                 totalCoinsText2.transform.DOKill();
                 totalCoinsText2.transform.localScale = Vector3.one * 1.3f;
                 totalCoinsText2.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
                 lastValue = current;
             }

             yield return null;
         }

         totalCoinsText2.text = to.ToString();

         // Save final coin count
         PlayerPrefs.SetInt(CurrencyManager.instance._coinsPref, finalValue);
         PlayerPrefs.Save();
     }

   

     /// <summary>
     /// Scene reload triggered by Next button.
     /// </summary>
     public void LoadNextLevel()
     {
         CanvasOffMap.SetActive(true);
         Time.timeScale = 1f;
         
         CanvasScriptSplash.instance.LoadScene(2);

         // if (loadingScreen != null)
         //     loadingScreen.SetActive(true);
         
                // GameplaysceneInitializer.SetActive(false);

       //  StartCoroutine(sceneload());
     }

     /// <summary>
     /// Consumes NEXT_ENV_KEY (if set) and overrides environment & skybox BEFORE gameplay init.
     /// </summary>
     private void ActivateNextLevelEnvFromPrefs()
     {
         // if (!PlayerPrefs.HasKey(NEXT_ENV_KEY))
         // {
         //     Debug.Log("[GameController] No NEXT_ENV_KEY; assuming Main Menu flow.");
         //     return;
         // }
         //
         // int envIndex = PlayerPrefs.GetInt(NEXT_ENV_KEY, 0);
         // Debug.Log($"[GameController] Consuming NEXT_ENV_KEY = {envIndex}");
         //
         // // Sync for laps system
         // PlayerPrefs.SetInt("SceneToLoad", envIndex);
         // PlayerPrefs.DeleteKey(NEXT_ENV_KEY);
         // PlayerPrefs.Save();
         //
         // // Activate env roots
         // if (nextLevelEnvironments == null || nextLevelEnvironments.Length == 0)
         // {
         //     Debug.LogError("[GameController] nextLevelEnvironments missing! Cannot activate next env.");
         //     return;
         // }
         //
         // for (int i = 0; i < nextLevelEnvironments.Length; i++)
         // {
         //     if (nextLevelEnvironments[i] != null)
         //         nextLevelEnvironments[i].SetActive(i == envIndex);
         // }
         //
         // // Apply skybox
         // if (nextLevelSkyboxes != null && envIndex >= 0 && envIndex < nextLevelSkyboxes.Length && nextLevelSkyboxes[envIndex] != null)
         // {
         //     RenderSettings.skybox = nextLevelSkyboxes[envIndex];
         //     DynamicGI.UpdateEnvironment();
         // }
         // else
         // {
         //     Debug.LogWarning($"[GameController] Missing skybox for env {envIndex}.");
         // }
         //
         // Debug.Log($"[GameController] LoadNextLevel env activated index={envIndex}");
         
         if (!PlayerPrefs.HasKey(NEXT_ENV_KEY))
         {
             Debug.Log("[GameController] No NEXT_ENV_KEY; assuming Main Menu flow (initializer already applied).");
             return;
         }

         int envIndex = PlayerPrefs.GetInt(NEXT_ENV_KEY, 0);
         Debug.Log($"[GameController] Consuming NEXT_ENV_KEY = {envIndex}");

         // sync for laps (GetLapsForCurrentEnvironment relies on SceneToLoad)
         PlayerPrefs.SetInt("SceneToLoad", envIndex);
         PlayerPrefs.DeleteKey(NEXT_ENV_KEY);
         PlayerPrefs.Save();

         // ensure ref
         if (gameplaySceneInitializer == null && GameplaySceneInitializer.Instance != null)
             gameplaySceneInitializer = GameplaySceneInitializer.Instance;

         if (gameplaySceneInitializer == null)
         {
             Debug.LogError("[GameController] GameplaySceneInitializer missing; cannot apply env override.");
             return;
         }

         // Let the initializer do all environment + skybox work.
         gameplaySceneInitializer.ApplyEnvironment(envIndex, saveToPrefs:false);

         Debug.Log($"[GameController] LoadNextLevel env applied via GameplaySceneInitializer index={envIndex}");
     }

     public IEnumerator sceneload()
     {
         yield return new WaitForSeconds(3f);
       //  FlowOrigin.MarkComingFromGameplay(); 
       CanvasScriptSplash.instance.LoadScene(2);
       //  SceneManager.LoadScene("Gameplay Karts"); // ensure this matches the actual gameplay scene name
     }

     #endregion

     #region Legacy Helpers

     // private void ActivateEnvironment(int index) // legacy
     // {
     //     if (levelGameObjects == null || index < 0 || index >= levelGameObjects.Length) return;
     //
     //     for (int i = 0; i < levelGameObjects.Length; i++)
     //     {
     //         if (levelGameObjects[i] != null)
     //             levelGameObjects[i].SetActive(i == index);
     //     }
     // }

     public IEnumerator loadingshow()
     {
         yield return new WaitForSeconds(3f);
         loadingScreen.SetActive(false);
         finishPanel2.SetActive(false);
     }

     #endregion
 }

 //#region Serializable Data Classes

 [Serializable]
 public class LevelData
 {
     public GameObject[] opponentKarts;
     public Transform[] opponentKartsSpawnPoints;
     public int totalOpponentKarts;
     public int totalLaps;
 }

 [Serializable]
 public class Karts
 {
     public GameObject[] kartObject;
     public Transform spawnTransform;
 }

 [Serializable]
 public class PowerUp
 {
     public string powerName;
     public Sprite powerSprite;
     public Sprite powerBG;
 }

 [Serializable]
 public class ItemData
 {
     public string itemName;
     public Item _item;
     public int _ammo;
     public bool IsItemAddedAtBtn;
 }
 
 public static class LapPrefsUtility
 {
     // IMPORTANT: Must match Main Menu order exactly.
     public static readonly string[] LapKeys = {
         "Laps_Snow",
         "Laps_Mountain",
         "Laps_Desert",
         "Laps_Space",
         "Laps_Circuit"
     };

     /// <summary>
     /// Return stored laps for envIndex if present; otherwise return fallback.
     /// Does NOT write fallback into PlayerPrefs (preserves "unknown" state).
     /// </summary>
     public static int Get(int envIndex, int fallback = 2)
     {
         if (envIndex < 0 || envIndex >= LapKeys.Length)
             return fallback;

         string key = LapKeys[envIndex];
         return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key, fallback) : fallback;
     }

     /// <summary>
     /// Save (overwrite) lap count for envIndex. Use when player confirms input.
     /// </summary>
     public static void Save(int envIndex, int laps, int min = 1, int max = 99)
     {
         if (envIndex < 0 || envIndex >= LapKeys.Length)
             return;

         laps = Mathf.Clamp(laps, min, max);
         PlayerPrefs.SetInt(LapKeys[envIndex], laps);
         PlayerPrefs.Save();
         Debug.Log($"[LapPrefsUtility] Saved {laps} laps to {LapKeys[envIndex]}");
     }

     /// <summary>
     /// Ensure a pref exists (writes fallback only if missing). Optional; default false.
     /// Use only if you WANT to seed missing prefs.
     /// </summary>
     public static int Ensure(int envIndex, int fallback = 2)
     {
         if (envIndex < 0 || envIndex >= LapKeys.Length)
             return fallback;

         string key = LapKeys[envIndex];
         if (!PlayerPrefs.HasKey(key))
         {
             PlayerPrefs.SetInt(key, fallback);
             PlayerPrefs.Save();
             Debug.Log($"[LapPrefsUtility] Auto-seeded {fallback} laps to {key}");
             return fallback;
         }
         return PlayerPrefs.GetInt(key, fallback);
     }
 }

  
