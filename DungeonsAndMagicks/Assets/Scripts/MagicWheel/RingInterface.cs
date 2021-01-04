using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInterface : MonoBehaviour
{
    [Header("Required Setup")]
    public RingMenu data;
    public OptionPrefab optionPrefab;
    public GameObject modList;
    public GameObject modPrefab;
    [Header("Highlight Options")]
    public Color activeColor = new Color(1f, 1f, 1f, 0.75f);
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.5f);

    // Local Variables
    protected OptionPrefab[] pieces;
    private TMPro.TextMeshProUGUI elementText;
    private float gapWidthDegree = 1.0f;

    [Header("Temp")]
    public Ability activeAbility;

    private void Awake()
    {
        elementText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Start()
    {
        // Position Data
        float stepLength = 360.0f / data.elements.Length;
        float iconDist = Vector3.Distance(optionPrefab.icon.transform.position, optionPrefab.piece.transform.position);
        pieces = new OptionPrefab[data.elements.Length];

        // Iterate through Menu Elements and Instantiate Prefabs w/ Attributes
        for (int i = 0; i < data.elements.Length; i++)
        {
            // Instantiate Prefab
            pieces[i] = Instantiate(optionPrefab, transform);
            pieces[i].element = data.elements[i];
            // Set Root Element
            pieces[i].transform.localPosition = Vector3.zero;
            pieces[i].transform.localRotation = Quaternion.identity;
            // Set Piece Position & Fill
            pieces[i].piece.fillAmount = 1f / data.elements.Length - gapWidthDegree / 360f;
            pieces[i].piece.transform.localPosition = Vector3.zero;
            pieces[i].piece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + gapWidthDegree / 2f + i * stepLength);
            pieces[i].piece.color = new Color(1f, 1f, 1f, 0.5f);
            // Set Icon Position & Sprite
            pieces[i].icon.transform.localPosition = pieces[i].piece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            pieces[i].icon.sprite = data.elements[i].icon;
        } 
    }

    private void Update()
    {
        // Get Highlighted Option
        float stepLength = 360.0f / data.elements.Length;
        float mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f);
        int activeElement = (int)(mouseAngle / stepLength);

        // Check if Highlighted
        for (int i = 0; i < data.elements.Length; i++)
        {
            pieces[i].piece.color = (i == activeElement) ? activeColor : inactiveColor;
        }

        // Update Centre Text w/ Element Description
        elementText.SetText(pieces[activeElement].element.name);

        // Clicked Element
        if (Input.GetMouseButtonDown(0))
        {
            // Add Functionality
            pieces[activeElement].Use(activeAbility);
            //gameObject.SetActive(false);
        }
    }

    public void SetActiveAbility(Ability _ability)
    {
        activeAbility = _ability;
    }

    private float NormalizeAngle(float a) => (a + 360f) % 360f;
}
