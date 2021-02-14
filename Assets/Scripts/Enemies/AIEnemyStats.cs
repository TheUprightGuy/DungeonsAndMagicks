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

    public void TakeDamage(float _damage)
    {
        Health -= _damage;
        if (Health <= 0.0f)
        {
            Die();
        }
    }

    public void Die()
    {
        AIController.Instance.RemoveAgent(this.transform);
        Destroy(this.gameObject);
    }
}
