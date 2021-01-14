using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIAgent : MonoBehaviour
{
    public List<AIAction> ActionQueue = new List<AIAction>();

    private NavMeshAgent AINavAgent;
    
    public Vector3 Target;
    private Vector3 TargetCache;

    private Vector3 MoveTarget;
    private Vector3 AttackTarget;

    
    public uint ClosestDist;
    public uint FurthestDist;

    // Start is called before the first frame update
    void Awake()
    {    
        AINavAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        TargetCache = Vector3.positiveInfinity;
        MoveTarget = Target;

        AttackTarget = Target;
    }

    void Update()
    {
        AgentUpdate();
        DoAgentQueue();


        //On Change of target given a range
        if (FurthestDist > 0 && Target != TargetCache)
        {
            //Update the cached target
            TargetCache = Target;
            MoveTarget = GetValidTarget();

            AINavAgent.SetDestination(MoveTarget);
        }

        //ActionQueue[0].Move(Target, ref AINavAgent);
    }

    /// <summary>
    /// Called within the update for Monobehaviour
    /// CALL DoAgentQueue() for AIActions
    /// </summary>
    public virtual void AgentUpdate()
    {
        
    }

    public void DoAgentQueue()
    {

    }

    public 


    /// <summary>
    /// Get a valid target in the navmesh environment
    /// </summary>
    /// <returns>The valid destination</returns>
    Vector3 GetValidTarget()
    {
        Vector3 validTarget = Target;

        int counter = 0;
        const int breakOutCount = 100;

        while ( counter < breakOutCount) //found a complete path or had to break out
        {
            Vector2 randPoint = RandomInTorus(Target, ClosestDist, FurthestDist);
            validTarget = new Vector3(randPoint.x, 0.0f, randPoint.y);

            NavMeshPath path = new NavMeshPath();
            AINavAgent.CalculatePath(validTarget, path);

            if (path.status == NavMeshPathStatus.PathComplete)//Check for a completable path
            {
                break;
            }
            else
            {
                validTarget = Target;
            }

            counter++;
        }
        return validTarget;
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
