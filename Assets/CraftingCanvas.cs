using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCanvas : MonoBehaviour
{
    [HideInInspector] public RingInterface ringInterface;
    public Controller pc;
    public static CraftingCanvas instance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            ToggleRingInterface(true);
            SetRingAbility(pc.abilities[pc.activeIndex]);
        }
    }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one crafting station exists!");
            Destroy(this.gameObject);
        }
        instance = this;
        ringInterface = GetComponentInChildren<RingInterface>();
    }

    private void Start()
    {
        ToggleRingInterface(false);
    }

    public void ToggleRingInterface(bool _toggle)
    {
        ringInterface.gameObject.SetActive(_toggle);
    }

    public void ToggleRingInterface()
    {
        ringInterface.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
    }

    public void ResetAbility()
    {
        if (ringInterface.gameObject.activeSelf)
        {
            if (ringInterface.activeAbility)
            {
                ringInterface.activeAbility.ResetMods();
            }
        }
    }

    public void SetRingAbility(Ability _ability)
    {
        ringInterface.SetActiveAbility(_ability);
        ResetAbility();
        ringInterface.CreateRing(0);
    }

    public void NextRing()
    {
        if (!ringInterface.NextRing())
        {
            ringInterface.currentData = 0;
            ToggleRingInterface();
        }
    }
}
