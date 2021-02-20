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

    [HideInInspector] public Mod mod;
    [HideInInspector] public bool selected;
    [HideInInspector] public RingInterface parent;
    public int id;

    private void OnEnable()
    {
        selected = false;
    }

    /*public void Use(Ability _ability)
    {
        if (!selected)// && mod.DisplayModifiers != ModType.None)
        {
            _ability.modsApplied.Add(mod);
            _ability.AddMods(mod);
            parent.modList.CreateMod(icon.sprite, parent.modPrefab);

            selected = true;
        }
        CallbackHandler.instance.UpdateAbilityModList(_ability);
    }*/

    public void Use()
    {
        if (!selected)// && mod.DisplayModifiers != ModType.None)
        {
            parent.activeAbility.modsApplied.Add(mod);
            parent.activeAbility.AddMods(mod);
            parent.modList.CreateMod(icon.sprite, parent.modPrefab);

            selected = true;
        }
        CallbackHandler.instance.UpdateAbilityModList(parent.activeAbility);
    }

    public void SetMod(Mod _mod)
    {
        mod = _mod;
        icon.sprite = mod.icon;

        parent.data.mods[id] = _mod;
    }
}
