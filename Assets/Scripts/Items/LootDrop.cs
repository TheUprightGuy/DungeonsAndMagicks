using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Header("Item Field")]
    public Item item;

    //Local Variables
    Player pc;

    private void Start()
    {
        // Clone self and Setup
        item = Instantiate(item);
        item.SetupLoot();

        Instantiate(ResourceManager.instance.GetItemMesh(item.type), transform);
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
        transform.Rotate(Vector3.up, Time.deltaTime * 10.0f);

        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            switch (item.type)
            {
                case ItemType.Wand:
                {
                    pc.EquipItem((Weapon)item);
                    MagicUI.instance.SetupItem();
                    ItemUI.instance.HideItem(item);
                    EventManager.TriggerEvent("Pick Up Wand");
                    Destroy(this.gameObject);
                    break;
                }
                case ItemType.Rune:
                {
                    pc.AddRune(((Rune)item).mod);
                    MagicUI.instance.SetupItem();
                    EventManager.TriggerEvent("Pick Up Rune");
                    ItemUI.instance.HideItem(item);
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
    }
}
