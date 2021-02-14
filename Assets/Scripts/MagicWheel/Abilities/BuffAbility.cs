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
    [Header("Buff & Debuffs")]
    public Buff buff;
    [Header("Buff Behaviours")]
    public OnBuffBehaviour onBuff;
    public List<OnBuffEndBehaviour> onBuffEnd;
}

[CreateAssetMenu(fileName = "BuffAbility", menuName = "Magic/Ability/BuffAbility", order = 2)]
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

    public override void AddMods(Mod _mod)
    {
        /*if (_mods.DisplayModifiers == ModType.None)
            return;*/

        // BuffEnd Behaviours (Additive)
        foreach (OnBuffEndBehaviour n in _mod.BuffModifiers.onBuffEnd)
        {
            if (!mods.onBuffEnd.Contains(n))
            {
                mods.onBuffEnd.Add(n);
            }
        }

        // Buff Behaviours (Override)
        if ((int)_mod.BuffModifiers.onBuff > (int)mods.onBuff)
        {
            mods.onBuff = _mod.BuffModifiers.onBuff;
        }

        // Add Buffs if Any
        if (_mod.BuffModifiers.buff)
        {
            buffs.Add(_mod.BuffModifiers.buff);
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
        modRing = ScriptableObject.CreateInstance<RingMenu>();
        modRing.ResetMods(sockets);
    }
}
