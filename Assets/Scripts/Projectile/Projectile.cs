using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    ProjectileAbilityModifiers mods;
    public float speed;
    public float lifetime;
    // temp
    public float damage;

    private List<Transform> chainTargets = new List<Transform>();


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            End();
        }
    }

    public void Setup(ProjectileAbilityModifiers _mods)
    {
        mods = _mods;
    }
    
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * speed);
    }
    private void End()
    {
        Destroy(gameObject);
    }

    private void Explode(Transform _transform)
    {
        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, 5.0f);
        foreach (Collider n in hitColliders)
        {
            // make sure enemy
            if (n.gameObject.CompareTag("Enemy"))
            {
                // Damage Enemy
                _transform.GetComponent<AIEnemyStats>().TakeDamage(10.0f); //Wayd pls remove magic numbers and put in proper call for damage amount

            }
        }
        End();
    }

    private void Pierce(Transform _transform)
    {
        // Damage Enemy
        _transform.GetComponent<AIEnemyStats>().TakeDamage(10.0f); //Wayd HERE TOO

    }

    public void Chain(Transform _transform)
    {
        // Add this transform
        chainTargets.Add(_transform);

        // Damage Enemy
        _transform.GetComponent<AIEnemyStats>().TakeDamage(10.0f); //Wayd HERE TOO

        // Check for nearby                                                 // chain distance
        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, 5.0f);

        // Variables to check closest
        Transform closest = null;
        float closestDistance = float.MaxValue;

        // Check for nearby
        foreach (Collider n in hitColliders)
        {
            // make sure enemy
            if (n.gameObject.CompareTag("Enemy"))
            {
                // find closest
                if (Vector3.Distance(_transform.position, n.transform.position) < closestDistance && !chainTargets.Contains(n.transform))
                {
                    closestDistance = Vector3.Distance(_transform.position, n.transform.position);
                    closest = n.transform;
                }
            }
        }
        // if another target found
        if (closest)
        {
            transform.forward = Vector3.Normalize(closest.position - _transform.position);
        }
        else
        {
            End();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            // temp
            collision.GetComponent<AIEnemyStats>().TakeDamage(damage);

            foreach (OnHitBehaviour n in mods.onHit)
            {
                switch (n)
                {
                    case OnHitBehaviour.Chain:
                    {
                        Chain(collision.transform);
                        break;
                    }
                    case OnHitBehaviour.Pierce:
                    {
                        Pierce(collision.transform);
                        break;
                    }
                    case OnHitBehaviour.Explode:
                    {
                        Explode(collision.transform);
                        break;
                    }
                    default:
                    {
                        End();
                        break;
                    }
                }
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            //Wayd pls thx I dunno how your applying damage to player
        }
    }
}
