using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent
{
    public enum AgentStatus
    {
        INRANGE,
        MOVINGTORANGE,
        OUTOFRANGE
    }

    protected AgentStatus agentStatus;
    protected AIType thisAIType;

    protected List<AIAction> MoveQueue = new List<AIAction>();
    protected List<AIAction> AttackQueue = new List<AIAction>();
    
    public Transform Target;
    public Transform AttachedObject;

    protected NavMeshAgent AINavAgent;
    
    protected uint MinTrackDist;
    protected uint MaxTrackDist;

    /// <summary>
    /// Defines the target, the attached object, and the required NavMeshAgent
    /// </summary>
    /// <param name="_Target">Transform for the player</param>
    /// <param name="_Attached">Transform this AIAgent is attached to</param>
    public AIAgent(Transform _Target, Transform _Attached)
    {
        Target = _Target;
        AttachedObject = _Attached;

        AINavAgent = AttachedObject.GetComponent<NavMeshAgent>();
    }

    public virtual void Act()
    {

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
