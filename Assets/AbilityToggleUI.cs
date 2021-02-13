using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityToggleUI : MonoBehaviour
{
    public GameObject togglePrefab;
    public List<AbilityToggle> toggles;

    public void Setup(List<Ability> _abilities)
    {
        foreach(Ability n in _abilities)
        {
            AbilityToggle temp = Instantiate(togglePrefab, this.transform).GetComponent<AbilityToggle>();
            temp.Setup(n);

            toggles.Add(temp);
        }
    }

    public void CleanUp()
    {
        foreach(AbilityToggle n in toggles)
        {
            Destroy(n.gameObject);
        }

        toggles.Clear();
    }
}
