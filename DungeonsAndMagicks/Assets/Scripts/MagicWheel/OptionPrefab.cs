using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPrefab : MonoBehaviour
{
    public Image piece;
    public Image icon;
    [HideInInspector] public RingElement element;
    public bool selected;
    public Color notSelectedColor;
    public Color selectedColor;
    [HideInInspector] public RingInterface parent;

    private void OnEnable()
    {
        selected = false;
        icon.color = notSelectedColor;
    }

    public void Use(Ability _ability)
    {
        // Interface?
        Debug.Log(element.name + " clicked.");

        if (!selected)
        {
            _ability.AddMods(element.modifiers);
            icon.color = selectedColor;
            GameObject mod = Instantiate(parent.modPrefab);
            //parent.modList
            selected = true;
        }
    }
}
