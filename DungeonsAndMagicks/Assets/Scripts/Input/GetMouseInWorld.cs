using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMouseInWorld : MonoBehaviour
{
    Camera mainCam = null;
    Plane groundPlane;

    public Transform GroundTransform;

    public Vector3 MouseToWorldPos = Vector3.zero;

    
    public bool Debug = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        groundPlane = new Plane(Vector3.up, 
            (GroundTransform == null) ? (Vector3.zero) : (GroundTransform.position));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Create a ray from the Mouse click position
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        //Initialise the enter variable
        float enter = 0.0f;

        if (groundPlane.Raycast(ray, out enter))
        {
            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);
            hitPoint.y = 0.0f;

            MouseToWorldPos = hitPoint;
        }
    }

    private void OnDrawGizmos()
    {
        if (Debug)
        {
            Gizmos.DrawSphere(MouseToWorldPos, 0.5f);
        }
    }
}
