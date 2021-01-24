using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

public enum ModType
{
    Projectile,
    Buff,
    Movement
}

[CreateAssetMenu(fileName = "Mod", menuName = "RingMenu/Mod", order = 2)]
public class Mod : ScriptableObject
{
    public ModType type;

    new public string name;
    public string description;
    public Sprite icon;
    public Element element;

    public RingMenu followUp;

    // temp
    public Buff buff;


    [HideIfEnumValue("type", HideIf.NotEqual, (int)ModType.Projectile)]
    public ProjectileAbilityModifiers projModifiers;
    [HideIfEnumValue("type", HideIf.NotEqual, (int)ModType.Buff)]
    public BuffAbilityModifiers buffModifiers;
    [HideIfEnumValue("type", HideIf.NotEqual, (int)ModType.Movement)]
    public MovementAbilityModifiers movementModifiers;
}
