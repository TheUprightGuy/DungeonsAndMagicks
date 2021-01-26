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

    protected List<AIAction> ActionQueue = new List<AIAction>();

    protected bool LockMovementQueue = false;
    protected Vector3 MoveToTarget = Vector3.zero;
    protected bool LockAttackQueue = false;
    protected Vector3 AttackTarget = Vector3.zero;

    public Transform TargetTransform;
    public Transform RootTransform;

    public NavMeshAgent AINavAgent;
    

    protected float MinTrackDist = 1;
    protected float MaxTrackDist = 10;

    protected float ViewAngle = 180.0f;
   
    /// <summary>
    /// Updates the agentStatus for what is visible to the AI,
    /// based on MaxTrackDist.
    /// </summary>
    private void UpdateDetection()
    {
        float AIToPlayerDist = Vector3.Distance(TargetTransform.position, RootTransform.position);
        if (AIToPlayerDist > MaxTrackDist)
        {
            agentStatus = DetectionStatus.OUTOFRANGE;
        }
        else if (AIToPlayerDist < MaxTrackDist)
        {
            agentStatus = DetectionStatus.INRANGE;
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


    private int ActionIndex = 0;
    private float timer = 0.0f;
    public void DoActionQueue()
    {
        if (ActionQueue.Count < 1)
        {
            Debug.Log(RootTransform.name + " AIAgent has no actions in its queue");
            return;
        }

        if (timer > ActionQueue[ActionIndex].m_fTimeForAction)
        {
            ActionIndex++;
            ActionIndex = ActionIndex % ActionQueue.Count;
            timer = 0.0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (!LockMovementQueue)
        {
            ActionQueue[ActionIndex].Move(TargetTransform, RootTransform);
        }
        if (!LockAttackQueue)
        {
            ActionQueue[ActionIndex].Attack(TargetTransform, RootTransform);
        }
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
    /// Get a valid target in the navmesh environment
    /// </summary>
    /// <returns>The valid destination</returns>
    //public Vector3 GetValidTarget()
    //{
    //    Vector3 validTarget = Target;

    //    int counter = 0;
    //    const int breakOutCount = 100;

    //    while (counter < breakOutCount) //found a complete path or had to break out
    //    {
    //        Vector2 randPoint = RandomInTorus(Target, ClosestDist, FurthestDist);
    //        validTarget = new Vector3(randPoint.x, 0.0f, randPoint.y);

    //        NavMeshPath path = new NavMeshPath();
    //        AINavAgent.CalculatePath(validTarget, path);

    //        if (path.status == NavMeshPathStatus.PathComplete)//Check for a completable path
    //        {
    //            break;
    //        }
    //        else
    //        {
    //            validTarget = Target;
    //        }

    //        counter++;
    //    }
    //    return validTarget;
    //}

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
