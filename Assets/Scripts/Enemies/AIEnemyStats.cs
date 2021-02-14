using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyStats : MonoBehaviour
{
    public float Health;

    [HideInInspector]
    public float MaxHealth;

    private void Start()
    {
        MaxHealth = Health;
    }

    private void Update()
    {
        if (Health <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
    public void TakeDamage(float _damage)
    {
        Health -= _damage;
    }
}
