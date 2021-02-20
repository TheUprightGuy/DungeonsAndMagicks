using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrop : MonoBehaviour
{
    public Mod rune;

    private void Start()
    {
        rune = Instantiate(DistributionManager.instance.RequestMod());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Controller>())
        {
            other.GetComponent<Controller>().runes.Add(rune);
            Destroy(this.gameObject);
        }
    }
}
