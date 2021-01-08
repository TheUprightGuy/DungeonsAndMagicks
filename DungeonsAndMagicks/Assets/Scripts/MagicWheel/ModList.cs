using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModList : MonoBehaviour
{
    public List<GameObject> modIcons;

    public void CreateMod(Sprite _icon, GameObject _n)
    {
        GameObject mod = Instantiate(_n, this.transform);
        mod.GetComponent<Image>().sprite = _icon;
        modIcons.Add(mod);
    }

    public void ClearIcons()
    {
        foreach(GameObject n in modIcons)
        {
            Destroy(n);
        }
        modIcons.Clear();
    }
}
