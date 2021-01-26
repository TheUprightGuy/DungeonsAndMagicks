using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Settings")]
    public float collisionCheckDistance;
    public float PlayerSpeed = 10.0f;


    [Range(0.0f, 1000.0f)]
    public float BorderThiccNess = 0.0f;
    public float smoothTime = 0.3F;

    [Header("Required Components")]
    public LevelTemplateUtilities LTU;


    GetMouseInWorld getMousePos;
    Vector3 camOffset;
    Quaternion camRot = Quaternion.identity;
    Camera mainCam;
    private Vector3 velocity = Vector3.zero;

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
        camRot = mainCam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCharacter();
        MoveCharacter();
        MoveCamera();
    }
    private void LateUpdate()
    {
        
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


        finalMoveVec.Normalize();

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
    void MoveCamera()
    {
        Vector3 oldPos = mainCam.transform.position;
        Vector3 calcPos = transform.position + camOffset;
        

        mainCam.transform.position = calcPos;

        float enterDist = 0.0f;
        Plane levelPlane = new Plane(Vector3.up, Vector3.zero);

        //LeftCheck
        /*********************************************/

        Ray leftRay = mainCam.ScreenPointToRay(new Vector3(BorderThiccNess, Screen.height / 2, 0.0f));

        levelPlane.Raycast(leftRay, out enterDist);
        Vector3 intersectPoint = leftRay.origin + (leftRay.direction * enterDist);

        bool leftCheck = LTU.PointWithinLevelBounds(intersectPoint);

        //RightCheck
        /*********************************************/
        Ray rightRay = mainCam.ScreenPointToRay(new Vector3(Screen.width - BorderThiccNess, Screen.height / 2, 0.0f));

        float right = 0.0f;
        levelPlane.Raycast(rightRay, out right);
        intersectPoint = rightRay.origin + (rightRay.direction * right);

        bool rightCheck = LTU.PointWithinLevelBounds(intersectPoint);

        //TopCheck
        /*********************************************/
        Ray topRay = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height - BorderThiccNess, 0.0f));


        float top = 0.0f;
        levelPlane.Raycast(topRay, out top);
        intersectPoint = topRay.origin + (topRay.direction * top);

        bool topCheck = LTU.PointWithinLevelBounds(intersectPoint);

        //BottomCheck
        /*********************************************/
        Ray botRay = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, BorderThiccNess, 0.0f));

        float bot = 0.0f;
        levelPlane.Raycast(botRay, out bot);
        intersectPoint = botRay.origin + (botRay.direction * bot);

        bool botCheck = LTU.PointWithinLevelBounds(intersectPoint);


        mainCam.transform.rotation = Quaternion.identity;



        Vector3 newPos = calcPos;


        if ((!rightCheck && calcPos.x > oldPos.x) ||//Over the right edge and going further
                (!leftCheck && calcPos.x < oldPos.x))//Over the left edge and going further
        {
            newPos.x = oldPos.x;
        }

        if ((!topCheck && calcPos.y > oldPos.y) ||//Over the top edge and going further
            (!botCheck && calcPos.y < oldPos.y))//Over the bottom edge and going further
        {
            newPos.y = oldPos.y;
        }


        mainCam.transform.position = Vector3.SmoothDamp(oldPos, newPos, ref velocity, smoothTime); ;
        mainCam.transform.rotation = camRot;
    }


    private void OnDrawGizmos()
    {
        float enterDist = 0.0f;
        Plane levelPlane = new Plane(Vector3.up, Vector3.zero);


        Ray leftRay = mainCam.ScreenPointToRay(new Vector3(BorderThiccNess, Screen.height / 2, 0.0f));

        levelPlane.Raycast(leftRay, out enterDist);
        Vector3 intersectPoint = leftRay.origin + (leftRay.direction * enterDist);

        bool leftCheck = LTU.PointWithinLevelBounds(intersectPoint);

        Gizmos.color = (leftCheck) ? (Color.white) : (Color.red);
        Gizmos.DrawSphere(intersectPoint, 0.5f);

        Ray rightRay = mainCam.ScreenPointToRay(new Vector3(Screen.width - BorderThiccNess, Screen.height / 2, 0.0f));

        float right = 0.0f;
        levelPlane.Raycast(rightRay, out right);
        intersectPoint = rightRay.origin + (rightRay.direction * right);

        bool rightCheck = LTU.PointWithinLevelBounds(intersectPoint);

        Gizmos.color = (rightCheck) ? (Color.white) : (Color.red);
        Gizmos.DrawSphere(intersectPoint, 0.5f);
    }
}
