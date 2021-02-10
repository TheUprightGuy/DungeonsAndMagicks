using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float hp;

    public void TakeDamage(float _damage)
    {
        hp -= _damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        TutorialTracking.instance.CheckQuest(this.gameObject);
        Destroy(this.gameObject);
    }
}
