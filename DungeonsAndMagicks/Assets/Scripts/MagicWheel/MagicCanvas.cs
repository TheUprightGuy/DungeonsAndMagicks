using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCanvas : MonoBehaviour
{
    public static MagicCanvas instance;
    private RingInterface ringInterface;
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

    public void ToggleRingInterface(bool _toggle)
    {
        ringInterface.gameObject.SetActive(_toggle);
        ResetAbility();
    }

    public void ToggleRingInterface()
    {
        ringInterface.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
        ResetAbility();
    }

    public void ResetAbility()
    {
        if (ringInterface.gameObject.activeSelf)
        {
            if (ringInterface.activeAbility)
            ringInterface.activeAbility.ResetMods();
        }
    }

    public void SetActiveAbility(Ability _ability)
    {
        ringInterface.activeAbility = _ability;
    }
}
