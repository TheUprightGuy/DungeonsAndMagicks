using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    Player pc;
    ShopTextBillboard shopText;
    ShopUI shopUI;
    GameObject shop;

    public string greeting;

    public List<Item> shopInventory;

    private void Awake()
    {
        shop = GetComponentInChildren<Canvas>().gameObject;
        shopText = GetComponentInChildren<ShopTextBillboard>();
        shopUI = GetComponentInChildren<ShopUI>();
    }

    private void Start()
    {
        shopUI.Setup(shopInventory);
        shop.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pc)
        {
            // Open Shop Menu
            shop.SetActive(!shop.activeSelf);
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
