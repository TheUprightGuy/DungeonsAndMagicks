using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/CollectQuest", order = 1)]
public class CollectQuest : Quest
{
    public GameObject target;
    public int required;
    int current = 0;

    public void CollectItem()
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
