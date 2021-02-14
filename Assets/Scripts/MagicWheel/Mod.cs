using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Mod", menuName = "Magic/Mod/Mod", order = 1)]
public class Mod : ScriptableObject
{
    [Header("Mod Details")]
    new public string name;
    public string description;
    public Sprite icon;

    [Header("Modifiers")]
    public AbilityType DisplayModifiers;    
    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Projectile)]
    public ProjectileAbilityModifiers ProjectileModifiers;
    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Buff)]
    public BuffAbilityModifiers BuffModifiers;
    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Movement)]
    public MovementAbilityModifiers MovementModifiers;   
}
