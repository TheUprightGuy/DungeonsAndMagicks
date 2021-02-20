using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public NEWOptionPrefab parent;

    private void Awake()
    {
        parent = GetComponentInParent<NEWOptionPrefab>();
    }

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return (transform.GetChild(0).gameObject);
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventdata)
    {
        if (!item)
        {
            Destroy(Rune.dragObject);
            parent.SetMod(Rune.dragObject.GetComponent<Rune>().mod);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        NEWMagicCanvas.instance.ShowDetails(parent);
        //parent.parent.ShowDetails(parent);
        parent.SetSprite(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        NEWMagicCanvas.instance.ShowAbility(parent);
        //parent.parent.ShowAbility();
        parent.SetSprite(false);
    }
}
