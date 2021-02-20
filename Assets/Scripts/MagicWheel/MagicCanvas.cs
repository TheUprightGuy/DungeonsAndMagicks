using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCanvas : MonoBehaviour
{
    [HideInInspector] public RingInterface ringInterface;
    [HideInInspector] public AbilityToggleUI toggleUI;
    #region Singleton
    public static MagicCanvas instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one MagicCanvas Exists!");
            Destroy(gameObject);
        }

        instance = this;
        ringInterface = GetComponentInChildren<RingInterface>();
        toggleUI = GetComponentInChildren<AbilityToggleUI>();

        ToggleRingInterface(false);
    }
    #endregion Singleton
    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.toggleRingInterface += ToggleRingInterface;
        CallbackHandler.instance.setRingAbilities += SetRingAbilities;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.toggleRingInterface -= ToggleRingInterface;
        CallbackHandler.instance.setRingAbilities -= SetRingAbilities;
    }
    #endregion Callbacks

    public void ToggleRingInterface(bool _toggle)
    {
        ringInterface.gameObject.SetActive(_toggle);
        toggleUI.gameObject.SetActive(_toggle);
    }

    public void ToggleRingInterface()
    {
        ringInterface.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
        toggleUI.gameObject.SetActive(!toggleUI.gameObject.activeSelf);

        if (!toggleUI.gameObject.activeSelf)
        {
            toggleUI.CleanUp();
        }
    }

    public void SetRingAbility(Ability _ability)
    {
        ringInterface.SetActiveAbility(_ability);
        ringInterface.CreateRing();
    }

    // Player Presses Q
    public void SetRingAbilities(List<Ability> _abilities, int _index)
    {
        // Canvas Displays Current Ability
        ringInterface.SetActiveAbility(_abilities[_index]);

        // Reset Selected Modifiers
        if (ringInterface.gameObject.activeSelf)
        {
            toggleUI.Setup(_abilities);

            if (ringInterface.activeAbility)
            {
                ringInterface.activeAbility.ResetMods();
            }
        }

        ringInterface.CreateRing();
    }
}
