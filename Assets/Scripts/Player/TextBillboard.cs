using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBillboard : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshPro>();
    }

    private void Start()
    {
        CallbackHandler.instance.setText += SetText;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.setText -= SetText;
    }

    public void SetText(string _text, float _time)
    {
        text.SetText(_text);
        StartCoroutine(DisableAfter(_time));
    }

    IEnumerator DisableAfter(float _time)
    {
        yield return new WaitForSeconds(_time);

        text.SetText("");
    }

    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;

        transform.LookAt(camPos);
    }
}
