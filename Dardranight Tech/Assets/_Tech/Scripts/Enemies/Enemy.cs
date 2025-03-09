using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Entity
{
    int m_id;
    Action<int, Enemy> notify;
    [SerializeField] EnemyVisual m_enemyVisual;
    protected float m_timeToMove;
    [SerializeField] protected float m_timeToMoveCounter;

    protected float m_timeToShoot = 1.85f;
    [SerializeField] protected float m_timeToShootCounter;


    protected float m_direction = 1;
    protected float m_speed = 3;
    protected bool m_canMove;
    Coroutine m_hitEffect;

    public Action<Enemy> OnDeath;

    [SerializeField] PowerUp m_powerUp;
    public EnemiesType EnemyType => (EnemiesType)m_id;
    public void Configure(int id, Action<int, Enemy> enemy)
    {
        m_id = id;
        notify = enemy;
    }

    protected override void Awake()
    {
        base.Awake();
        InitCounters();
    }
    
    public void SetTimeToShoot(float time)
    {
        m_timeToShoot = time;
    }

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

        if (transform.position.x > (GameManager.Instance.Width) || transform.position.x < -GameManager.Instance.Width)
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

    public override void TakeDamage(int damage)
    {
        if (m_isDead) return;
        m_enemyVisual.StartHitEffect();
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        OnDeath?.Invoke(this);
        var probability = Random.Range(0, 100);
        if (probability < 15)
        {
            Instantiate(m_powerUp, transform.position, Quaternion.identity);
        }
        base.Die();
        notify.Invoke(m_id, this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            m_direction *= -1;
        }
    }

    public void TurnOn()
    {
        m_enemyVisual.ResetVisuals();
        m_health = m_maxHealth;
        m_isDead = false;
        InitCounters();
    }

    private void InitCounters()
    {
        m_timeToMove = UnityEngine.Random.Range(1f, 3f);
        m_timeToMoveCounter = m_timeToMove;
        m_timeToShootCounter = m_timeToShoot;
    }

    public void TurnOff()
    {
        m_enemyVisual.ResetVisuals();
    }

    private void OnDisable()
    {
        OnDeath = null;
    }
}