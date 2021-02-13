using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ScoutAI : AIAgent
{
    public ScoutAI()
    {
        //BaseMove baseMove = new BaseMove();
        //MovementQueue.Add(baseMove); //Gonna say handling where to move with an action is overkill

        BaseAttack baseAttack = new BaseAttack();
        AttackQueue.Add(baseAttack);

        thisAIType = AIType.SCOUT;

        stopDist = 10.0f;
        ViewRange = 50.0f;
        ViewAngle = 180.0f;
    }

    float stopDist = 10.0f;
    float DistTooCloseForComfortBro = 5.0f;

    private Vector3 velocity = Vector3.zero;

    public override void Randomise(uint AbilityCount, bool stack = true)
    {
        base.Randomise(AbilityCount, stack);
       
    }

    public override void Act()
    {
        base.Act();

        float AIToPlayerDist = Vector3.Distance(TargetTransform.position, RootTransform.position);
        Vector3 flatTargetPos = TargetTransform.position;
        flatTargetPos.y = 0.0f;
        Vector3 flatRootPos = RootTransform.position;
        flatTargetPos.y = 0.0f;
        Vector3 AIToPlayerDir = (flatTargetPos - flatRootPos).normalized;
        switch (agentStatus)
        {
            case DetectionStatus.DETECTED:
                LockAttackQueue = false;
                if (AIToPlayerDist > stopDist)
                {
                    RootTransform.GetComponent<NavMeshAgent>().isStopped = false;
                    RootTransform.GetComponent<NavMeshAgent>().SetDestination(TargetTransform.position + (AIToPlayerDir * stopDist));
                }
                else
                {
                    RootTransform.GetComponent<NavMeshAgent>().isStopped = true;
                    RootTransform.forward = Vector3.SmoothDamp(RootTransform.forward, AIToPlayerDir, ref velocity, 0.1f);
                }

                //if(AIToPlayerDist < DistTooCloseForComfortBro)
                //{
                //    RootTransform.GetComponent<NavMeshAgent>().isStopped = false;
                //    RootTransform.GetComponent<NavMeshAgent>().SetDestination(TargetTransform.position - (AIToPlayerDir * stopDist));
                //}
                //else
                //{
                    
                //}

                break;
            case DetectionStatus.INRANGE:
                LockAttackQueue = true;
                break;
            case DetectionStatus.OUTOFRANGE:
                LockAttackQueue = true;
                Wander();
                break;
            default:
                break;
        }
        
        MoveToTarget = TargetTransform.position;
        DoActionQueue();
    }

    private void Seek()
    {

    }

    private void Wander()
    {

    }
}
