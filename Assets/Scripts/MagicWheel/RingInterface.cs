using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingInterface : MonoBehaviour
{
    [Header("Required Setup")]
    public int currentData = 0;
    public List<RingMenu> data;
    public OptionPrefab optionPrefab;
    public ModList modList;
    public GameObject modPrefab;
    [Header("Mod Display")]
    public TMPro.TextMeshProUGUI elementName;
    public TMPro.TextMeshProUGUI elementDescription;
    public Image elementIcon;
    // Local Variables
    protected List<OptionPrefab> pieces;
    private float gapWidthDegree = 1.0f;
    [HideInInspector] public Ability activeAbility;

    public void CreateRing(int _current)
    {
        CleanUp();

        currentData = _current;

        // Position Data
        float stepLength = 360.0f / data[currentData].elements.Count;
        float iconDist = Vector3.Distance(optionPrefab.icon.transform.position, optionPrefab.piece.transform.position);
        pieces = new List<OptionPrefab>();

        // Iterate through Menu Elements and Instantiate Prefabs w/ Attributes
        for (int i = 0; i < data[currentData].elements.Count; i++)
        {
            // Instantiate Prefab
            pieces.Add(Instantiate(optionPrefab, transform));
            pieces[i].element = data[currentData].elements[i];
            // Set Root Element
            pieces[i].transform.localPosition = Vector3.zero;
            pieces[i].transform.localRotation = Quaternion.identity;
            // Set Piece Position & Fill
            pieces[i].piece.fillAmount = 1f / data[currentData].elements.Count - gapWidthDegree / 360f;
            pieces[i].piece.transform.localPosition = Vector3.zero;
            pieces[i].piece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + gapWidthDegree / 2f + i * stepLength);
            // Set Icon Position & Sprite
            pieces[i].icon.transform.localPosition = pieces[i].piece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            pieces[i].backPlate.transform.localPosition = pieces[i].piece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            pieces[i].icon.sprite = data[currentData].elements[i].icon;
            // Set Parent
            pieces[i].parent = this;
            pieces[i].id = i;
        }
    }

    public void CleanUp()
    {
        if (pieces != null)
        {
            foreach (OptionPrefab n in pieces)
            {
                Destroy(n.gameObject);
            }
            pieces.Clear();
        }
    }

    public bool NextRing()
    {
        CleanUp();
        currentData++;

        if (currentData < data.Count)
        {
            CreateRing(currentData);
            return true;
        }
        return false;      
    }

    private void Update()
    {
        // Get Highlighted Option
        float stepLength = 360.0f / data[currentData].elements.Count;
        float mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f);
        int activeElement = (int)(mouseAngle / stepLength);

        // Check if Highlighted
        for (int i = 0; i < data[currentData].elements.Count; i++)
        {
            pieces[i].backPlate.sprite = (i == activeElement) ? pieces[i].selectedSprite : pieces[i].notSelectedSprite;
        }

        // Update Centre Text w/ Element Description
        elementName.SetText(pieces[activeElement].element.name);
        elementDescription.SetText(pieces[activeElement].element.description);
        elementIcon.sprite = pieces[activeElement].element.icon;

        // Clicked Element
        if (Input.GetMouseButtonDown(0))
        {
            // Add Functionality
            pieces[activeElement].Use(activeAbility);
        }
    }

    public void SetActiveAbility(Ability _ability)
    {
        activeAbility = _ability;
        data = _ability.modRings;
        modList.ClearIcons();
    }

    private float NormalizeAngle(float a) => (a + 360f) % 360f;
}
