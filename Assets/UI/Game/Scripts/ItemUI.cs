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
    public GameObject[] abilities;
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
            image.sprite = items[0].icon;

            for (int i = 0; i < items[0].abilities.Count; i++)
            {
                abilities[i].SetActive(true);
                abilities[i].GetComponent<Image>().sprite = items[0].abilities[i].icon;
                abilities[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(items[0].abilities[i].sockets.ToString());
            }
        }
        else
        {
            container.SetActive(false);
        }
    }
}
