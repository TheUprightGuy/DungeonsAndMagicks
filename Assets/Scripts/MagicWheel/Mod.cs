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
    public Sprite icon;

    [Header("Modifiers")]
    public AbilityType DisplayModifiers;

    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Projectile)]
    public string projectileDescription;
    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Projectile)]
    public ProjectileAbilityModifiers ProjectileModifiers;

    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Buff)]
    public string BuffDescription;
    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Buff)]
    public BuffAbilityModifiers BuffModifiers;

    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Movement)]
    public string MovementDescription;
    [HideIfEnumValue("DisplayModifiers", HideIf.NotEqual, (int)AbilityType.Movement)]
    public MovementAbilityModifiers MovementModifiers;
    

    public string GetText(AbilityType _type)
    {
        switch (_type)
        {
            case AbilityType.Projectile:
            {
                return projectileDescription;
            }
            case AbilityType.Buff:
            {
                return BuffDescription;
            }
            case AbilityType.Movement:
            {
                return MovementDescription;
            }
            default:
            {
                return ("Missing Type!");
            }
        }
    }
}
