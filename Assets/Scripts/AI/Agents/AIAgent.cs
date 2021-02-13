using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent
{
    public enum DetectionStatus
    {
        /// <summary>
        /// Player is seen and can be acted upon
        /// </summary>
        DETECTED,
        /// <summary>
        /// Player is within the AI view distance
        /// </summary>
        INRANGE,
        /// <summary>
        /// Player is out of range
        /// </summary>
        OUTOFRANGE
    }

    protected DetectionStatus agentStatus;
    public AIType thisAIType;

    public List<AIAction> AttackQueue = new List<AIAction>();
    public List<AIAction> MovementQueue = new List<AIAction>();

    protected bool LockMovementQueue = false;
    protected Vector3 MoveToTarget = Vector3.zero;
    protected bool LockAttackQueue = false;
    protected Vector3 AttackTarget = Vector3.zero;

    public Transform TargetTransform;
    public Transform RootTransform;

    public NavMeshAgent AINavAgent;
    

    protected float ViewRange = 10;
    protected float ViewAngle = 180.0f;
   
    /// <summary>
    /// Updates the agentStatus for what is visible to the AI,
    /// based on MaxTrackDist.
    /// </summary>
    private void UpdateDetection()
    {
        float AIToPlayerDist = Vector3.Distance(TargetTransform.position, RootTransform.position);
        if (AIToPlayerDist > ViewRange)
        {
            agentStatus = DetectionStatus.OUTOFRANGE;
        }
        else if (AIToPlayerDist < ViewRange)
        {
            agentStatus = DetectionStatus.INRANGE;

            Vector3 rootPos = RootTransform.position;
            Vector3 targetPos = TargetTransform.position;
            rootPos.y = 0.0f;
            targetPos.y = 0.0f;

            Vector3 playerDir = (targetPos - rootPos).normalized;
            float angle = Vector3.Angle(playerDir, RootTransform.forward);
            if (angle < (ViewAngle / 2)) //Within the forward view angle of the AI
            {
                RaycastHit hit = new RaycastHit();
                rootPos.y = 1.0f;
                if (Physics.Raycast(rootPos, playerDir,
                    out hit,
                    Vector3.Distance(TargetTransform.position, RootTransform.position)
                    ))
                {
                    if (hit.collider.tag == "Player")
                    {
                        agentStatus = DetectionStatus.DETECTED;
                    }
                }
            }
        }

    }

    /// <summary>
    /// Base act calls:
    /// <list type="bullet">
    /// <item>
    /// <see cref="UpdateDetection()"/>
    /// </item>
    /// </list>
    /// </summary>
    public virtual void Act()
    {
        UpdateDetection();
    }


    public int AttackIndex = 0;
    private int MovementIndex = 0;

    private float attackChangeTimer = 0.0f;
    private float movementChangeTimer = 0.0f;

    private float attackDelayTimer = 0.0f;
    private float movementDelayTimer = 0.0f;
    public void DoActionQueue()
    {
        if (AttackQueue.Count < 1 && MovementQueue.Count < 1)
        {
            Debug.Log(RootTransform.name + " AIAgent has no actions in its queue");
            return;
        }


        AttackQueueHandling();
        MovementQueueHandling();
    }

    private void AttackQueueHandling()
    {

        if (AttackQueue.Count <= 0) //Error Handling
        {
            return;
        }
        //Attack Action handling
        /*******************************************************************/
        if (attackChangeTimer > AttackQueue[AttackIndex].m_fTimeForAction) //Check if next action needs to be queued
        {
            AttackIndex++;
            AttackIndex = AttackIndex % AttackQueue.Count;//Loop if required
            attackChangeTimer = 0.0f;//Reset timer
        }
        else
        {
            attackChangeTimer += Time.deltaTime; //Continue 
        }

        if (attackDelayTimer >= AttackQueue[AttackIndex].m_fActionDelay //Check for delay between each action call
            && !LockAttackQueue) //Check the queue isn't locked
        {
            AttackQueue[AttackIndex].Attack(TargetTransform, RootTransform);
            attackDelayTimer = 0.0f;
        }
        else if (!LockAttackQueue) //Comment this if out if wanting to time while locked
        {
            attackDelayTimer += Time.deltaTime;
        }
        /*******************************************************************/
    }

    private void MovementQueueHandling()
    {
        if (MovementQueue.Count <= 0) //Error Handling
        {
            return;
        }
        //Movement Action handling
        /*******************************************************************/
        if (movementChangeTimer > MovementQueue[MovementIndex].m_fTimeForAction) //Check if next action needs to be queued
        {
            MovementIndex++;
            MovementIndex = MovementIndex % MovementQueue.Count;//Loop if required
            movementChangeTimer = 0.0f;//Reset timer
        }
        else
        {
            movementChangeTimer += Time.deltaTime; //Continue 
        }

        if (movementDelayTimer >= MovementQueue[MovementIndex].m_fActionDelay //Check for delay between each action call
            && !LockMovementQueue) //Check the queue isn't locked
        {
            MovementQueue[MovementIndex].Move(TargetTransform, RootTransform);
            movementDelayTimer = 0.0f;
        }
        else if (!LockMovementQueue) //Comment this if out if wanting to time while locked
        {
            movementDelayTimer += Time.deltaTime;
        }
        /*******************************************************************/

    }
    /// <summary>
    /// Pulls a random AIAction queue, adding AbilityCount amount of AIActions if stack true
    /// Clears then adds if stack is false
    /// </summary>
    /// <param name="AbilityCount">The amount of actions to apply</param>
    /// <param name="stack">clears queues before adding if false, default is true</param>
    public virtual void Randomise(uint AbilityCount, bool stack = true)
    {

    }


    public AIAgent ShallowCopy()
    {
        return (AIAgent)this.MemberwiseClone();
    }


    

    /// <summary>
    /// Gets a random point on a torus in 2D within two distances
    /// </summary>
    /// <param name="_Centre">The centre point of the torus</param>
    /// <param name="_fMin">The inside circle of the torus</param>
    /// <param name="_fMax">The outside circle of the torus</param>
    /// <returns>The random point on the torus within the two distances</returns>
    Vector3 RandomInTorus(Vector2 _Centre, float _fMin, float _fMax)
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector2 direction = new Vector3(x, y);
        //if you need the vector to have a specific length:
        direction = direction.normalized * Random.Range(_fMin, _fMax);

        return (_Centre + direction);
    }
}
