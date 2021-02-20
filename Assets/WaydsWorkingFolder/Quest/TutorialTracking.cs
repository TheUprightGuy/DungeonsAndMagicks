using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTracking : QuestManager
{
    public void Awake()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].id = i;
        }
    }

    public Action<int> progress;
    public override void Progress(int _id)
    {
        if (progress != null)
        {
            progress(_id);
        }
    }
}
