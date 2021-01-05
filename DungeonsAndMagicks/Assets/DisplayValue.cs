using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayValue : MonoBehaviour
{
    #region Setup
    TMPro.TextMeshProUGUI text;
    Slider slider;
    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        slider = GetComponentInChildren<Slider>();
    }
    private void Start()
    {
        SetText();
    }
    #endregion Setup

    public void SetText()
    {
        if (text && slider)
        {
            text.SetText(slider.value.ToString());
        }
    }
}
