using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    Player pc;
    float speed = 10.0f;
    public int amount = 0;

    private void Update()
    {
        if (pc)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, pc.transform.position, step);

            if (Vector3.Distance(transform.position, pc.transform.position) < 0.01f)
            {
                // Give Player Gold - Spawn FX?
                pc.GiveGold(amount);
                // Swap the position of the cylinder.
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            pc = other.GetComponent<Player>();
        }
    }
}
