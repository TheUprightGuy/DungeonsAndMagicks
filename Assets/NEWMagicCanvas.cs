using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWMagicCanvas : MonoBehaviour
{
    #region Singleton
    public static NEWMagicCanvas instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one NEWMagicCanvas Exists!");
            Destroy(gameObject);
        }

        instance = this;
    }
    #endregion Singleton

    // Player Reference
    public Controller player;

    // Switched On/Off
    public bool display = true;
    public bool crafting = false;

    // Ring Interface prefab;
    public GameObject ringPrefab;
    public GameObject togglePrefab;
    public GameObject runePrefab;

    // Reference to Ability Rings
    public List<NEWRingInterface> abilities;
    NEWRingInterface activeIndex;
    public List<NEWAbilityToggle> toggles;

    // Reference to container Object
    public GameObject ringContainer;
    public GameObject toggleContainer;
    public GameObject runeContainer;

    private void Start()
    {
        ToggleCanvas();
    }

    public void EquipItem()
    {
        ToggleCanvas();
        SetupAbilities();
        SetupRunes();
        ToggleCanvas();
    }

    // Toggles Canvas
    public void ToggleCanvas()
    {
        display = !display;
        ringContainer.SetActive(display);
        toggleContainer.SetActive(display);
        runeContainer.SetActive(crafting && display);

        if (player.equipment)
        {
            ToggleAbility(player.equipment.abilities[player.activeIndex]);
        }
    }
    
    // Sets ring up for each Ability
    public void SetupAbilities()
    {
        CleanUp();

        foreach (Ability n in player.equipment.abilities)
        {
            NEWRingInterface temp = Instantiate(ringPrefab, ringContainer.transform).GetComponent<NEWRingInterface>();
            temp.CreateRing(n);
            abilities.Add(temp);

            NEWAbilityToggle toggle = Instantiate(togglePrefab, toggleContainer.transform).GetComponent<NEWAbilityToggle>();
            toggle.Setup(n);
            toggles.Add(toggle);
        }

        // Toggle to active ability ring
        ToggleAbility(player.equipment.abilities[player.activeIndex]);
    }

    // Toggling between each Ability
    public void ToggleAbility(Ability _ability)
    {
        foreach(NEWRingInterface n in abilities)
        {
            if (n.ability == _ability)
            {
                activeIndex = n;
            }
            n.ShowAbility();

            n.gameObject.SetActive(n.ability == _ability);
        }
    }

    // Show Mod/Socket Details
    public void ShowDetails(NEWOptionPrefab _option)
    {
        activeIndex.ShowDetails(_option);
    }

    // Show Ability Details
    public void ShowAbility(NEWOptionPrefab _option)
    {
        activeIndex.ShowAbility();
    }

    // Initial Setup for Runes
    public void SetupRunes()
    {
        foreach(Mod n in player.runes)
        {
            Rune temp = Instantiate(runePrefab, runeContainer.transform).GetComponentInChildren<Rune>();
            temp.mod = n;
        }
    }
    // Add Rune - may change to actual inventory system in future
    public void AddRune(Mod _rune)
    {
        Rune temp = Instantiate(runePrefab, runeContainer.transform).GetComponentInChildren<Rune>();
        temp.mod = _rune;
    }

    // Clears all rings, called on picking up new weapon
    public void CleanUp()
    {
        activeIndex = null;
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            Destroy(abilities[i].gameObject);
        }
        abilities.Clear();
        for (int i = toggles.Count - 1; i >= 0; i--)
        {
            Destroy(toggles[i].gameObject);
        }
        toggles.Clear();
    }
}
