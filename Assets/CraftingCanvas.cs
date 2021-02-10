using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCanvas : MonoBehaviour
{
    [HideInInspector] public RingInterface ringInterface;
    [HideInInspector] public RunesUI runesUI;
    public Controller pc;
    public static CraftingCanvas instance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            CallbackHandler.instance.ToggleCraftInterface();
            SetRingAbility(pc.abilities[pc.activeIndex]);
            SetRunes(pc.runes);
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
    }

    public void ToggleCraftInterface()
    {
        ringInterface.gameObject.SetActive(!ringInterface.gameObject.activeSelf);
        runesUI.gameObject.SetActive(!runesUI.gameObject.activeSelf);

        if (!runesUI.gameObject.activeSelf)
        {
            runesUI.CleanUp();
        }
    }

    public void SetRunes(List<Mod> _runes)
    {
        runesUI.DisplayRunes(_runes);
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
            ToggleCraftInterface();
        }
    }
}
