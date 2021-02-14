using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnEndBehaviour
{
    None,
    Evasion,
    Phasing
}

public enum OnMoveBehaviour
{
    Normal = 0,
    Blink,
    Sprint
}

[System.Serializable]
public struct MovementAbilityModifiers
{
    public OnMoveBehaviour onMove;
    public List<OnEndBehaviour> onEnd;
}

[CreateAssetMenu(fileName = "MovementAbility", menuName = "Magic/Ability/MovementAbility", order = 1)]
public class MovementAbility : Ability
{
    public MovementAbilityModifiers mods;
    [HideInInspector] public MovementAbilityModifiers startMods;
   
    public override void Use(Transform _user)
    {
        switch (mods.onMove)
        {
            case OnMoveBehaviour.Normal:
            {
                // Do Nothing?
                break;
            }
            case OnMoveBehaviour.Blink:
            {
                // Teleport - temp
                Vector3 pos = CharacterStats.instance.mousePos.MouseToWorldPos;
                _user.position = new Vector3(pos.x, pos.y + _user.GetComponent<MeshFilter>().mesh.bounds.extents.y, pos.z);
                foreach (Buff n in buffs)
                {
                    Buff temp = Instantiate(n);
                    n.StartUp();
                    CharacterStats.instance.AddBuff(temp);
                }
                break;
            }
            case OnMoveBehaviour.Sprint:
            {
                // figuring out buff system
                foreach (Buff n in buffs)
                {
                    Buff temp = Instantiate(n);
                    n.StartUp();
                    CharacterStats.instance.AddBuff(temp);
                }
                break;
            }
        }
    }


    public override void AddMods(Mod _mods)
    {
        if (_mods.type == ModType.None)
            return;

        // Hit Behaviours (Additive)
        foreach (OnEndBehaviour n in mods.onEnd)
        {
            if (!mods.onEnd.Contains(n))
            {
                mods.onEnd.Add(n);
            }
        }

        // Move Behaviours (Override)
        if ((int)_mods.movementModifiers.onMove > (int)mods.onMove)
        {
            mods.onMove = _mods.movementModifiers.onMove;
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
        mods.onEnd.Clear();
        mods.onMove = OnMoveBehaviour.Normal;
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
