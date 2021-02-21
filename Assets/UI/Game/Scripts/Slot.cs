using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SocketPrefab parent;

    private void Awake()
    {
        parent = GetComponentInParent<SocketPrefab>();
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
            EventManager.TriggerEvent("Socket Rune");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MagicUI.instance.ShowDetails(parent);
        parent.SetSprite(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MagicUI.instance.ShowAbility(parent);
        parent.SetSprite(false);
    }
}
