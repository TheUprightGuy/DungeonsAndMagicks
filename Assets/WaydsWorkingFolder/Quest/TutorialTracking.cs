using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTracking : QuestManager
{
    #region Singleton
    new public static TutorialTracking instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one TutorialTracking exists!");
            Destroy(this.gameObject);
        }
        instance = this;

        TutorialPointer[] pointers = GetComponentsInChildren<TutorialPointer>();

        for (int i = 0; i < pointers.Length; i++)
        {
            pointers[i].id = i;
        }

        Invoke("Setup", 0.1f);
    }
    public void Setup()
    {
        TogglePS(0);
    }
    #endregion Singleton

    public Action<int> togglePS;
    public void TogglePS(int _id)
    {
        if (togglePS != null)
        {
            togglePS(_id);
        }
    }

    public override void CompleteObjective(QuestObjective _quest)
    {
        quests.Remove(_quest);
        Debug.Log("Quest Completed");
        TogglePS(((TutorialObjective)_quest).id + 1);
    }
}
