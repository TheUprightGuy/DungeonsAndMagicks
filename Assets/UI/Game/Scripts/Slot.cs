using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public OptionPrefab parent;

    private void Awake()
    {
        parent = GetComponentInParent<OptionPrefab>();
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
        parent.parent.ShowDetails(parent);
        parent.SetSprite(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        parent.parent.ShowAbility();
        parent.SetSprite(false);
    }
}
