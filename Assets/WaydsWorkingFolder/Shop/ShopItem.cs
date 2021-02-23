using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Item item;

    Image image;
    public TMPro.TextMeshProUGUI itemName;
    public TMPro.TextMeshProUGUI itemPrice;

    public GameObject lootDrop;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        
    }

    public void Setup(Item _item)
    {
        item = _item;
        image.sprite = item.icon;
        itemName.SetText(item.name);
        itemPrice.SetText("$5");
    }

    public void BuyItem()
    {
        GameObject temp = Instantiate(lootDrop, Player.instance.transform.position, Quaternion.identity);
        temp.AddComponent<LootEffect>();
        Debug.Log("You buy " + item.name);
    }
}
