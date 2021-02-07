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

    [HideInInspector] public Mod element;
    [HideInInspector] public bool selected;
    [HideInInspector] public RingInterface parent;

    private void OnEnable()
    {
        selected = false;
    }

    public void Use(Ability _ability)
    {
        if (!selected && element.type != ModType.None)
        {
            _ability.modsApplied.Add(element);
            _ability.AddMods(element);
            parent.modList.CreateMod(icon.sprite, parent.modPrefab);


            if (element.followUp)
            {
                parent.data.Add(element.followUp);
            }


            selected = true;
        }
        CallbackHandler.instance.UpdateAbilityModList(_ability);
        MagicCanvas.instance.NextRing();
    }
}
