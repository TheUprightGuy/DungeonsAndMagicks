using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingElement", menuName = "RingMenu/Element", order = 2)]
public class RingElement : ScriptableObject
{
    new public string name;
    public string description;
    public Sprite icon;

    public AbilityModifiers modifiers;
}
