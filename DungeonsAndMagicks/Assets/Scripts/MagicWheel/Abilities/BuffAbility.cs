using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnBuffEndBehaviour
{
    None,
}

public enum OnBuffBehaviour
{
    Normal = 0,
    Blink,
    Dash,
    Sprint
}

[System.Serializable]
public struct BuffAbilityModifiers
{
    public int numProj;
    public OnBuffBehaviour onBuff;
    public List<OnBuffEndBehaviour> onBuffEnd;
}

[CreateAssetMenu(fileName = "BuffAbility", menuName = "Ability/BuffAbility", order = 2)]
public class BuffAbility : Ability
{
    public BuffAbilityModifiers mods;
    [HideInInspector] public BuffAbilityModifiers startMods;

    public override void Use(Transform _user)
    {

    }

    public override void AddMods(Mod _mods)
    {

    }

    public override void ResetMods()
    {
        mods = startMods;
        modsApplied.Clear();
    }
    public override void StartUp()
    {
        mods.onBuffEnd.Clear();
        mods.onBuff = OnBuffBehaviour.Normal;
        startMods = mods;
    }
}
