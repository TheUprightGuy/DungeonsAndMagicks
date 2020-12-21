using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPrefab : MonoBehaviour
{
    public Image piece;
    public Image icon;
    [HideInInspector] public RingElement element;

    public void Use(Ability _ability)
    {
        // Interface?
        Debug.Log(element.name + " clicked.");

        _ability.AddMods(element.modifiers);
    }
}
