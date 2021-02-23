using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTextUI : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;
    Quest quest;
    private void Awake()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }
    public void Setup(Quest _quest)
    {
        quest = _quest;
    }

    public void SetQuestText(Quest _quest, string _text)
    {
        if (quest == _quest)
        {
            text.SetText(_text);
        }
    }
}
