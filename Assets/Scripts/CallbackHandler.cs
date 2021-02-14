using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CallbackHandler : MonoBehaviour
{
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

    public Action togglePause;
    public void TogglePause()
    {
        // Temp
        CanvasController.instance.gameObject.SetActive(!CanvasController.instance.gameObject.activeSelf);

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

    public Action toggleRingInterface;
    public void ToggleRingInterface(Ability _ability)
    {
        if (toggleRingInterface != null)
        {
            toggleRingInterface();
            SetRingAbility(_ability);
        }
    }
    public Action<Ability> setRingAbility;
    public void SetRingAbility(Ability _ability)
    {
        if (setRingAbility != null)
        {
            setRingAbility(_ability);
        }
    }

    public Action toggleCraftInterface;
    public void ToggleCraftInterface()
    {
        if (toggleCraftInterface != null)
        {
            toggleCraftInterface();
        }
    }

    public Action<bool> closeCraftInterface;
    public void CloseCraftInterface(bool _toggle)
    {
        if (closeCraftInterface != null)
        {
            closeCraftInterface(_toggle);
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

    public Action<string, float> setText;
    public void SetText(string _text, float _time)
    {
        if (setText != null)
        {
            setText(_text, _time);
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
