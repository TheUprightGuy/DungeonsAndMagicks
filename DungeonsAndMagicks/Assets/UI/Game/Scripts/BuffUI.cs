using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public GameObject buffPrefab;
    public List<BuffIcon> buffs;

    private void Start()
    {
        CallbackHandler.instance.addBuffToUI += AddBuffToUI;
    }
    private void OnDestroy()
    {
        CallbackHandler.instance.addBuffToUI -= AddBuffToUI;
    }

    public void AddBuffToUI(Buff _buff)
    {
        BuffIcon mod = Instantiate(buffPrefab, this.transform).GetComponent<BuffIcon>();
        mod.Setup(_buff);
    }
}
