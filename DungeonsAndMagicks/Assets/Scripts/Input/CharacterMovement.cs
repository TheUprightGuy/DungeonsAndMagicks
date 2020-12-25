using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float collisionCheckDistance;
    public float PlayerSpeed = 10.0f;
    GetMouseInWorld getMousePos;
    Vector3 camOffset;
    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        getMousePos = GetComponent<GetMouseInWorld>();

        mainCam = Camera.main;
        camOffset = Vector3.Normalize( mainCam.transform.position - transform.position ) * Vector3.Distance(transform.position, mainCam.transform.position);
    }


    private void LateUpdate()
    {
        mainCam.transform.position = transform.position + camOffset;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCharacter();
        MoveCharacter();
    }

    void RotateCharacter()
    {
        Vector3 a = getMousePos.MouseToWorldPos;
        a.y = 0.0f;

        Vector3 b = transform.position;
        b.y = 0.0f;
        Vector3 dir = Vector3.Normalize(a - b);
        transform.forward = dir;
    }
    void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 horVec = new Vector3(moveHorizontal, 0.0f, 0.0f);
        Vector3 vertVec = new Vector3(0.0f, 0.0f, moveVertical);

        Vector3 finalMoveVec = Vector3.zero;

        Vector3 rayStart = transform.position;
        rayStart.y -= 0.4f;
        RaycastHit hit;
        if (!Physics.Raycast(rayStart, horVec, out hit, collisionCheckDistance))
        {
            finalMoveVec += horVec;  //transform.position + (movement * Time.deltaTime * PlayerSpeed);
        }
        if (!Physics.Raycast(rayStart, vertVec, out hit, collisionCheckDistance))
        {
            finalMoveVec += vertVec;
        }

        //transform.Translate((finalMoveVec * Time.deltaTime * PlayerSpeed));
        transform.position = transform.position + (finalMoveVec * Time.deltaTime * PlayerSpeed);
    }
}
