using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
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
            //Rune.dragObject.transform.SetParent(transform);
            parent.SetMod(Rune.dragObject.GetComponent<Rune>().mod);
            //parent.element = Rune.dragObject.GetComponent<Rune>().mod;

            // temp
            TutorialTracking.instance.CheckQuest(parent.element);
        }
    }
}
