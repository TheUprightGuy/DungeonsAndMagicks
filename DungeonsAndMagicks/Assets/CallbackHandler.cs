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

    public Action<int, Ability> setAbilityReference;
    public void SetAbilityReference(int _id, Ability _ability)
    {
        if (setAbilityReference != null)
        {
            setAbilityReference(_id, _ability);
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
