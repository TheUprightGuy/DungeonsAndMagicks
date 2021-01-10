using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    #region Setup
    Image health;
    private void Awake()
    {
        health = GetComponent<Image>();
    }
    #endregion Setup
    #region Callbacks
    void Start()
    {
        CallbackHandler.instance.updateHealth += UpdateHealth;
    }
    void OnDestroy()
    {
        CallbackHandler.instance.updateHealth -= UpdateHealth;
    }
    #endregion Callbacks

    public void UpdateHealth(float _health)
    {
        health.fillAmount = _health;
    }
}
