using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Movement = 0,
    Phasing,
    Dodge,
    Damage,
    Shield
}

[System.Serializable]
public struct MovementBuffModifiers
{
    // temp
    public float moveIncrease;
}
[System.Serializable]
public struct DamageBuffModifiers
{
    // temp
    public bool damage;
}
[System.Serializable]
public struct ShieldBuffModifiers
{
    // temp
    public bool shield;
}

[CreateAssetMenu(fileName = "Buff", menuName = "Buff/Buff", order = 1)]
public class Buff : ScriptableObject
{
    public Sprite icon;
    new public string name;

    public BuffType buffType;
    public float lifeTime;
    public float life;

    public void StartUp()
    {
        life = lifeTime;
    }

    public bool UpdateCount(float _deltaTime)
    {
        life -= _deltaTime;
        return (life <= 0);
    }

    [HideIfEnumValue("buffType", HideIf.NotEqual, (int)BuffType.Movement)]
    public MovementBuffModifiers movementModifiers;
    [HideIfEnumValue("buffType", HideIf.NotEqual, (int)BuffType.Damage)]
    public DamageBuffModifiers damageModifiers;
    [HideIfEnumValue("buffType", HideIf.NotEqual, (int)BuffType.Shield)]
    public ShieldBuffModifiers shieldModifiers;
}
