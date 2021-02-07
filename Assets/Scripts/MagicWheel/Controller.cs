using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    public List<Item> equipment;




    public List<Ability> abilities;
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
        Invoke("SetupAbilityReferences", 0.1f);

        Invoke("SetupStuff", 0.1f);
    }

    public void SetupStuff()
    {
        CallbackHandler.instance.SetCastSpeed(castspeed);
        activeIndex = 0;
    }

    private void OnApplicationQuit()
    {
        foreach(Ability n in abilities)
        {
            n.ResetMods();
        }
    }

    public void EquipItem(Item _item)
    {
        if (equipment.Count > 3)
        {
            equipment[activeIndex] = _item;
            abilities[activeIndex] = _item.ability;
        }
        else
        {
            equipment.Add(_item);
            abilities.Add(_item.ability);
        }

        _item.ability.StartUp();
        SetupAbilityReferences();
        if (equipment.Count == 1)
        {
            CallbackHandler.instance.SetActiveAbility(0);
        }
    }

    public void SetupAbilityReferences()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < abilities.Count)
            {
                CallbackHandler.instance.ShowAbility(i);
                CallbackHandler.instance.SetAbilityReference(i, abilities[i]);
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
            CallbackHandler.instance.ToggleRingInterface(abilities[activeIndex]);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (CharacterStats.instance.Control())
            {
                // Callback might be better for this

                switch(abilities[activeIndex].type)
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
        abilities[activeIndex].Use(transform);
    }
}
