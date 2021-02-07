using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rune : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Mod mod;

    public static GameObject dragObject;
    Vector3 startPosition;
    public Transform startParent;

    private void Start()
    {
        GetComponent<Image>().sprite = mod.icon;
    }

    public void OnBeginDrag(PointerEventData eventdata)
    {
        dragObject = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventdata)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventdata)
    {
        dragObject = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }

}
