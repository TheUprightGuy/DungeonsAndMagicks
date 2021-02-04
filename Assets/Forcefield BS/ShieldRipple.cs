using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRipple : MonoBehaviour
{
    public GameObject rippleVFX;

    private Material mat;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            var ripples = Instantiate(rippleVFX, transform) as GameObject;
            var psr = ripples.transform.GetComponent<ParticleSystemRenderer>();
            mat = psr.material;
            mat.SetVector("_SphereCenter", collision.contacts[0].point);

            Destroy(ripples, 2);
        }
    }
}
