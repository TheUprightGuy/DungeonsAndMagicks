using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPrefab : MonoBehaviour
{
    [Header("Setup Fields")]
    public Image piece;
    public Image icon;
    public Image backPlate;
    [Header("Backplate Sprites")]
    public Sprite notSelectedSprite;
    public Sprite selectedSprite;

    [HideInInspector] public RingElement element;
    [HideInInspector] public bool selected;
    [HideInInspector] public RingInterface parent;

    private void OnEnable()
    {
        selected = false;
    }

    public void Use(Ability _ability)
    {
        // Interface?
        Debug.Log(element.name + " clicked.");

        if (!selected)
        {
            _ability.AddMods(element.modifiers);
            parent.modList.CreateMod(icon, parent.modPrefab);

            selected = true;
        }

        MagicCanvas.instance.NextRing();
    }
}
