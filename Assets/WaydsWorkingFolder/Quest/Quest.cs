using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Collect = 0,
    Kill,
    Socket
}


//[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public QuestType questType;

    public bool completed;
}
