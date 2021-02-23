using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEffect : MonoBehaviour
{
    private Vector3 vel = Vector3.up;
    private Rigidbody rb;

    private void Awake()
    {
        vel *= Random.Range(4.0f, 6.0f);
        vel += new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));

        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void Update()
    {
        rb.position += vel * Time.deltaTime;

        Quaternion delRot = Quaternion.Euler(new Vector3(Random.Range(-150.0f, 150.0f), Random.Range(150.0f, 250.0f), Random.Range(-150.0f, 150.0f)) * Time.deltaTime);
        rb.MoveRotation(rb.rotation * delRot);

        vel.y = (vel.y < -4.0f) ? -4.0f : vel.y - 5.0f * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            if (hit.distance <= 0.1f)
            {
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
                Destroy(this);
            }
        }
    }
}
