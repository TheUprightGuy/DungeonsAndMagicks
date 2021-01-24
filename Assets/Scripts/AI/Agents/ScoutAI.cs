using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutAI : AIAgent
{
    public ScoutAI()
    {
        BaseMove baseMove = new BaseMove();
        ActionQueue.Add(baseMove);
        thisAIType = AIType.SCOUT;
    }

    public override void Randomise(uint AbilityCount, bool stack = true)
    {
        base.Randomise(AbilityCount, stack);
    }

    public override void Act()
    {
        base.Act();

        float AIToPlayerDist = Vector3.Distance(TargetTransform.position, AttachedTransform.position);
        switch (agentStatus)
        {
            case DetectionStatus.DETECTED:
                break;
            case DetectionStatus.INRANGE:
                if (AIToPlayerDist > MinTrackDist)
                {
                    LockMovementQueue = true;
                    LockAttackQueue = false;
                }
                else
                {
                    LockAttackQueue = true;
                    LockMovementQueue = false;
                }
                break;
            case DetectionStatus.OUTOFRANGE:
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
