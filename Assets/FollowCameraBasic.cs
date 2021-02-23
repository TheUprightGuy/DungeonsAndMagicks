using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraBasic : MonoBehaviour
{
    public Transform followTarget;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - followTarget.position;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.position + offset;
    }
}
