using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    private MagicCanvas magicCanvas;

    public Ability ability;

    private void Start()
    {
        magicCanvas = MagicCanvas.instance;
        ability.StartUp();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            magicCanvas.ToggleRingInterface();
            magicCanvas.SetActiveAbility(ability);        
        }

        if (Input.GetMouseButtonDown(1))
        {
            ability.Use(transform);
        }
    }
}
