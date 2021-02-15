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

        stopDist = 25.0f;
        ViewRange = 40.0f;
        ViewAngle = 180.0f;

        wandertimer = Random.Range(0.0f, TimeBetweenWanders); //They all move in unison if not doing this and its super creepy
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

    float WanderRadius = 10.0f;
    float TimeBetweenWanders = 8.0f;
    float TimeToTurn = 5.0f;

    float wandertimer = 0.0f; //force straight into stage 1
    float rotateToAngle = 0.0f;
    Vector3 nextPos = Vector3.zero;
    Vector3 cachedDir = Vector3.zero;
    int iWanderStage = 0;
    private void Wander()
    {
        switch (iWanderStage)
        {
            case 0: //WAITING FOR NEXT POSITION
                wandertimer += Time.deltaTime;

                if (wandertimer > TimeBetweenWanders)
                {
                    Vector2 randOffset = Random.insideUnitCircle * WanderRadius; //Get NextPos
                    nextPos = new Vector3(RootTransform.position.x + randOffset.x, 0.0f, RootTransform.position.z + randOffset.y);

                    //bool test = Physics.Raycast(rayOrigin, Vector3.down, 10.0f, layerMask); //Identical to other script but always returning true - ??????
                    NavMeshPath path = new NavMeshPath();
                    bool isPath = RootTransform.GetComponent<NavMeshAgent>().CalculatePath(nextPos, path); //check is able to path 
                    while (!isPath) //Not able to path to pos
                    {
                        randOffset = Random.insideUnitCircle * WanderRadius; //Get NextPos
                        nextPos = new Vector3(RootTransform.position.x + randOffset.x, 0.0f, RootTransform.position.z + randOffset.y);
                        isPath = RootTransform.GetComponent<NavMeshAgent>().CalculatePath(nextPos, path); //check is able to path 

                    }


                    cachedDir = RootTransform.forward;
                    iWanderStage = 1;
                    wandertimer = 0.0f;

                }
                break;
            case 1: //ROTATING TO NEXT POSITION
                wandertimer += Time.deltaTime;

                Vector3 AIToNextDir = (nextPos - RootTransform.position).normalized;
                RootTransform.forward = Vector3.Lerp(cachedDir, AIToNextDir, (wandertimer / TimeToTurn));


                if (wandertimer > TimeToTurn)
                {
                    iWanderStage = 2;
                    wandertimer = 0.0f;
                }
                break;
            case 2: //WALKING TO NEXT POSITION
                RootTransform.GetComponent<NavMeshAgent>().isStopped = false;
                RootTransform.GetComponent<NavMeshAgent>().SetDestination(nextPos);

                if (Vector3.Distance(RootTransform.position, nextPos) < 0.5f) //At next pos
                {
                    iWanderStage = 0;
                }
                break;
            default:
                break;
        }

        //StartCoroutine(RotateTo(,));
    }

    private IEnumerator RotateTo(Vector3 start, Vector3 end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }
}
