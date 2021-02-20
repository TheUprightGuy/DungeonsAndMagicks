using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public int id;
    private void OnDestroy()
    {
        QuestManager.instance.CompleteObjective(this);
    }
}
