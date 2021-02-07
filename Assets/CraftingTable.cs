using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    public CraftingCanvas cc;
    private void Start()
    {
        cc = CraftingCanvas.instance;
    }


    private void OnTriggerEnter(Collider other)
    {
        cc.pc = other.GetComponent<Controller>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Controller>())
        {
            cc.pc = null;
        }
    }
}
