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
    public bool display;

    // Ring Interface prefab;
    public GameObject ringPrefab;
    public GameObject togglePrefab;

    // Reference to Ability Rings
    public List<NEWRingInterface> abilities;
    NEWRingInterface activeIndex;

    // Reference to container Object
    public GameObject container;
    public GameObject toggleContainer;



    private void Update()
    {
        // temp - this will be on equip
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetupAbilities();
        }

        // temp
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCanvas();
        }
    }


    // Toggles Canvas
    public void ToggleCanvas()
    {
        display = !display;
        container.SetActive(display);
        toggleContainer.SetActive(display);
    }
    
    // Sets ring up for each Ability
    public void SetupAbilities()
    {
        foreach (Ability n in player.equipment.abilities)
        {
            NEWRingInterface temp = Instantiate(ringPrefab, container.transform).GetComponent<NEWRingInterface>();
            temp.CreateRing(n);
            abilities.Add(temp);

            NEWAbilityToggle toggle = Instantiate(togglePrefab, toggleContainer.transform).GetComponent<NEWAbilityToggle>();
            toggle.Setup(n);
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


    // Displays Rune Inventory
    public void UpdateRunes()
    {

    }

    // Clears all rings, called on picking up new weapon
    public void CleanUp()
    {

    }
}
