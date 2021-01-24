using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    #region Setup
    Image mana;
    private void Awake()
    {
        mana = GetComponent<Image>();
    }
    #endregion Setup
    #region Callbacks
    void Start()
    {
        CallbackHandler.instance.updateMana += UpdateMana;
    }
    void OnDestroy()
    {
        CallbackHandler.instance.updateMana -= UpdateMana;
    }
    #endregion Callbacks

    public void UpdateMana(float _mana)
    {
        mana.fillAmount = _mana;
    }
}
