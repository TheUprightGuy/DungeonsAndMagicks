using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCAnimatorFunctions : MonoBehaviour
{
    [HideInInspector] public Controller controller;

    // Callback is probably better
    public void Cast()
    {
        controller.Cast();
    }

    public void RemoveControl()
    {
        CharacterStats.instance.SetControl(false);
    }

    public void GiveControl()
    {
        CharacterStats.instance.SetControl(true);
    }
}
