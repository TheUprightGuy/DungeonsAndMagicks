using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnHitBehaviour
{
    None,
    Chain,
    Pierce,
    Explode
}

public enum OnShootBehaviour
{
    Normal = 0,
    Multiple,
    Volley,
    Line
}

[System.Serializable]
public struct ProjectileAbilityModifiers
{
    [Header("Buff & Debuffs")]
    public Buff buff;
    [Header("Projectile Modifiers")]
    public int numProj;
    [Header("Projectile Behaviours")]
    public OnShootBehaviour onShoot;
    public List<OnHitBehaviour> onHit;

    // Change this for pfx of projectile turned on
    public GameObject projPrefab;
}

[CreateAssetMenu(fileName = "ProjectileAbility", menuName = "Magic/Ability/ProjectileAbility", order = 1)]
public class ProjectileAbility : Ability
{
    public ProjectileAbilityModifiers mods;
    [HideInInspector] public ProjectileAbilityModifiers startMods;

    public GameObject projectilePrefab;

    // Projectile Arc - Math?
    int[] oddArray = { 0, -10, 10, -20, 20, -30, 30, -40, 40, -50, 50 };
    int[] evenArray = { -10, 10, -20, 20, -30, 30, -40, 40, -50, 50 };

    public override void Use(Transform _user)
    {
        switch (mods.onShoot)
        {
            case OnShootBehaviour.Normal:
            {
                Projectile temp = Instantiate((mods.projPrefab) ? mods.projPrefab : projectilePrefab, _user.transform.position + _user.transform.forward, Quaternion.identity).GetComponent<Projectile>();
                temp.transform.rotation = Quaternion.Euler(0.0f, _user.transform.rotation.eulerAngles.y, 0.0f);

                temp.Setup(mods);
                    
                break;
            }
            case OnShootBehaviour.Multiple:
            {
                int[] angleArray = (mods.numProj % 2 == 0) ? evenArray : oddArray;
                //transform.LookAt(hit.point);

                // Fan behaviour
                for (int i = 0; i < mods.numProj; i++)
                {
                    Projectile temp = Instantiate((mods.projPrefab) ? mods.projPrefab : projectilePrefab, _user.transform.position + _user.transform.forward, Quaternion.identity).GetComponent<Projectile>();
                    temp.transform.rotation = Quaternion.Euler(0.0f, _user.transform.rotation.eulerAngles.y + angleArray[i], 0.0f);

                    temp.Setup(mods);
                }
                break;
            }
            case OnShootBehaviour.Volley:
            {
                // Volley behaviour
                for (int i = 0; i < mods.numProj; i++)
                {
                    int space = Mathf.CeilToInt((float)((float)i / 2.0f));
                    // Check if left or right
                    int side = (i % 2 != 0) ? -1 * space : 1 * space;

                    Projectile temp = Instantiate((mods.projPrefab) ? mods.projPrefab : projectilePrefab, _user.transform.position + _user.transform.forward + _user.transform.right * side, Quaternion.identity).GetComponent<Projectile>();              
                    temp.transform.rotation = Quaternion.Euler(0.0f, _user.transform.rotation.eulerAngles.y, 0.0f); 

                    temp.Setup(mods);
                }
                break;
            }
            case OnShootBehaviour.Line:
            {
                StartCoroutine((Fire(_user, 0.1f)));
                break;
            }
        }
    }

    IEnumerator Fire(Transform _user, float _delay)
    {
        // Add minor delay between instantiate
        for (int i = 0; i < mods.numProj; i++)
        {
            Projectile temp = Instantiate((mods.projPrefab) ? mods.projPrefab : projectilePrefab, _user.transform.position, Quaternion.identity).GetComponent<Projectile>();
            temp.transform.rotation = Quaternion.Euler(0.0f, _user.transform.rotation.eulerAngles.y, 0.0f);
            temp.Setup(mods);
            yield return new WaitForSeconds(_delay);
        }
    }

    public override void AddMods(Mod _mods)
    {
        /*if (_mods.DisplayModifiers == ModType.None)
            return;*/


        // Number of Projectiles (Additive)
        mods.numProj += _mods.ProjectileModifiers.numProj;

        // Hit Behaviours (Additive)
        foreach (OnHitBehaviour n in _mods.ProjectileModifiers.onHit)
        {
            if (!mods.onHit.Contains(n))
            {
                mods.onHit.Add(n);
            }
        }

        // Shoot Behaviours (Override)
        if ((int)_mods.ProjectileModifiers.onShoot > (int)mods.onShoot)
        {
            mods.onShoot = _mods.ProjectileModifiers.onShoot;
        }

        // Add Buffs if Any
        if (_mods.ProjectileModifiers.buff)
        {
            buffs.Add(_mods.ProjectileModifiers.buff);
        }

        if (_mods.ProjectileModifiers.projPrefab)
        {
            mods.projPrefab = _mods.ProjectileModifiers.projPrefab;
        }
    }

    // Called on End Run / Press Q
    public override void ResetMods()
    {
        mods = startMods;
        mods.onHit.Clear();
        modsApplied.Clear();
        buffs.Clear();
        buffs = startingBuffs;
    }

    // Called on Equip
    public override void StartUp()
    {
        mods.numProj = 1;
        mods.onHit.Clear();
        mods.onShoot = OnShootBehaviour.Normal;
        startMods = mods;
        startingBuffs = buffs;
        ResetStartingModifiers();
    }

    // Called Through Startup - Sets Elements to Start Elements
    public override void ResetStartingModifiers()
    {
        modRing = ScriptableObject.CreateInstance<RingMenu>();
        modRing.ResetMods(sockets);
    }
}
