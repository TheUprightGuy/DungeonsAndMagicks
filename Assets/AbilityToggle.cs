using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityToggle : MonoBehaviour
{
    Ability ability;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>(); 
    }

    // Setups toggle reference
    public void Setup(Ability _ability)
    {
        ability = _ability;
        image.sprite = _ability.icon;
    }

    public void ToggleAbility()
    {
        MagicUI.instance.ToggleAbility(ability);
    }
}
