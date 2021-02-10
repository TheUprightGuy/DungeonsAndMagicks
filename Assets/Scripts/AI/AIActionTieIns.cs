using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AnimFloatType
{
    BOOL,
    FLOAT,
    INT,
    TRIGGER
}
[System.Serializable]
public struct AIActionAnim
{
    public string ActionID;
    public string AnimationID;

    [Header("Animation Parameters")]
    public AnimFloatType AnimationParameterType;

    [HideInInspector]
    public bool isTriggered;

    public bool AnimBoolIfTriggered;
    public float AnimFloatIfTriggered;
    public int AnimIntIfTriggered;
}


public class AIActionTieIns : MonoBehaviour
{
    public bool staticAI = false;

    public AIActionAnim[] ActionAnimationBools;


    private NavMeshAgent ControllingAgent;
    private Animator animator;
    private AIObject attachedAI;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (!staticAI)
        {
            ControllingAgent = GetComponent<NavMeshAgent>();
        }

        attachedAI = GetComponent<AIObject>();
    }

    private Vector3 previousPosition;
    // Update is called once per frame
    void Update()
    {
        if (!staticAI)
        {
            //animator.SetFloat("MovementSpeed", ControllingAgent.velocity != Vector3.zero ? 1.0f : 0.0f);
            //Need to find someway to sync the walking to the navmeshagent
        }

        ManageAnimBools();
        SetAnimatorParams();
    }

    void ManageAnimBools()
    {

        //string ctmID = "";
        //string ctaID = "";

        //int ctm = attachedAI.thisAgent.MoveIndexTriggeredThisFrame;
        //int cta = attachedAI.thisAgent.AttackIndexTriggeredThisFrame;

        //if (ctm >= 0)
        //{
        //    ctmID = attachedAI.thisAgent.MovementQueue[ctm].id;
        //}
        //if (cta >= 0)
        //{
        //    ctaID = attachedAI.thisAgent.AttackQueue[cta].id;
        //}


        //for (int i = 0; i < ActionAnimationBools.Length; i++)
        //{
        //    if (ctmID == ActionAnimationBools[i].ActionID || ctaID == ActionAnimationBools[i].ActionID)
        //    {
        //        ActionAnimationBools[i].isTriggered = true;
        //    }
        //    else
        //    {
        //        ActionAnimationBools[i].isTriggered = false;
        //    }
        //}

    }

    void SetAnimatorParams()
    {
        foreach (AIActionAnim item in ActionAnimationBools)
        {
            if (!item.isTriggered)
            {
                continue;
            }

            switch (item.AnimationParameterType)
            {
                case AnimFloatType.BOOL:
                    animator.SetBool(item.AnimationID, item.AnimBoolIfTriggered);
                    break;
                case AnimFloatType.FLOAT:
                    animator.SetFloat(item.AnimationID, item.AnimFloatIfTriggered);
                    break;
                case AnimFloatType.INT:
                    animator.SetInteger(item.AnimationID, item.AnimIntIfTriggered);
                    break;
                case AnimFloatType.TRIGGER:
                    animator.SetTrigger(item.AnimationID);
                    break;
                default:
                    break;
            }
        }
    }
}
