using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    // public Transform player;
    // public List<Transform> aiKarts;
    // public RectTransform playerIcon;
    // public List<RectTransform> aiIcons;
    // public RectTransform minimap; // Minimap UI Panel
    // public Transform trackMinBound; // Top-left world position
    // public Transform trackMaxBound; // Bottom-right world position
    //
    // public Vector2 minimapOffset = Vector2.zero; // Fine-tune adjustment
    // public float minimapScaleFactor = 1.0f; // Adjust to fit minimap
    //
    // void Update()
    // {
    //     UpdateMinimapIcon(player, playerIcon);
    //     for (int i = 0; i < aiKarts.Count; i++)
    //     {
    //         if (aiIcons[i] != null && aiKarts[i] != null)
    //             UpdateMinimapIcon(aiKarts[i], aiIcons[i]);
    //     }
    // }
    //
    // void UpdateMinimapIcon(Transform car, RectTransform icon)
    // {
    //     // Get world space bounds
    //     Vector2 worldMin = new Vector2(trackMinBound.position.x, trackMinBound.position.z);
    //     Vector2 worldMax = new Vector2(trackMaxBound.position.x, trackMaxBound.position.z);
    //
    //     // Get UI space bounds
    //     float minimapWidth = minimap.rect.width;
    //     float minimapHeight = minimap.rect.height;
    //
    //     // Normalize position in world space (0 to 1)
    //     Vector2 worldPos = new Vector2(car.position.x, car.position.z);
    //     float normX = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
    //     float normY = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.y);
    //
    //     // Convert to minimap UI coordinates
    //     float mapX = Mathf.Lerp(-minimapWidth / 2, minimapWidth / 2, normX) * minimapScaleFactor + minimapOffset.x;
    //     float mapY = Mathf.Lerp(-minimapHeight / 2, minimapHeight / 2, normY) * minimapScaleFactor + minimapOffset.y;
    //
    //     // Set the icon position in UI
    //     icon.anchoredPosition = new Vector2(mapX, mapY);
    //
    //     // Optional: Rotate the icon based on car's rotation
    //     icon.rotation = Quaternion.Euler(0, 0, -car.eulerAngles.y);
    // }
    //
    // public void InitMinimapData(Transform _player, GameObject[] _aIkarts)
    // {
    //     player = _player;
    //     aiKarts = _aIkarts.Select(kart => kart.transform).ToList();
    // }
    
     [Header("Karts & Minimap")]
    public Transform player;
    public List<Transform> aiKarts;
    public RectTransform playerIcon;
    public List<RectTransform> aiIcons;
    public RectTransform minimap; // Minimap UI Panel
    public Transform trackMinBound; // Top-left world position
    public Transform trackMaxBound; // Bottom-right world position
    public Vector2 minimapOffset = Vector2.zero;
    public float minimapScaleFactor = 1.0f;

    [Header("Character Faces")]
    public Sprite[] playerCharacterFaces; // link your player face sprites here
    public Sprite[] enemyCharacterFaces;  // link your enemy face sprites here

    [Header("Icon Images")]
    public Image playerIconImage; // the Image component for the player icon
    public List<Image> aiIconImages; // Image components for the AI icons

    // void Start()
    // {
    //     SetupIcons();
    // }

    void Update()
    {
        UpdateMinimapIcon(player, playerIcon);
        for (int i = 0; i < aiKarts.Count; i++)
        {
            if (aiIcons[i] != null && aiKarts[i] != null)
                UpdateMinimapIcon(aiKarts[i], aiIcons[i]);
        }
    }

    void UpdateMinimapIcon(Transform car, RectTransform icon)
    {
        Vector2 worldMin = new Vector2(trackMinBound.position.x, trackMinBound.position.z);
        Vector2 worldMax = new Vector2(trackMaxBound.position.x, trackMaxBound.position.z);

        float minimapWidth = minimap.rect.width;
        float minimapHeight = minimap.rect.height;

        Vector2 worldPos = new Vector2(car.position.x, car.position.z);
        float normX = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPos.x);
        float normY = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPos.y);

        float mapX = Mathf.Lerp(-minimapWidth / 2, minimapWidth / 2, normX) * minimapScaleFactor + minimapOffset.x;
        float mapY = Mathf.Lerp(-minimapHeight / 2, minimapHeight / 2, normY) * minimapScaleFactor + minimapOffset.y;

        icon.anchoredPosition = new Vector2(mapX, mapY);
        icon.rotation = Quaternion.Euler(0, 0, -car.eulerAngles.y);
    }

    /// <summary>
    /// Initializes the minimap icons based on spawned karts.
    /// Call this from GameController after spawning.
    /// </summary>
    /// <param name="_player">The spawned player kart Transform</param>
    /// <param name="_aiKarts">The spawned AI karts</param>
    public void InitMinimapData(Transform _player, GameObject[] _aiKarts)
    {
        player = _player;
        aiKarts = _aiKarts.Select(k => k.transform).ToList();
    }

    /// <summary>
    /// Sets the correct face sprites based on GameController data.
    /// </summary>
    public void SetupIcons()
    {
        // Get correct index for player from PlayerPrefs
        int playerIndex = PlayerPrefs.GetInt("Player", 0);
        if (playerCharacterFaces.Length > playerIndex)
        {
            playerIconImage.sprite = playerCharacterFaces[playerIndex];
        }
        else
        {
            Debug.LogWarning($"Player index {playerIndex} is out of bounds for playerCharacterFaces!");
        }

        // Get enemy faces based on GameController
        if (GameController.instance != null)
        {
            GameObject[] enemyKarts = GameController.instance.opponentKarts;

            for (int i = 0; i < enemyKarts.Length; i++)
            {
                int enemyIndex = i; // same as spawn order
                if (enemyCharacterFaces.Length > enemyIndex && aiIconImages.Count > i)
                {
                    aiIconImages[i].sprite = enemyCharacterFaces[enemyIndex];
                }
                else
                {
                    Debug.LogWarning($"Enemy index {enemyIndex} is out of bounds for enemyCharacterFaces or aiIconImages!");
                }
            }
        }
        else
        {
            Debug.LogWarning("GameController instance not found for minimap icon setup!");
        }
    }
}
