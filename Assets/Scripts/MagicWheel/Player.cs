using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Player class currently handles inventory management, hotkey input and animations. */
public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Player exists!");
            Destroy(this.gameObject);
        }
        instance = this;

        animator = GetComponentInChildren<Animator>();
        GetComponentInChildren<PCAnimatorFunctions>().controller = this;
    }
    #endregion Singleton
    #region Setup & Callbacks
    private void Start()
    {
        CallbackHandler.instance.addRune += AddRune;
        CallbackHandler.instance.removeRune += RemoveRune;
        CallbackHandler.instance.getPlayerCoins += GetPlayerCoins;
        CallbackHandler.instance.giveCoins += GiveCoins;
        CallbackHandler.instance.spendCoins += SpendCoins;

        Invoke("SetupStuff", 0.1f);
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.addRune -= AddRune;
        CallbackHandler.instance.removeRune -= RemoveRune;
        CallbackHandler.instance.getPlayerCoins -= GetPlayerCoins;
        CallbackHandler.instance.giveCoins -= GiveCoins;
        CallbackHandler.instance.spendCoins -= SpendCoins;
    }
    public void SetupStuff()
    {
        SetupAbilityReferences();
        CallbackHandler.instance.SetCastSpeed(castspeed);
        activeIndex = 0;
    }
    #endregion Setup & Callbacks
    #region EditorFunctions
    private void OnApplicationQuit()
    {
        if (equipment)
        {
            foreach (Ability n in equipment.abilities)
            {
                n.ResetMods();
            }
        }
    }
    #endregion EditorFunctions

    //[HideInInspector]
    public int coins;
    [HideInInspector] public Weapon equipment;
    //[HideInInspector]
    public List<Mod> runes;
    [HideInInspector] public int activeIndex = 0;
    Animator animator;
       
    [Header("Setup Fields")]
    public float castspeed = 1.0f;

    public void SpendCoins(int _coins)
    {
        coins -= _coins;
    }

    public void GiveCoins(int _gold)
    {
        coins += _gold;
    }

    public int GetPlayerCoins()
    {
        return coins;
    }

    private void Update()
    {
        // Reset Animator Booleans
        animator.SetBool("ProjectileCast", false);
        animator.SetBool("MovementCast", false);

        // Toggle Magic Menus - temp
        if (Input.GetKeyDown(KeyCode.C))
        {
            MagicUI.instance.ToggleCanvas();
        }
        // Testing Dialogue Box - temp
        if (Input.GetKeyDown(KeyCode.R))
        {
            CallbackHandler.instance.SetDialogueText("Testing Callback", 3.0f);
        }

        // Use Ability on Right Click - consider changing this to hotkeys instead
        if (Input.GetMouseButtonDown(1))
        {
            if (CharacterStats.instance.Control() && equipment)
            {
                // Callback might be better for this
                switch(equipment.abilities[activeIndex].type)
                {
                    case AbilityType.Projectile:
                    {
                        animator.SetBool("ProjectileCast", true);
                        break;
                    }
                    case AbilityType.Buff:
                    {
                        animator.SetBool("ProjectileCast", true);
                        break;
                    }
                    case AbilityType.Movement:
                    {
                        animator.SetBool("MovementCast", true);
                        break;
                    }
                }
            }
        }

        // Switch ability on num press - consider changing this in future
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeIndex = 0;
            CallbackHandler.instance.SetActiveAbility(activeIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeIndex = 1;
            CallbackHandler.instance.SetActiveAbility(activeIndex);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeIndex = 2;
            CallbackHandler.instance.SetActiveAbility(activeIndex);
        }
    }

    // Pretty self explanatory
    public void AddRune(Mod _rune)
    {
        runes.Add(_rune);
    }
    public void RemoveRune(Mod _rune)
    {
        runes.Remove(_rune);
    }

    // Sets up Abilities on Item pickup
    public void EquipItem(Weapon _item)
    {
        equipment = _item;

        foreach (Ability n in _item.abilities)
        {
            n.StartUp();
        }
        SetupAbilityReferences();
        CallbackHandler.instance.SetActiveAbility(0);
    }

    // Links Equipment Abilities to UI Elements
    public void SetupAbilityReferences()
    {
        for (int i = 0; i < 3; i++)
        {
            if (equipment && i < equipment.abilities.Count)
            {
                CallbackHandler.instance.ShowAbility(i);
                CallbackHandler.instance.SetAbilityReference(i, equipment.abilities[i]);
            }
            else
            {
                CallbackHandler.instance.HideAbility(i);
            }
        }
    }

    // Use ability at index
    public void Cast()
    {
        equipment.abilities[activeIndex].Use(transform);
    }
}
