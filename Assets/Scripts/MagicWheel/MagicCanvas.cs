using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCanvas : MonoBehaviour
{
    [HideInInspector] public RingInterface ringInterface;
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

        ToggleRingInterface(false);
    }
    #endregion Singleton
    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.toggleRingInterface += ToggleRingInterface;
        CallbackHandler.instance.setRingAbility += SetRingAbility;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.toggleRingInterface -= ToggleRingInterface;
        CallbackHandler.instance.setRingAbility -= SetRingAbility;
    }
    #endregion Callbacks

    public void ToggleRingInterface(bool _toggle)
    {
        ringInterface.gameObject.SetActive(_toggle);
    }

    public void ToggleRingInterface()
    {
        ringInterface.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
    }

    // Player Presses Q
    public void SetRingAbility(Ability _ability)
    {
        // Canvas Displays Current Ability
        ringInterface.SetActiveAbility(_ability);

        // Reset Selected Modifiers
        if (ringInterface.gameObject.activeSelf)
        {
            if (ringInterface.activeAbility)
            {
                ringInterface.activeAbility.ResetMods();
            }
        }

        ringInterface.CreateRing();
    }
}
