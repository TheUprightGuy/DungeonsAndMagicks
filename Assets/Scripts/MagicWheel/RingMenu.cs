using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RingMenu", menuName = "RingMenu/Ring", order = 1)]
public class RingMenu : ScriptableObject
{
    public List<Mod> startElements;
    [HideInInspector] public List<Mod> elements;

    public void ResetMods()
    {
        elements.Clear();

        foreach(Mod n in startElements)
        {
            elements.Add(n);
        }
    }
}
