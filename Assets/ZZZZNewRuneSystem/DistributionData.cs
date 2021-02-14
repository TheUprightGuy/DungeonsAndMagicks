using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistributionData", menuName = "Data/DistributionData", order = 2)]
public class DistributionData : ScriptableObject
{
    public List<Mod> mods;
    public List<Ability> abilities;
}
