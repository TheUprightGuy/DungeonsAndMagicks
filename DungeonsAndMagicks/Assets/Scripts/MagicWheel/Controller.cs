using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    public List<Ability> abilities;
    [HideInInspector] public int activeIndex = 0;

    private void Start()
    {
        Invoke("SetupAbilityReferences", 0.1f);
        // This needs to go
        abilities[0].StartUp();
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CallbackHandler.instance.ToggleRingInterface(abilities[activeIndex]);
        }

        if (Input.GetMouseButtonDown(1))
        {
            abilities[activeIndex].Use(transform);
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
}
