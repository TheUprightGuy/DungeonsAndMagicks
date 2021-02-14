using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/KillQuest", order = 1)]
public class KillQuest : Quest
{
    public GameObject target;
    public int required;
    int current = 0;

    public void Kill()
    {
        current++;
        if (current >= required)
        {
            ProgressQuest();
        }
    }

    public void ProgressQuest()
    {
        TutorialTracking.instance.ProgressTutorial();
    }
}
