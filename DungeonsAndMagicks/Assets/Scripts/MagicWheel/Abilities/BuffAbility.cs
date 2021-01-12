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
        // behaviours here
    }

    public override void AddMods(Mod _mods)
    {
        // BuffEnd Behaviours (Additive)
        foreach (OnBuffEndBehaviour n in mods.onBuffEnd)
        {
            if (!mods.onBuffEnd.Contains(n))
            {
                mods.onBuffEnd.Add(n);
            }
        }

        // Buff Behaviours (Override)
        if ((int)_mods.buffModifiers.onBuff > (int)mods.onBuff)
        {
            mods.onBuff = _mods.buffModifiers.onBuff;
        }

        // Add Buffs if Any
        if (_mods.buff)
        {
            buffs.Add(_mods.buff);
        }
    }

    public override void ResetMods()
    {
        mods = startMods;
        modsApplied.Clear();
        modRings.Clear();
        foreach (RingMenu n in startRings)
        {
            modRings.Add(n);
        }
        buffs.Clear();
    }

    public override void StartUp()
    {
        mods.onBuffEnd.Clear();
        mods.onBuff = OnBuffBehaviour.Normal;
        startMods = mods;
        startRings.Clear();
        foreach (RingMenu n in modRings)
        {
            startRings.Add(n);
        }
        buffs.Clear();
    }
}
