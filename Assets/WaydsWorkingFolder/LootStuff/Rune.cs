using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rune", menuName = "Item/Rune", order = 1)]
public class Rune : Item
{
    public Mod mod;

    public override void SetupLoot()
    {
        mod = Instantiate(DistributionManager.instance.RequestMod());
    }
}
