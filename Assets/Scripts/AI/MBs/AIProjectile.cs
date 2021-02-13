using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIProjectileBehaviour : UnityEvent<Transform>{}

public class AIProjectile : MonoBehaviour
{

    Rigidbody rb;

    public float speed;
    public float lifetime;

    public AIProjectileBehaviour OverridingBehavior = new AIProjectileBehaviour();
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        OverridingBehavior.AddListener(Default);
    }
    private void FixedUpdate()
    {
        OverridingBehavior.Invoke(transform);
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            End();
        }
    }
    private void End()
    {
        Destroy(gameObject);
    }

    public void Default(Transform _trans)
    {
        rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
         if (collision.gameObject.CompareTag("Player"))
        {
            //Wayd pls thx I dunno how your applying damage to player
            End();
        }
    }
}
