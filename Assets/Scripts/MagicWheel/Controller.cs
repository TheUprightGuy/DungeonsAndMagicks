using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
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
        // This needs to go
        abilities[0].StartUp();
        abilities[1].StartUp();
        abilities[2].StartUp();

        Invoke("SetupStuff", 0.1f);
    }

    public void SetupStuff()
    {
        CallbackHandler.instance.SetCastSpeed(castspeed);
        activeIndex = 0;
        CallbackHandler.instance.SetActiveAbility(0);
    }

    private void OnApplicationQuit()
    {
        // required
        abilities[0].ResetMods();
        abilities[1].ResetMods();
        abilities[2].ResetMods();
    }

    public void SetupAbilityReferences()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            CallbackHandler.instance.SetAbilityReference(i, abilities[i]);
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
