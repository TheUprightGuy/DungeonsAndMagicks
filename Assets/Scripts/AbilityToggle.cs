using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityToggle : MonoBehaviour
{
    public Ability ability;
    public Image icon;


    public void Setup(Ability _ability)
    {
        ability = _ability;
        // set icon
    }

    public void ToggleAbility()
    {
        CraftingCanvas.instance.SetRingAbility(ability);
        CraftingCanvas.instance.SetRunes(ability);
    }
}
