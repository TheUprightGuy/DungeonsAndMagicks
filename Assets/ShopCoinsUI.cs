using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCoinsUI : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Start()
    {
        CallbackHandler.instance.updateShopText += SetText;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.updateShopText -= SetText;
    }

    private void OnEnable()
    {
        SetText();
    }

    public void SetText()
    {
        if (CallbackHandler.instance)
        text.SetText(CallbackHandler.instance.GetPlayerCoins().ToString());
    }
}
