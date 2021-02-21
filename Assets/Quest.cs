using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct QuestObjective
{
    public string eventName;
    public int quant;
}

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public List<QuestObjective> objectives;
    UnityAction questListener;
    int index;
    int tracking;

    public void Setup()
    {
        CallbackHandler.instance.CreateQuestTracker(this);
        questListener = new UnityAction(ProgressQuest);
        EventManager.StartListening(objectives[index].eventName, questListener);
        index = 0;
        tracking = 0;
        CallbackHandler.instance.SetQuestText(this, objectives[index].eventName + " " + tracking + "/" + objectives[index].quant);
    }

    private void OnDisable()
    {
        EventManager.StopListening(objectives[index].eventName, questListener);
    }

    public void ProgressQuest()
    {
        // Update Count
        tracking++;
        // Check if ready to progress to next objective
        if (tracking >= objectives[index].quant)
        {
            // stop listening for prev objective
            EventManager.StopListening(objectives[index].eventName, questListener);

            // Progress to next
            tracking = 0;
            index++;

            // Check if at end of objectives
            if (index >= objectives.Count)
            {
                // temp
                CallbackHandler.instance.SetQuestText(this, "");
            }
            // If not progress to next objective
            else
            {
                EventManager.StartListening(objectives[index].eventName, questListener);
                CallbackHandler.instance.SetQuestText(this, objectives[index].eventName + " " + tracking + "/" + objectives[index].quant);
            }
        }
        // Else just update quest text
        else
        {
            CallbackHandler.instance.SetQuestText(this, objectives[index].eventName + " " + tracking + "/" + objectives[index].quant);
        }
    }
}
