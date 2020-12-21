using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{
    private MagicCanvas magicCanvas;

    private void Start()
    {
        magicCanvas = MagicCanvas.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            magicCanvas.ToggleRingInterface();
        }
    }
}
