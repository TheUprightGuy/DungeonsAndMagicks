using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialChecklist
{
    PickUpWand = 0,
    CollectRunes,
    SocketRune,
    KillEnemy,
    GetRuneFromShop,
    CraftWand,
    FightBoss,
    OpenChest,
    End
}

public class TutorialTracking : MonoBehaviour
{
    public static TutorialTracking instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Tutorial Tracking instance already exists!");
            Destroy(this.gameObject);
        }
        instance = this;
    }

    public TutorialChecklist tutorialStage;
    public Quest[] quests;
    public List<GameObject> pointers;

    private void Start()
    {
        foreach(Transform n in transform)
        {
            pointers.Add(n.gameObject);
        }

        SetPointer();
    }

    public void SetPointer()
    {
        for (int i = 0; i < pointers.Count; i++)
        {
            pointers[i].SetActive(i == (int)tutorialStage);
        }
    }

    public Action<int> progress;
    public void Progress()
    {
        if (progress != null)
        {
            progress((int)tutorialStage);
        }
    }

    public void ProgressTutorial()
    {
        tutorialStage++;
        SetPointer();
        Progress();
    }

    public void CheckQuest(GameObject _object)
    {
        switch(quests[(int)tutorialStage].questType)
        {
            case QuestType.Collect:
            {
                if (((CollectQuest)quests[(int)tutorialStage]).target.name == _object.name)
                {
                    ((CollectQuest)quests[(int)tutorialStage]).CollectItem();
                }
                break;
            }
            case QuestType.Kill:
            {
                if (((KillQuest)quests[(int)tutorialStage]).target.name == _object.name)
                {
                    ((KillQuest)quests[(int)tutorialStage]).Kill();
                }
                break;
            }
        }
    }

    public void CheckQuest(Mod _mod)
    {
        if (quests[(int)tutorialStage].questType == QuestType.Socket)
        {
            ((SocketQuest)quests[(int)tutorialStage]).Socket(_mod);      
        }
    }
}
