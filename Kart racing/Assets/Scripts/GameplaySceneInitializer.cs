using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Persistent manager for the Gameplay scene.
/// Activates the environment selected in Main Menu (PlayerPrefs key "SceneToLoad")
/// and applies its corresponding skybox.
/// 
/// Place this on a GameObject in the Gameplay scene. It will auto‑initialize in Start()
/// unless you disable <see cref="autoInitOnStart"/> and call <see cref="InitFromPrefs"/> manually.
/// </summary>

public class GameplaySceneInitializer : MonoBehaviour
{
   
    
    public static GameplaySceneInitializer Instance { get; private set; }

    [Header("PlayerPrefs Key (Main Menu sets this)")]
    [Tooltip("Key used to store the selected environment index from Main Menu.")]
    public string envPrefKey = "SceneToLoad";

    [Header("Environment Setup")]
    [Tooltip("Assign all environment root GameObjects in the same order as the Main Menu selection.")]
    public GameObject[] environmentRoots;

    [Header("Skyboxes")]
    [Tooltip("Skybox materials corresponding 1:1 to environmentRoots.")]
    public Material[] skyboxMaterials;

    [Header("Auto Behavior")]
    [Tooltip("If true, initializes environment automatically on Start().")]
    public bool autoInitOnStart = true;
    [Tooltip("Enable debug logs for initialization steps.")]
    public bool verboseLogging = true;

    public int ActiveEnvironmentIndex { get; private set; } = -1;

    public static event Action<int> EnvironmentApplied;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (autoInitOnStart)
            InitFromPrefs();
    }

    /// <summary>
    /// Reads environment index from PlayerPrefs and applies it.
    /// </summary>
    public void InitFromPrefs()
    {
        int index = PlayerPrefs.GetInt(envPrefKey, 0);

        if (verboseLogging)
            Debug.Log($"[GameplaySceneInitializer] InitFromPrefs: Using {envPrefKey} = {index}");

        ApplyEnvironment(index, saveToPrefs: false);
    }

    /// <summary>
    /// Activates the specified environment, applies skybox, and optionally updates PlayerPrefs.
    /// </summary>
    public void ApplyEnvironment(int index, bool saveToPrefs = true)
    {
        if (environmentRoots == null || environmentRoots.Length == 0)
        {
            Debug.LogError("[GameplaySceneInitializer] No environments assigned!");
            return;
        }

        if (index < 0 || index >= environmentRoots.Length)
        {
            Debug.LogWarning($"[GameplaySceneInitializer] Invalid index {index}. Defaulting to 0.");
            index = 0;
        }

        // Activate only the selected environment
        for (int i = 0; i < environmentRoots.Length; i++)
        {
            if (environmentRoots[i] != null)
                environmentRoots[i].SetActive(i == index);
        }

        // Apply skybox
        ApplySkybox(index);

        ActiveEnvironmentIndex = index;

        if (saveToPrefs)
        {
            PlayerPrefs.SetInt(envPrefKey, index);
            PlayerPrefs.Save();
        }

        if (verboseLogging)
            Debug.Log($"[GameplaySceneInitializer] Environment #{index} activated: {environmentRoots[index].name}");

        EnvironmentApplied?.Invoke(index);
    }

    /// <summary>
    /// Applies the correct skybox for the given environment index.
    /// </summary>
    private void ApplySkybox(int index)
    {
        if (skyboxMaterials != null &&
            index >= 0 && index < skyboxMaterials.Length &&
            skyboxMaterials[index] != null)
        {
            RenderSettings.skybox = skyboxMaterials[index];
            DynamicGI.UpdateEnvironment();

            if (verboseLogging)
                Debug.Log($"[GameplaySceneInitializer] Skybox applied for index {index}.");
        }
        else
        {
            Debug.LogWarning($"[GameplaySceneInitializer] Missing skybox for index {index}.");
        }
    }
    
}
