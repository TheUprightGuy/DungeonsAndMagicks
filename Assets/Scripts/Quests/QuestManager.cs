using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one QuestManager exists!");
            Destroy(this.gameObject);
        }
        instance = this;
    }
    #endregion Singleton

    public List<QuestObjective> quests;

    public virtual void CompleteObjective(QuestObjective _quest)
    {
        quests.Remove(_quest);
        Debug.Log("Quest Completed");
    }
}
