using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon", order = 1)]
public class Weapon : Item
{
    public List<Ability> abilities;
    public override void SetupLoot()
    {
        for (int i = 0; i < 3; i++)
        {
            abilities.Add(Instantiate(DistributionManager.instance.RequestAbility()));
            abilities[i].sockets = Random.Range((int)rarity, (int)rarity + 2);
            abilities[i].ResetStartingModifiers();
        }
    }
}
