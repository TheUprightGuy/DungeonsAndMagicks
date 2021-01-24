using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    public Buff buff;
    public TMPro.TextMeshProUGUI text;
    public Image image;
    public Image cooldown;

    private void Awake()
    {
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }
    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.removeBuffFromUI += RemoveBuffFromUI;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.removeBuffFromUI -= RemoveBuffFromUI;
    }
    #endregion Callbacks

    public void Setup(Buff _buff)
    {
        buff = _buff;
        image.sprite = _buff.icon;
    }

    public void RemoveBuffFromUI(Buff _buff)
    {
        if (buff == _buff)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        text.SetText(Mathf.RoundToInt(buff.life).ToString());
        cooldown.fillAmount = buff.life / buff.lifeTime;
    }
}
