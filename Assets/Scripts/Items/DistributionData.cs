using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistributionData", menuName = "Magic/Data/DistributionData", order = 3)]
public class DistributionData : ScriptableObject
{
    public List<Mod> mods;
    public List<Ability> abilities;
}
