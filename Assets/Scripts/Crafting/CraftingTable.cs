using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // temp
        if (other.GetComponent<Controller>())
        {
            NEWMagicCanvas.instance.crafting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // temp
        if (other.GetComponent<Controller>())
        {
            NEWMagicCanvas.instance.crafting = false;          
        }
    }
}
