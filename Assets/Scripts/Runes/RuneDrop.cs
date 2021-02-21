using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrop : MonoBehaviour
{
    Mod rune;

    private void Start()
    {
        rune = Instantiate(DistributionManager.instance.RequestMod());
    }

    private void OnTriggerEnter(Collider other)
    {
        Player pc = other.GetComponent<Player>();
        if (pc)
        {
            pc.runes.Add(rune);
            MagicUI.instance.AddRune(rune);
            Destroy(this.gameObject);
            EventManager.TriggerEvent("Pick Up Rune");
        }
    }
}
