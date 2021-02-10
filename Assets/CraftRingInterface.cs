using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftRingInterface : RingInterface
{
    public override void Update()
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
    }
}