using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketPrefab : MonoBehaviour
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

    public void Use()
    {
        if (!selected && mod.name != "Empty")
        {
            parent.SelectMod(mod);

            selected = true;
        }
    }

    public void SetMod(Mod _mod)
    {
        mod = _mod;
        icon.sprite = mod.icon;

        parent.ability.modRing.mods[id] = _mod;
    }

    public void SetSprite(bool _active)
    {
        backPlate.sprite = _active ? selectedSprite : notSelectedSprite;
    }
}