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

    #region Animation Variables
    [HideInInspector] public float currentMovMag;
    [HideInInspector] public float strafing = 0;
    [HideInInspector] public float backwards;
    #endregion Animation Variables

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
            finalMoveVec += horVec;
        }
        if (!Physics.Raycast(rayStart, vertVec, out hit, collisionCheckDistance))
        {
            finalMoveVec += vertVec;
        }

        // Update Animations
        float angle = Vector3.Angle(finalMoveVec, transform.right);
        if (finalMoveVec == Vector3.zero)
        {
            strafing = Mathf.Lerp(strafing, 0, Time.deltaTime);
            backwards = Mathf.Lerp(backwards, 0, Time.deltaTime);
        }
        else 
        {
            strafing = (angle - 90) / 90;
            backwards = (Vector3.Angle(finalMoveVec, transform.forward) - 90.0f) / -90.0f;
        }
        // Magnitude - Determine if Moving or not
        currentMovMag = Vector3.Magnitude(finalMoveVec);
        transform.position = transform.position + (finalMoveVec * Time.deltaTime * PlayerSpeed);
    }
}
