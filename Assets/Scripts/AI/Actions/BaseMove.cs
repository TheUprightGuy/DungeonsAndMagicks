using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMove : AIAction
{
    public NavMeshAgent agent = null;
    public override void Move(Transform _target, Transform _root)
    {
        m_fActionDelay = 0.0f;
        if (agent == null)
        {
            agent = _root.GetComponent<NavMeshAgent>();
        }
        //base.Move(_target, ref _baseAgent);
        agent.SetDestination(_target.position);
    }
}
