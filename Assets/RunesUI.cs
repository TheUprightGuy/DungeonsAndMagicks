using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesUI : MonoBehaviour
{
    public GameObject runePrefab;

    public List<GameObject> runes;


    public void DisplayRunes(List<Mod> _runes)
    {
        foreach(Mod n in _runes)
        {
            Rune temp = Instantiate(runePrefab, this.transform).GetComponentInChildren<Rune>();
            temp.mod = n;

            runes.Add(temp.gameObject);
        }
    }

    public void CleanUp()
    {
        foreach(GameObject n in runes)
        {
            Destroy(n.gameObject);
        }
        runes.Clear();
    }
}
