using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnvironmentTransparancyController : MonoBehaviour
{
    Camera MainCam;
    public Shader refShader;

    public float TransparencyModifier;

    public float RayThiccNess = 2.0f;
    void Awake()
    {
        MainCam = Camera.main;

    }



    RaycastHit[] hits;
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 screenPos = MainCam.WorldToScreenPoint(transform.position);
        Ray camRay = MainCam.ScreenPointToRay(screenPos);


        if (hits != null)
        {
            foreach (RaycastHit item in hits)
            {
                if (item.collider.gameObject.GetComponent<MeshRenderer>().material.shader == refShader)
                {
                    //StartCoroutine(MoveToAlpha(0.0f, 1.0f, 0.5f, item.collider.gameObject.GetComponent<MeshRenderer>()));
                    item.collider.gameObject.GetComponent<MeshRenderer>().material.SetVector("Vector3_be9a2389ddea4e8f9ded27341aa1fbfe", camRay.origin);//LineStart
                    item.collider.gameObject.GetComponent<MeshRenderer>().material.SetVector("Vector3_497cf597f45f4dd4a25ff67eff5dbed9", camRay.direction);//LineDir
                }
            }

            Array.Clear(hits, 0, hits.Length);
        }


        hits = Physics.SphereCastAll(camRay, RayThiccNess, 100.0F);
        
        foreach (RaycastHit item in hits)
        {
            if (item.collider.gameObject.GetComponent<MeshRenderer>().material.shader == refShader)
            {
                //Vector1_f43e6d0c99874a298a8a901fc1170d81
                item.collider.gameObject.GetComponent<MeshRenderer>().material.SetVector("Vector3_be9a2389ddea4e8f9ded27341aa1fbfe", camRay.origin);//LineStart
                item.collider.gameObject.GetComponent<MeshRenderer>().material.SetVector("Vector3_497cf597f45f4dd4a25ff67eff5dbed9", camRay.direction);//LineDir
            }
        }
        
    }


    IEnumerator MoveToAlpha(float from, float to,float timeToMove, MeshRenderer renderer)
    {
        for (float i = 0.0f; i < 1.0f; i += 0.1f)
        {
            renderer.material.SetFloat("Vector1_f43e6d0c99874a298a8a901fc1170d81", Mathf.Lerp(from, to, i));
            yield return new WaitForSeconds(timeToMove / 10.0f);
        }
    }
}
