using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HomingAttack : AIAction
{

    public HomingAttack()
    {
        m_fTimeForAction = 20.0f;
        m_fActionDelay = 5.0f;
        id = "HomingAttack";
        proj = Resources.Load<GameObject>("Projectile");
        
    }


    EnemyAnimatorFunctions animHandler;
    GameObject proj;

    Transform target;
    Transform root;

    float maxspeed = 7.0f;
    float maxforce = 0.25f;


    public override void Attack(Transform _target, Transform _root)
    {
        GameObject obj = Instantiate(proj);
        obj.transform.position = _root.position;
        AIProjectile aip = obj.GetComponent<AIProjectile>();

        target = _target;
        root = _root;

        aip.OverridingBehavior.RemoveAllListeners();
        aip.OverridingBehavior.AddListener(Homing);
    }

    Vector3 vel = Vector3.zero;
    Vector3 accel = Vector3.zero;
    public void Homing(Transform _trans)
    {
        Rigidbody rb = _trans.GetComponent<Rigidbody>();

        Vector3 tPos = target.position;
        Vector3 rPos = _trans.position;
        Vector3 desired = tPos - rPos;
        desired.Normalize();
        desired *= (maxspeed);

        Vector3 steer = desired - vel;
        steer = Vector3.ClampMagnitude(steer, maxforce);

        accel += steer;

        vel += accel;

        vel = Vector3.ClampMagnitude(vel, maxspeed);

        rb.MovePosition(_trans.position + vel * Time.deltaTime);
        accel *= 0.0f;


        _trans.forward = vel.normalized;
    }
}
