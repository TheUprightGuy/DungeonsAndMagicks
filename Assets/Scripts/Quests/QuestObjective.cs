using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public virtual void OnDestroy()
    {
        QuestManager.instance.CompleteObjective(this);
    }
}
