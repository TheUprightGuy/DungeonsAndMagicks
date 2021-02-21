using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Header("Item Field")]
    public Item item;

    [Header("Required Fields")]
    public Transform rot;
    Player pc;
    
    private void Start()
    {
        // Clone self and Setup
        item = Instantiate(item);
        item.SetupLoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        pc = other.GetComponent<Player>();
        if (pc)
        {
            ItemUI.instance.ShowItem(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            ItemUI.instance.HideItem(item);
            pc = null;
        }
    }

    private void Update()
    {
        rot.Rotate(Vector3.up, Time.deltaTime * 10.0f);

        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            pc.EquipItem(item);
            MagicUI.instance.SetupItem();
            ItemUI.instance.HideItem(item);
            Destroy(this.gameObject);
            EventManager.TriggerEvent("Pick Up Wand");
        }
    }
}
