using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public GameObject questPrefab;
    public List<QuestTextUI> quests;

    private void Start()
    {
        CallbackHandler.instance.createQuestTracker += CreateQuestTracker;
        CallbackHandler.instance.setQuestText += SetQuestText;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.createQuestTracker -= CreateQuestTracker;
        CallbackHandler.instance.setQuestText -= SetQuestText;
    }

    public void CreateQuestTracker(Quest _quest)
    {
        QuestTextUI temp = Instantiate(questPrefab, this.transform).GetComponent<QuestTextUI>();
        temp.Setup(_quest);
        quests.Add(temp);
    }

    public void SetQuestText(Quest _quest, string _text)
    {
        foreach (QuestTextUI n in quests)
        {
            n.SetQuestText(_quest, _text);
        }
    }
}
