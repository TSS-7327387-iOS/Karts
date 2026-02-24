using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowOrigin : MonoBehaviour
{
    public static FlowOrigin Instance { get; private set; }

    private const string PREF_KEY = "CameFromMainMenuFlag";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Call from Main Menu JUST before loading the gameplay scene.</summary>
    public static void MarkComingFromMainMenu()
    {
        PlayerPrefs.SetInt(PREF_KEY, 1);
        PlayerPrefs.Save();
#if UNITY_EDITOR
        Debug.Log("[FlowOrigin] Marked: coming from Main Menu.");
#endif
    }

    /// <summary>Call from gameplay flow (LoadNextLevel etc.) BEFORE loading gameplay again.</summary>
    public static void MarkComingFromGameplay()
    {
        PlayerPrefs.SetInt(PREF_KEY, 0);
        PlayerPrefs.Save();
#if UNITY_EDITOR
        Debug.Log("[FlowOrigin] Marked: NOT from Main Menu (gameplay reload).");
#endif
    }

    /// <summary>
    /// Returns true *once* if last mark was from Main Menu. Resets flag to 0 after reading.
    /// Use in Gameplay Awake/Start.
    /// </summary>
    public static bool ReadAndConsumeCameFromMainMenu()
    {
        int v = PlayerPrefs.GetInt(PREF_KEY, 0);
        if (v == 1)
        {
            // consume
            PlayerPrefs.SetInt(PREF_KEY, 0);
            PlayerPrefs.Save();
#if UNITY_EDITOR
            Debug.Log("[FlowOrigin] Consumed Main Menu flag: TRUE.");
#endif
            return true;
        }
#if UNITY_EDITOR
        Debug.Log("[FlowOrigin] Consumed Main Menu flag: FALSE.");
#endif
        return false;
    }
}
