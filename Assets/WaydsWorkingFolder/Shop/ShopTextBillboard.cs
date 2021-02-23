using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTextBillboard : MonoBehaviour
{
    public TMPro.TextMeshPro text;
    Coroutine myCoroutine = null;

    private void Awake()
    {
        text = GetComponent<TMPro.TextMeshPro>();
    }

    private void Start()
    {
        //CallbackHandler.instance.setDialogueText += SetText;
    }

    private void OnDestroy()
    {
        //CallbackHandler.instance.setDialogueText -= SetText;
    }

    public void SetText(string _text, float _time)
    {
        text.SetText(_text);
        // Cancels previous Dialogue
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
        }
        // Start new Dialogue
        myCoroutine = StartCoroutine(DisableAfter(_time));
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
