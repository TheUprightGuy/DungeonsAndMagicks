using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPrefab : MonoBehaviour
{
    public Image piece;
    public Image icon;
    [HideInInspector] public RingElement element;

    public void Use()
    {
        // Interface?
        Debug.Log(element.name + " clicked.");
    }
}
