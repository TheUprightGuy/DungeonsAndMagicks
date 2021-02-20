using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityToggle : MonoBehaviour
{
    public Ability ability;
    public Image icon;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void Setup(Ability _ability)
    {
        ability = _ability;
        icon.sprite = _ability.icon;
    }

    public void ToggleAbility()
    {
        if (CraftingCanvas.instance.ringInterface.gameObject.activeSelf)
        {
            CraftingCanvas.instance.SetRingAbility(ability);
            CraftingCanvas.instance.DisplayRunes();
        }
        if (MagicCanvas.instance.ringInterface.gameObject.activeSelf)
        {
            MagicCanvas.instance.SetRingAbility(ability);
        }
    }
}
