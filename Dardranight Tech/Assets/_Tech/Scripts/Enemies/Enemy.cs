using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] EnemyVisual m_enemyVisual;
    protected float m_timeToMove;
    [SerializeField] protected float m_timeToMoveCounter;

    protected float m_timeToShoot = 1.85f;
    [SerializeField] protected float m_timeToShootCounter;


    protected float m_direction = 1;
    protected float m_speed = 3;
    protected bool m_canMove;
    Coroutine m_hitEffect;

    public Action OnDeath;

    protected virtual void Update()
    {
        m_timeToMoveCounter -= Time.deltaTime;
        if (m_timeToMoveCounter <= 0)
        {
            m_canMove = false;
            m_timeToMove = UnityEngine.Random.Range(1f, 3f);
            m_direction = UnityEngine.Random.Range(-1f, 1f);
            m_timeToMoveCounter = m_timeToMove;
        }

        m_timeToShootCounter -= Time.deltaTime;
        if (m_timeToShootCounter <= 0)
        {
            m_timeToShootCounter = m_timeToShoot;
            Shoot();
        }
        
        if(transform.position.x > (GameManager.Instance.Width) || transform.position.x < -GameManager.Instance.Width)
        {
            m_direction *= -1;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (m_timeToMoveCounter > 0)
        {
            m_rb.linearVelocity = new Vector2(m_direction, 0) * m_speed;
        }
    }

    protected void Shoot()
    {
        BulletPoolSystem.Instance.GetBullet((Vector2)transform.position - Vector2.up, -transform.up,
            BulletType.EnemyBullet);
    }

    public override void TakeDamage(float damage)
    {
        m_enemyVisual.StartHitEffect();
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        OnDeath?.Invoke();
        base.Die();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            m_direction *= -1;
        }
    }
}