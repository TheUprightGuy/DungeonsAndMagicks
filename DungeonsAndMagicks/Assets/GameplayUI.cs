using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    public List<AbilityUI> abilities;
    public int activeAbility;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeAbility = 0;
            ChooseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeAbility = 1;
            ChooseAbility();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeAbility = 2;
            ChooseAbility();
        }
    }

    public void ChooseAbility()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (i == activeAbility)
            {
                abilities[i].gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
            }
            else
            {
                abilities[i].gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
