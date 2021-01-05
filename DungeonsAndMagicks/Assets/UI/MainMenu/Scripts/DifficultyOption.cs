using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    EASY = 0,
    NORMAL,
    HARD
}

public class DifficultyOption : MonoBehaviour
{
    public Difficulty difficulty;
    public TMPro.TextMeshProUGUI text;

    public void IncreaseDifficulty()
    {
        if ((int)difficulty < 2)
        {
            difficulty++; 
        }
        UpdateText();
    }

    public void LowerDifficulty()
    {
        if ((int)difficulty > 0)
        {
            difficulty--;
        }
        UpdateText();
    }

    public void UpdateText()
    {
        text.SetText(difficulty.ToString());
    }
}
