using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public static ItemUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }


    public TMPro.TextMeshProUGUI itemName;
    public Image image;
    public GameObject[] rings;
    public GameObject container;

    public List<Item> items;

    private void Start()
    {
        container.SetActive(false);
    }

    public void ShowItem(Item _item)
    {
        items.Add(_item);
        DisplayItem();
        
    }

    public void HideItem(Item _item)
    {
        items.Remove(_item);
        DisplayItem();      
    }

    public void DisplayItem()
    {
        if (items.Count > 0)
        {
            container.SetActive(true);
            itemName.SetText(items[0].name);
            image.sprite = items[0].sprite;

            for (int i = 0; i < rings.Length; i++)
            {
                rings[i].SetActive(i < items[0].numRings);
            }
        }
        else
        {
            // hide
            container.SetActive(false);
        }
    }
}
