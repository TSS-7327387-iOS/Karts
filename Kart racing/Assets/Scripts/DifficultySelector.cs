using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public void SetDifficultyEasy()
    {
        DifficultyManager.SelectedDifficulty = DifficultyLevel.Easy;
    }

    public void SetDifficultyMedium()
    {
        DifficultyManager.SelectedDifficulty = DifficultyLevel.Medium;
    }

    public void SetDifficultyHard()
    {
        DifficultyManager.SelectedDifficulty = DifficultyLevel.Hard;
    }
}
