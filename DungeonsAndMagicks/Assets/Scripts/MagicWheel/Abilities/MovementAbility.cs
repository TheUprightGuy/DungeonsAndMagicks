using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MovementAbilityModifiers
{
    public int numProj;
    public List<OnHitBehaviour> onHit;
    public OnShootBehaviour onShoot;
}

[CreateAssetMenu(fileName = "MovementAbility", menuName = "Ability/MovementAbility", order = 1)]
public class MovementAbility : Ability
{
    public MovementAbilityModifiers mods;
    [HideInInspector] public MovementAbilityModifiers startMods;

    public override void Use(Transform _user)
    {

    }

    public override void AddMods(RingElement _mods)
    {
        // Number of Projectiles (Additive)
        //mods.numProj += _mods.movementModifiers.numProj;

        // Hit Behaviours (Additive)
        /*foreach (OnHitBehaviour n in mods.onHit)
        {
            if (!mods.onHit.Contains(n))
            {
                mods.onHit.Add(n);
            }
        }*/

        // Shoot Behaviours (Override)
        /*if ((int)_mods.projModifiers.onShoot > (int)mods.onShoot)
        {
            mods.onShoot = _mods.projModifiers.onShoot;
        */
    }
    public override void ResetMods()
    {
       /* mods = startMods;
        modsApplied.Clear();*/
    }
    public override void StartUp()
    {
        /*mods.numProj = 1;
        mods.onHit.Clear();
        mods.onShoot = OnShootBehaviour.Normal;
        startMods = mods;*/
    }
}
