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
    Shield,
    Reflect,
    Slow
}

[System.Serializable]
public struct BuffAbilityModifiers
{
    public OnBuffBehaviour onBuff;
    public List<OnBuffEndBehaviour> onBuffEnd;
}

[CreateAssetMenu(fileName = "BuffAbility", menuName = "Ability/BuffAbility", order = 2)]
public class BuffAbility : Ability
{
    public BuffAbilityModifiers mods;
    [HideInInspector] public BuffAbilityModifiers startMods;
    public GameObject buffPrefab;

    public override void Use(Transform _user)
    {
        if (buffPrefab)
        {
            // temp + behaviours here
            GameObject buff = Instantiate(buffPrefab, _user);
            // temp (one buff)
            buff.GetComponent<ShieldAlphaScript>().duration = buffs[0].lifeTime;
            // change for buff duration
            Destroy(buff, buffs[0].lifeTime);
        }
    }

    public override void AddMods(Mod _mods)
    {
        if (_mods.type == ModType.None)
            return;

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

    // Called on End Run / Press Q
    public override void ResetMods()
    {
        mods = startMods;
        modsApplied.Clear();
        buffs.Clear();
    }

    // Called on Equip
    public override void StartUp()
    {
        mods.onBuffEnd.Clear();
        mods.onBuff = OnBuffBehaviour.Normal;
        startMods = mods;
        buffs.Clear();
        ResetStartingModifiers();
    }

    // Called Through Startup - Sets Elements to Start Elements
    public override void ResetStartingModifiers()
    {
        modRing.ResetMods(sockets);
    }
}
