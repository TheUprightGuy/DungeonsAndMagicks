using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    Player pc;
    ShopTextBillboard shopText;
    public string greeting;

    private void Awake()
    {
        shopText = GetComponentInChildren<ShopTextBillboard>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            // Open Shop Menu
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            pc = other.GetComponent<Player>();
            shopText.SetText(greeting, 3.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            pc = null;
        }
    }
}
