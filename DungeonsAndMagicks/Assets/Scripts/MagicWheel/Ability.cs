using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OnHitBehaviour
{
    None,
    Chain,
    Fork,
    Explode
}

public enum OnShootBehaviour
{
    Normal = 0,
    Volley,
    Line
}

[System.Serializable]
public struct AbilityModifiers
{
    public int numProj;
    public List<OnHitBehaviour> onHit;
    public OnShootBehaviour onShoot;
}

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Ability", order = 1)]
public class Ability : ScriptableObject
{
    public GameObject projectilePrefab;
    public AbilityModifiers mods;
    private AbilityModifiers startMods;

    // temp
    public List<RingElement> modsApplied;

    public List<RingMenu> modRings;

    // Projectile Arc - Math?
    int[] oddArray = { 0, -10, 10, -20, 20, -30, 30, -40, 40, -50, 50 };
    int[] evenArray = { -10, 10, -20, 20, -30, 30, -40, 40, -50, 50 };

    public void StartUp()
    {
        mods.numProj = 1;
        mods.onHit.Clear();
        mods.onShoot = OnShootBehaviour.Normal;
        startMods = mods;
    }

    public void Use(Transform _user)
    {
        switch(mods.onShoot)
        {
            case OnShootBehaviour.Normal:
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
                break;
            }
            case OnShootBehaviour.Line:
            {
                // Add minor delay between instantiate
                for (int i = 0; i < mods.numProj; i++)
                {
                    Projectile temp = Instantiate(projectilePrefab, _user.transform.position, Quaternion.identity).GetComponent<Projectile>();
                    temp.Setup(mods);
                }
                break;
            }
        }
    }

    public void AddMods(AbilityModifiers _mods)
    {
        // Number of Projectiles (Additive)
        mods.numProj += _mods.numProj;

        // Hit Behaviours (Additive)
        foreach(OnHitBehaviour n in _mods.onHit)
        {
            if (!mods.onHit.Contains(n))
            {
                mods.onHit.Add(n);
            }
        }

        // Shoot Behaviours (Override)
        if ((int)_mods.onShoot > (int)mods.onShoot)
        {
            mods.onShoot = _mods.onShoot;
        }
    }

    public void ResetMods()
    {
        mods = startMods;
        modsApplied.Clear();
    }
}
