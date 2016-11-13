using UnityEngine;
using System.Collections;
using System;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public int startingHealth;
    protected int health;

    public event Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }

        Destroy(gameObject);
    }
}
