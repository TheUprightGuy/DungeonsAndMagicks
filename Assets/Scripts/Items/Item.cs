using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common = 1,
    Uncommon,
    Rare,
    Epic
}

public enum ItemType
{
    Wand = 0,
    Rune
}

public abstract class Item : ScriptableObject
{
    new public string name;
    public Sprite icon;
    public Rarity rarity;
    public ItemType type;

    public abstract void SetupLoot();
}
