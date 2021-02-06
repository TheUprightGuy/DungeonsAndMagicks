using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will probably use this for damage absorption
public class ShieldAlphaScript : MonoBehaviour
{
    Material mat;
    public float alpha = 0.0f;
    [HideInInspector] public float duration;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        duration -= Time.deltaTime;

        alpha = (duration > 1.0f) ? alpha + Time.deltaTime : alpha - Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        mat.SetFloat("Alpha", alpha);
    }
}
