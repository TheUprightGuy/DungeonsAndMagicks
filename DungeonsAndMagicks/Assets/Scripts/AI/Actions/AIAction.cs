using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIAction
{
    public string Name;

    ///<summary>
    ///Time that the AI
    ///Action is used for
    ///</summary>
    public float m_fTimeForAction = 1.0f;

    ///<summary>
    ///Base Movement method
    ///</summary>
    public virtual void Move(Vector3 _target, ref NavMeshAgent _baseAgent) { }

    public virtual void Attack(Vector3 _target, ref NavMeshAgent _baseAgent) { }
}
