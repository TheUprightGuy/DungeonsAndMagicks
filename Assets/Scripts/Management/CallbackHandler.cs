using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CallbackHandler : MonoBehaviour
{
    #region Singleton
    public static CallbackHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Callback Handler Exists!");
            Destroy(this.gameObject);
        }
        instance = this;
    }
    #endregion Singleton

    public Action togglePause;
    public void TogglePause()
    {
        // Temp
        PauseMenuCanvasController.instance.gameObject.SetActive(!PauseMenuCanvasController.instance.gameObject.activeSelf);

        if (togglePause != null)
        {
            togglePause();
        }
    }

    public Action<int> setActiveAbility;
    public void SetActiveAbility(int _id)
    {
        if (setActiveAbility != null)
        {
            setActiveAbility(_id);
        }
    }

    public Action<Mod> addRune;
    public void AddRune(Mod _rune)
    {
        if (addRune != null)
        {
            addRune(_rune);
        }
    }

    public Action<Mod> removeRune;
    public void RemoveRune(Mod _rune)
    {
        if (removeRune != null)
        {
            removeRune(_rune);
        }
    }

    public Action<int, Ability> setAbilityReference;
    public void SetAbilityReference(int _id, Ability _ability)
    {
        if (setAbilityReference != null)
        {
            setAbilityReference(_id, _ability);
        }
    }

    public Action<int> showAbility;
    public void ShowAbility(int _i)
    {
        if (showAbility != null)
        {
            showAbility(_i);
        }
    }

    public Action<int> hideAbility;
    public void HideAbility(int _i)
    {
        if (hideAbility != null)
        {
            hideAbility(_i);
        }
    }

    public Action<float> updateHealth;
    public void UpdateHealth(float _health)
    {
        if (updateHealth != null)
        {
            updateHealth(_health);
        }
    }

    public Action<float> updateMana;
    public void UpdateMana(float _mana)
    {
        if (updateMana != null)
        {
            updateMana(_mana);
        }
    }

    public Action<Ability> updateAbilityModList;
    public void UpdateAbilityModList(Ability _ability)
    {
        if (updateAbilityModList != null)
        {
            updateAbilityModList(_ability);
        }
    }

    public Action<Buff> addBuffToUI;
    public void AddBuffToUI(Buff _buff)
    {
        if (addBuffToUI != null)
        {
            addBuffToUI(_buff);
        }
    }
    public Action<Buff> removeBuffFromUI;
    public void RemoveBuffFromUI(Buff _buff)
    {
        if (removeBuffFromUI != null)
        {
            removeBuffFromUI(_buff);
        }
    }

    public Action<float> setCastSpeed;
    public void SetCastSpeed(float _speed)
    {
        if (setCastSpeed != null)
        {
            setCastSpeed(_speed);
        }
    }

    public Action<string, float> setDialogueText;
    public void SetDialogueText(string _text, float _time)
    {
        if (setDialogueText != null)
        {
            setDialogueText(_text, _time);
        }
    }

    public Action<Quest,string> setQuestText;
    public void SetQuestText(Quest _quest, string _text)
    {
        if (setQuestText != null)
        {
            setQuestText(_quest, _text);
        }
    }

    public Action<Quest> createQuestTracker;
    public void CreateQuestTracker(Quest _quest)
    {
        if (createQuestTracker != null)
        {
            createQuestTracker(_quest);
        }
    }

    public void QuitToMenu()
    {
        // SceneLoad here
    }

    private void Start()
    {
        TogglePause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
