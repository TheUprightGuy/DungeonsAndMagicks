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

            if (items[0] as Weapon)
            {
                for (int i = 0; i < ((Weapon)items[0]).abilities.Count; i++)
                {
                    abilities[i].SetActive(true);
                    abilities[i].GetComponent<Image>().sprite = ((Weapon)items[0]).abilities[i].icon;
                    abilities[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(((Weapon)items[0]).abilities[i].sockets.ToString());
                }
            }
            else if (items[0] as Rune)
            {
                // Do Stuff
                for (int i = 0; i < abilities.Length; i++)
                {
                    abilities[i].SetActive(false);
                }
            }
        }
        else
        {
            container.SetActive(false);
        }
    }
}
