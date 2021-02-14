using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu : ScriptableObject
{
    [HideInInspector] public List<Mod> mods;

    public void ResetMods(int _sockets)
    {
        mods = new List<Mod>();

        mods.Clear();

        for (int i = 0; i < _sockets; i++)
        {
            mods.Add(Instantiate(DistributionManager.instance.emptySocket));         
        }
    }
}
