using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyLevel SelectedDifficulty = DifficultyLevel.Medium;

    public static float GetEnemyMaxSpeed()
    {
        switch (SelectedDifficulty)
        {
            case DifficultyLevel.Easy:
                return 15f;
            case DifficultyLevel.Medium:
                return 25f;
            case DifficultyLevel.Hard:
                return 30f;
            default:
                return 25f;
        }
    }
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}
