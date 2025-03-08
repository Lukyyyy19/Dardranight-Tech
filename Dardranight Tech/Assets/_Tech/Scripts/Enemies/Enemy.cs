using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] EnemyVisual m_enemyVisual;
    private float m_timeToMove;
    [SerializeField] private float m_timeToMoveCounter;

    private float m_timeToShoot = 1.85f;
    [SerializeField] private float m_timeToShootCounter;


    float direction = 1;
    private bool m_canMove;
    Coroutine m_hitEffect;

    public Action OnDeath;

    private void Update()
    {
        m_timeToMoveCounter -= Time.deltaTime;
        if (m_timeToMoveCounter <= 0)
        {
            m_canMove = false;
            m_timeToMove = UnityEngine.Random.Range(1f, 3f);
            direction = UnityEngine.Random.Range(-1f, 1f);
            m_timeToMoveCounter = m_timeToMove;
        }

        m_timeToShootCounter -= Time.deltaTime;
        if (m_timeToShootCounter <= 0)
        {
            m_timeToShootCounter = m_timeToShoot;
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (m_timeToMoveCounter > 0)
        {
            m_rb.linearVelocity = new Vector2(direction, 0) * 3;
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
}