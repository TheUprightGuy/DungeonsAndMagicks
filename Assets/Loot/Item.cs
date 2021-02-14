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

[CreateAssetMenu(fileName = "Wand", menuName = "Item/Wand", order = 1)]
public class Item : ScriptableObject
{
    public List<Ability> abilities;

    new public string name;
    public Sprite icon;
    public Rarity rarity;

    public void SetupLoot()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            // Clone original ability & setup
            abilities[i] = Instantiate(abilities[i]);
            abilities[i].sockets = Random.Range((int)rarity, (int)rarity + 2);
            abilities[i].ResetStartingModifiers();
        }
    }
}
