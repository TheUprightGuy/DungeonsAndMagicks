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
    public int numProj;
    public OnShootBehaviour onShoot;
    public List<OnHitBehaviour> onHit;

    public GameObject projPrefab;
}

[CreateAssetMenu(fileName = "ProjectileAbility", menuName = "Ability/ProjectileAbility", order = 1)]
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
                Projectile temp = Instantiate(projectilePrefab, _user.transform.position + _user.transform.forward, Quaternion.identity).GetComponent<Projectile>();
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
                    Projectile temp = Instantiate(projectilePrefab, _user.transform.position + _user.transform.forward, Quaternion.identity).GetComponent<Projectile>();
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

                    Projectile temp = Instantiate(projectilePrefab, _user.transform.position + _user.transform.forward + _user.transform.right * side, Quaternion.identity).GetComponent<Projectile>();              
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
            Projectile temp = Instantiate(projectilePrefab, _user.transform.position, Quaternion.identity).GetComponent<Projectile>();
            temp.transform.rotation = Quaternion.Euler(0.0f, _user.transform.rotation.eulerAngles.y, 0.0f);
            temp.Setup(mods);
            yield return new WaitForSeconds(_delay);
        }
    }

    public override void AddMods(Mod _mods)
    {
        if (_mods.type == ModType.None)
            return;


        // Number of Projectiles (Additive)
        mods.numProj += _mods.projModifiers.numProj;

        // Hit Behaviours (Additive)
        foreach (OnHitBehaviour n in _mods.projModifiers.onHit)
        {
            if (!mods.onHit.Contains(n))
            {
                mods.onHit.Add(n);
            }
        }

        // Shoot Behaviours (Override)
        if ((int)_mods.projModifiers.onShoot > (int)mods.onShoot)
        {
            mods.onShoot = _mods.projModifiers.onShoot;
        }

        // Add Buffs if Any
        if (_mods.buff)
        {
            buffs.Add(_mods.buff);
        }

        if (_mods.projModifiers.projPrefab)
        {
            projectilePrefab = _mods.projModifiers.projPrefab;
        }
    }

    public override void ResetMods()
    {
        mods = startMods;
        mods.onHit.Clear();
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
        mods.numProj = 1;
        mods.onHit.Clear();
        mods.onShoot = OnShootBehaviour.Normal;
        startMods = mods;
        startRings.Clear();
        foreach (RingMenu n in modRings)
        {
            startRings.Add(n);
        }
        buffs.Clear();
        ResetRings();
    }

    public override void ResetRings()
    {
        foreach(RingMenu n in modRings)
        {
            n.ResetMods();
        }
    }
}
