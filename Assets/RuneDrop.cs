using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrop : MonoBehaviour
{
    public Mod rune;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Controller>())
        {
            other.GetComponent<Controller>().runes.Add(rune);
            TutorialTracking.instance.CheckQuest(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}