using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAttack : AIAction
{

    public BaseAttack()
    {
        m_fActionDelay = 2.0f;
        id = "BaseAttack";
        proj = Resources.Load<GameObject>("Projectile");
    }

    GameObject proj;
    public float Accuracy = 1.0f;

    public override void Attack(Transform _target, Transform _root)
    {
        Vector2 pos = Random.insideUnitCircle * Accuracy;
        Vector3 convPos = new Vector3(pos.x, 0.0f, pos.y);
        proj.transform.forward = (_target.position + convPos) - _root.position;
        proj.transform.position = _root.position;

        Instantiate(proj);
    }
}
