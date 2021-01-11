using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuffAbilityModifiers
{
    public int numProj;
    public List<OnHitBehaviour> onHit;
    public OnShootBehaviour onShoot;
}

[CreateAssetMenu(fileName = "BuffAbility", menuName = "Ability/BuffAbility", order = 2)]
public class BuffAbility : Ability
{
    public BuffAbilityModifiers mods;
    [HideInInspector] public BuffAbilityModifiers startMods;

    public override void Use(Transform _user)
    {

    }

    public override void AddMods(RingElement _mods)
    {

    }

    public override void ResetMods()
    {
        /*mods = startMods;
        modsApplied.Clear();*/
    }
    public override void StartUp()
    {
       /* mods.numProj = 1;
        mods.onHit.Clear();
        mods.onShoot = OnShootBehaviour.Normal;
        startMods = mods;*/
    }
}
