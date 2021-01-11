using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModType
{
    Projectile,
    Buff,
    Movement
}

[CreateAssetMenu(fileName = "RingElement", menuName = "RingMenu/Element", order = 2)]
public class RingElement : ScriptableObject
{
    public ModType type;

    new public string name;
    public string description;
    public Sprite icon;


    [HideIfEnumValue("type", HideIf.NotEqual, (int)ModType.Projectile)]
    public ProjectileAbilityModifiers projModifiers;
    [HideIfEnumValue("type", HideIf.NotEqual, (int)ModType.Buff)]
    public BuffAbilityModifiers buffModifiers;
    [HideIfEnumValue("type", HideIf.NotEqual, (int)ModType.Movement)]
    public MovementAbilityModifiers movementModifiers;
}
