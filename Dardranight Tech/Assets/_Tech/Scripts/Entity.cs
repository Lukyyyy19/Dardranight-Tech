using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] protected float m_health;
    protected float m_maxHealth = 4;
    [SerializeField] protected Rigidbody2D m_rb;
    protected bool m_isDead;

    protected virtual void Awake()
    {
        m_health = m_maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        m_isDead = true;
        gameObject.SetActive(false);
    }
}