using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterMovement : MonoBehaviour
{
    public enum MoveModes
    {
        //NAVMESH, //Felt terrible and gross
        UPDATED,
        OLD,
    }

    [Header("Character Movement Settings")]
    public MoveModes MovementMode = MoveModes.UPDATED;
    public float collisionCheckDistance;
    public float PlayerSpeed = 10.0f;
    [HideIfEnumValue("MovementMode", HideIf.NotEqual, (int)MoveModes.UPDATED)]
    public LayerMask LayerMaskOnGround;
    [HideIfEnumValue("MovementMode", HideIf.NotEqual, (int)MoveModes.UPDATED)]
    public float StepUpHeight = 0.5f;
    [HideIfEnumValue("MovementMode", HideIf.NotEqual, (int)MoveModes.UPDATED)]
    public float OffsetHeight;
    public enum CamModes
    {
        [Tooltip("Has no border awareness and just follows the player")]
        EASY,
        [Tooltip("Attempts to avoid viewing over the edge")]
        SMART
    }
    [Header("Camera Settings")]

    public CamModes CameraMode = CamModes.EASY;
    [Tooltip("PlayerCamera will fetch from Camera.main if not set")]
    public Camera PlayerCamera;
    [HideIfEnumValue("CameraMode", HideIf.NotEqual, (int)CamModes.SMART)]
    public LevelTemplateUtilities LTU;
    [HideIfEnumValue("CameraMode", HideIf.NotEqual, (int)CamModes.SMART)]
    public float ScreenBorderSize = 0.0f;
    [HideIfEnumValue("CameraMode", HideIf.NotEqual, (int)CamModes.SMART)]
    public float smoothTime = 0.3F;

    //RandomShit
    Vector3 camOffset;
    Quaternion camRot = Quaternion.identity;
    private Vector3 velocity = Vector3.zero;

    #region Animation Variables
    [HideInInspector] public float currentMovMag;
    [HideInInspector] public float strafing = 0;
    [HideInInspector] public float backwards;
    #endregion Animation Variables

    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = (PlayerCamera == null) ? (Camera.main) : (PlayerCamera); //Get Main Camera if not set

        camOffset = Vector3.Normalize( PlayerCamera.transform.position - transform.position ) * Vector3.Distance(transform.position, PlayerCamera.transform.position);
        camRot = PlayerCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCharacter();


        MoveCharacter();
        switch (CameraMode)
        {
            case CamModes.EASY:
                EasyCam();
                break;
            case CamModes.SMART:
                SmartCam();
                break;
            default:
                break;
        }
        //MoveCamera();
    }

    #region Rotation Stuff
    private void LateUpdate()
    {
        //Create a ray from the Mouse click position
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
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

    Vector3 MouseToWorldPos = Vector3.zero;
    void RotateCharacter()
    {
        Vector3 a = MouseToWorldPos;
        a.y = 0.0f;

        Vector3 b = transform.position;
        b.y = 0.0f;
        Vector3 dir = Vector3.Normalize(a - b);
        transform.forward = dir;
    }
    #endregion Rotation Stuff

    void MoveCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 horVec = new Vector3(moveHorizontal, 0.0f, 0.0f);
        Vector3 vertVec = new Vector3(0.0f, 0.0f, moveVertical);

        Vector3 finalMoveVec = Vector3.zero;

        Vector3 rayStart = transform.position;

        if (MovementMode == MoveModes.UPDATED)
        {
            rayStart.y += StepUpHeight;
        }
        else
        {
            rayStart.y -= 0.4f;
        }
        RaycastHit hit;


        if (!Physics.Raycast(rayStart, horVec, out hit, collisionCheckDistance) || hit.collider.isTrigger)
        {
            finalMoveVec += horVec;
        }
        if (!Physics.Raycast(rayStart, vertVec, out hit, collisionCheckDistance) || hit.collider.isTrigger)
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
        
        Vector3 newPos = transform.position + (finalMoveVec * Time.deltaTime * PlayerSpeed);
        
        if (MovementMode == MoveModes.UPDATED)
        {
            RaycastHit rayhit = new RaycastHit();
            Vector3 rayPos = newPos;
            rayPos.y += StepUpHeight;

            if (Physics.Raycast(rayPos, Vector3.down, out rayhit, 69, LayerMaskOnGround)) //Checks down for the ground
            {
                newPos.y = rayhit.point.y + OffsetHeight;
            }
        }

        transform.position = newPos;

    }


    void SmartCam()
    {
        Vector3 oldPos = PlayerCamera.transform.position;
        Vector3 calcPos = transform.position + camOffset;
        

        PlayerCamera.transform.position = calcPos;

        float enterDist = 0.0f;
        Plane levelPlane = new Plane(Vector3.up, Vector3.zero);

        //LeftCheck
        /*********************************************/

        Ray leftRay = PlayerCamera.ScreenPointToRay(new Vector3(ScreenBorderSize, Screen.height / 2, 0.0f));

        levelPlane.Raycast(leftRay, out enterDist);
        Vector3 intersectPoint = leftRay.origin + (leftRay.direction * enterDist);

        bool leftCheck = LTU.CheckPointWithinLevelBounds(intersectPoint);

        //RightCheck
        /*********************************************/
        Ray rightRay = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width - ScreenBorderSize, Screen.height / 2, 0.0f));

        float right = 0.0f;
        levelPlane.Raycast(rightRay, out right);
        intersectPoint = rightRay.origin + (rightRay.direction * right);

        bool rightCheck = LTU.CheckPointWithinLevelBounds(intersectPoint);

        //TopCheck
        /*********************************************/
        Ray topRay = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height - ScreenBorderSize, 0.0f));


        float top = 0.0f;
        levelPlane.Raycast(topRay, out top);
        intersectPoint = topRay.origin + (topRay.direction * top);

        bool topCheck = LTU.CheckPointWithinLevelBounds(intersectPoint);

        //BottomCheck
        /*********************************************/
        Ray botRay = PlayerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, ScreenBorderSize, 0.0f));

        float bot = 0.0f;
        levelPlane.Raycast(botRay, out bot);
        intersectPoint = botRay.origin + (botRay.direction * bot);

        bool botCheck = LTU.CheckPointWithinLevelBounds(intersectPoint);


        PlayerCamera.transform.rotation = Quaternion.identity;



        Vector3 newPos = calcPos;


        if ((!rightCheck && calcPos.x > oldPos.x) ||//Over the right edge and going further
                (!leftCheck && calcPos.x < oldPos.x))//Over the left edge and going further
        {
            newPos.x = oldPos.x;
        }

        if ((!topCheck && calcPos.z > oldPos.z) ||//Over the top edge and going further
            (!botCheck && calcPos.z < oldPos.z))//Over the bottom edge and going further
        {
            newPos.z = oldPos.z;
        }


        PlayerCamera.transform.position = Vector3.SmoothDamp(oldPos, newPos, ref velocity, smoothTime);
        PlayerCamera.transform.rotation = camRot;
    }

    void EasyCam()
    {
        PlayerCamera.transform.position = transform.position + camOffset;
    }

    private void OnDrawGizmos()
    {

    }
}
