using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{ 
    GameObject displayGroup;
    public GameObject itemPrefab;

    private void Awake()
    {
        displayGroup = GetComponentInChildren<GridLayoutGroup>().gameObject;
    }

    public void Setup(List<Item> _items)
    {
        foreach (Item n in _items)
        {
            Instantiate(itemPrefab, displayGroup.transform).GetComponent<ShopItem>().Setup(n);
        }
    }
}
