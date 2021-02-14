using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RingMenu", menuName = "RingMenu/Ring", order = 1)]
public class RingMenu : ScriptableObject
{
    public List<Mod> startMods;
    public Mod emptySocket;
    [HideInInspector] public List<Mod> mods;

    public void ResetMods(int _sockets)
    {
        mods.Clear();

        for (int i = 0; i < _sockets; i++)
        {
            if (i < startMods.Count)
            {
                mods.Add(startMods[i]);
            }
            else
            {
                mods.Add(Instantiate(emptySocket));
            }
        }
    }
}
