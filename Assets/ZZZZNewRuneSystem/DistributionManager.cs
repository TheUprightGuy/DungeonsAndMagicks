using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionManager : MonoBehaviour
{
    public static DistributionManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Distribution Manager exists!");
            Destroy(this.gameObject);
        }
        instance = this;
    }

    public DistributionData data;

    public Mod RequestMod()
    {
        StartCoroutine(Delay());
        return (data.mods[Random.Range(0, data.mods.Count)]);
    }

    public Ability RequestAbility()
    {
        StartCoroutine(Delay());
        return (data.abilities[Random.Range(0, data.abilities.Count)]);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.01f);
    }
}
