using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObjective : QuestObjective
{
    public int id;
    private void Start()
    {
        TutorialTracking.instance.quests.Add(this);
    }

    public override void OnDestroy()
    {
        TutorialTracking.instance.CompleteObjective(this);
    }
}
