using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIAction :ScriptableObject
{
    public string id;

    ///<summary>
    ///Time that the AI
    ///Action is used for
    ///</summary>
    public float m_fTimeForAction = 1.0f;
    public float m_fActionDelay = 1.0f;
    ///<summary>
    ///Base Movement method 
    ///</summary>
    public virtual void Move(Transform _target, Transform _root) { }

    public virtual void Attack(Transform _target, Transform _root) { }


}
