using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCanvas : MonoBehaviour
{
    [HideInInspector] public RingInterface ringInterface;
    [HideInInspector] public RunesUI runesUI;
    [HideInInspector] public AbilityToggleUI toggleUI;

    public Controller pc;
    public static CraftingCanvas instance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            CallbackHandler.instance.ToggleCraftInterface();
            if (ringInterface.gameObject.activeSelf)
            {
                SetRingAbility(pc.equipment.abilities[pc.activeIndex]);
                DisplayRunes();
                SetAbilities(pc.equipment.abilities);
            }
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
        runesUI = GetComponentInChildren<RunesUI>();
        toggleUI = GetComponentInChildren<AbilityToggleUI>();
    }

    private void Start()
    {
        CallbackHandler.instance.toggleCraftInterface += ToggleCraftInterface;
        CallbackHandler.instance.closeCraftInterface += CloseCraftInterface;
        CallbackHandler.instance.setRingAbility += SetRingAbility;

        CloseCraftInterface(false);
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.toggleCraftInterface -= ToggleCraftInterface;
        CallbackHandler.instance.closeCraftInterface -= CloseCraftInterface;
        CallbackHandler.instance.setRingAbility -= SetRingAbility;
    }

    public void CloseCraftInterface(bool _toggle)
    {
        ringInterface.gameObject.SetActive(_toggle);
        runesUI.gameObject.SetActive(_toggle);
        toggleUI.gameObject.SetActive(_toggle);

        GameplayUI.instance.gameObject.SetActive(!_toggle);
    }

    public void ToggleCraftInterface()
    {
        ringInterface.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
        runesUI.gameObject.SetActive(!runesUI.gameObject.activeSelf);
        toggleUI.gameObject.SetActive(!toggleUI.gameObject.activeSelf);

        if (!runesUI.gameObject.activeSelf)
        {
            runesUI.CleanUp();
            toggleUI.CleanUp();
        }

        GameplayUI.instance.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
    }

    public void SetAbilities(List<Ability> _abilities)
    {
        toggleUI.Setup(_abilities);
    }

    public void DisplayRunes()
    {
        runesUI.DisplayRunes(pc.runes);
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
        ringInterface.CreateRing();
    }
}
