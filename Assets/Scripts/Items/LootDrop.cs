using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public Item item;
    public Transform rot;
    Controller pc;
    
    private void Start()
    {
        // Clone self and Setup
        item = Instantiate(item);
        item.SetupLoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterStats>())
        {
            ItemUI.instance.ShowItem(item);
            pc = other.GetComponent<Controller>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterStats>())
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
            ItemUI.instance.HideItem(item);
            Destroy(this.gameObject);

            //TutorialTracking.instance.CheckQuest(this.gameObject);
        }
    }
}
