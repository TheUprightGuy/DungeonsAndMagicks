using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    public Item equipment;
    public List<Mod> runes;




    //public List<Ability> abilities;
    [HideInInspector] public int activeIndex = 0;
    Animator animator;

    public float castspeed = 1.0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        GetComponentInChildren<PCAnimatorFunctions>().controller = this;
    }

    private void Start()
    {
        CallbackHandler.instance.addRune += AddRune;
        CallbackHandler.instance.removeRune += RemoveRune;

        Invoke("SetupAbilityReferences", 0.1f);

        Invoke("SetupStuff", 0.1f);
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.addRune -= AddRune;
        CallbackHandler.instance.removeRune -= RemoveRune;
    }

    public void SetupStuff()
    {
        CallbackHandler.instance.SetCastSpeed(castspeed);
        activeIndex = 0;
    }

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

    public void AddRune(Mod _rune)
    {
        runes.Add(_rune);
    }

    public void RemoveRune(Mod _rune)
    {
        runes.Remove(_rune);
    }

    public void EquipItem(Item _item)
    {
        equipment = _item;

        foreach(Ability n in _item.abilities)
        {
            //abilities.Add(n);
            n.StartUp();
        }
        //abilities[activeIndex] = _item.abilities[0];
        SetupAbilityReferences();
        CallbackHandler.instance.SetActiveAbility(0);
    }

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

    private void Update()
    {
        animator.SetBool("ProjectileCast", false);
        animator.SetBool("MovementCast", false);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CallbackHandler.instance.ToggleRingInterface(equipment.abilities, activeIndex);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CallbackHandler.instance.SetText("Testing Callback", 3.0f);
        }

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeIndex = 0;
            CallbackHandler.instance.SetActiveAbility(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeIndex = 1;
            CallbackHandler.instance.SetActiveAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeIndex = 2;
            CallbackHandler.instance.SetActiveAbility(2);
        }
    }

    public void Cast()
    {
        equipment.abilities[activeIndex].Use(transform);
    }
}
