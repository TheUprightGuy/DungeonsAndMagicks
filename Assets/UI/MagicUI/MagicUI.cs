using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUI : MonoBehaviour
{
    #region Singleton
    public static MagicUI instance;
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
    #region Setup
    private void Start()
    {
        player = Player.instance;
        ToggleCanvas();
    }
    #endregion Setup
    // Player Reference
    Player player;
    // Display Status
    bool display = true;
    bool crafting = false;
    // Container Element References
    RingInterface activeIndex;
    List<RingInterface> abilities = new List<RingInterface>();
    List<AbilityToggle> toggles = new List<AbilityToggle>();

    /* CHANGE THESE TO RESOURCE LOAD IN FUTURE */
    [Header("Prefab References")]
    public GameObject ringPrefab;
    public GameObject togglePrefab;
    public GameObject runePrefab;
    [Header("Container References")]
    public GameObject ringContainer;
    public GameObject toggleContainer;
    public GameObject runeContainer;

    // Toggles Canvas & Elements On/Off
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

    // Sets up Ability & Rune references
    public void SetupItem()
    {
        ToggleCanvas();
        SetupAbilities();
        SetupRunes();
        ToggleCanvas();
    }
    // Sets ring up for each Ability
    public void SetupAbilities()
    {
        CleanUp();

        foreach (Ability n in player.equipment.abilities)
        {
            RingInterface temp = Instantiate(ringPrefab, ringContainer.transform).GetComponent<RingInterface>();
            temp.CreateRing(n);
            abilities.Add(temp);

            AbilityToggle toggle = Instantiate(togglePrefab, toggleContainer.transform).GetComponent<AbilityToggle>();
            toggle.Setup(n);
            toggles.Add(toggle);
        }

        // Toggle to active ability ring
        ToggleAbility(player.equipment.abilities[player.activeIndex]);
    }

    // Toggling between each Ability
    public void ToggleAbility(Ability _ability)
    {
        foreach(RingInterface n in abilities)
        {
            if (n.ability == _ability)
            {
                activeIndex = n;
            }
            n.ShowAbility();

            n.gameObject.SetActive(n.ability == _ability);
        }
    }

    // Show Mod/Socket Details on Mouseover
    public void ShowDetails(SocketPrefab _option)
    {
        activeIndex.ShowDetails(_option);
    }

    // Show Ability Details
    public void ShowAbility(SocketPrefab _option)
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
    public void SetCrafting(bool _toggle)
    {
        crafting = _toggle;
    }
}
