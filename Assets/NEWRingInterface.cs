using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NEWRingInterface : MonoBehaviour
{
    [Header("Required Setup")]
    // Ability Reference
    public Ability ability;
    // Socket Prefab
    public NEWOptionPrefab optionPrefab;
    public GameObject modPrefab;
    public ModList modList;

    // Display References
    [Header("Mod Display")]
    public TMPro.TextMeshProUGUI elementName;
    public TMPro.TextMeshProUGUI elementDescription;
    public Image elementIcon;
    // Spacing
    private float gapWidthDegree = 1.0f;


    // Creates the ability ring & spaces out options
    public void CreateRing(Ability _ability)
    {
        ability = _ability;
        // Creates OptionPrefab based on ability

        // Position Data
        float stepLength = 360.0f / ability.modRing.mods.Count;
        float iconDist = Vector3.Distance(optionPrefab.icon.transform.position, optionPrefab.piece.transform.position);
        List<NEWOptionPrefab> pieces = new List<NEWOptionPrefab>();

        // Iterate through Menu Elements and Instantiate Prefabs w/ Attributes
        for (int i = 0; i < ability.modRing.mods.Count; i++)
        {
            // Instantiate Prefab
            pieces.Add(Instantiate(optionPrefab, transform));
            pieces[i].mod = ability.modRing.mods[i];
            // Set Root Element
            pieces[i].transform.localPosition = Vector3.zero;
            pieces[i].transform.localRotation = Quaternion.identity;
            // Set Piece Position & Fill
            pieces[i].piece.fillAmount = 1f / ability.modRing.mods.Count - gapWidthDegree / 360f;
            pieces[i].piece.transform.localPosition = Vector3.zero;
            pieces[i].piece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + gapWidthDegree / 2f + i * stepLength);
            // Set Icon Position & Sprite
            pieces[i].icon.transform.localPosition = pieces[i].piece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            pieces[i].backPlate.transform.localPosition = pieces[i].piece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            pieces[i].icon.sprite = ability.modRing.mods[i].icon;
            // Set Parent
            pieces[i].parent = this;
            pieces[i].id = i;
        }
    }

    // Mod Selection
    public void SelectMod(Mod _mod)
    {
        ability.modsApplied.Add(_mod);
        ability.AddMods(_mod);
        modList.CreateMod(_mod.icon, modPrefab);
    }

    // Display Ability Details
    public void ShowAbility()
    {
        elementName.SetText(ability.name);
        elementDescription.SetText("");
        elementIcon.sprite = ability.icon;
    }

    // Show Mod Details
    public void ShowDetails(NEWOptionPrefab _piece)
    {
        elementName.SetText(_piece.mod.name);
        elementDescription.SetText(_piece.mod.GetText(ability.type));
        elementIcon.sprite = _piece.mod.icon;
    }

    // Helper Function
    public float NormalizeAngle(float a) => (a + 360f) % 360f;
}
