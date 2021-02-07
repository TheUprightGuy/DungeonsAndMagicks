using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public Item item;
    public Transform rot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterStats>())
        {
            ItemUI.instance.ShowItem(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterStats>())
        {
            ItemUI.instance.HideItem(item);
        }
    }

    private void Update()
    {
        rot.Rotate(Vector3.up, Time.deltaTime * 10.0f);
    }
}
