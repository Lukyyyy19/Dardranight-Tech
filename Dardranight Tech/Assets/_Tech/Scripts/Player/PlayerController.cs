using System;
using DG.Tweening;
using UnityEngine;

public class PlayerController : Entity
{
    PlayerData m_playerData;
    private Vector2 m_movementInput;
    private bool m_goingRight;
    private bool m_isMoving;
    

    bool GoingRight
    {
        set
        {
            if (m_goingRight != value && UserInputsManager.Instance.GetMovementInput().x != 0)
            {
                m_goingRight = value;
                OnGoingRightChanged?.Invoke(m_goingRight);
            }
        }
    }

    bool IsMoving
    {
        set
        {
            if (m_isMoving != value)
            {
                m_isMoving = value;
                OnIsMovingChanged?.Invoke(m_isMoving);
            }
        }
    }

    public Action<bool> OnGoingRightChanged;
    public Action<bool> OnIsMovingChanged;
    public Action<PlayerData> OnPlayerDeath;
    public Action<int> OnPlayerLooseHealth;
    public Action<int> OnPlayerScoreChanged;

    protected override void Awake()
    {
        base.Awake();
        m_playerData = new PlayerData();
        m_maxHealth = m_playerData.maxHealth;
        m_health = m_maxHealth;
        m_playerData.health = m_health;
    }

    private void Start()
    {
        GameManager.Instance.OnEnemyDeath += OnEnemyDeath;
    }

    void Update()
    {
        m_movementInput = UserInputsManager.Instance.GetMovementInput();
        GoingRight = m_movementInput.x > 0;
        IsMoving = m_movementInput.x != 0;
        if (UserInputsManager.Instance.ShootInput)
        {
            SoundFXManager.Instance.PlaySound(SoundType.PlayerShoot);
            for (int i = 0; i < m_playerData.bulletQty; i++)
            {
                var x = m_playerData.bulletQty == 1 ? i : i - 1;
                BulletPoolSystem.Instance.GetBullet(transform.position - Vector3.right * 0.3f * x, transform.up,
                    BulletType.PlayerBullet);
            }
        }

        if (m_playerData.m_abilityTimer > 0)
        {
            m_playerData.m_abilityTimer -= Time.deltaTime;
            if (m_playerData.m_abilityTimer <= 0)
            {
                ResetAbilities();
            }
        }
    }

    private void FixedUpdate()
    {
        m_rb.linearVelocity = m_movementInput * m_playerData.speed;
    }

    public override void TakeDamage(int damage)
    {
        // var mainCamera = Camera.main;
        // mainCamera.DOShakePosition(0.25f);
        m_playerData.health -= damage;
        base.TakeDamage(damage);
        OnPlayerLooseHealth?.Invoke(m_health);
    }

    public override void Die()
    {
        base.Die();
        SaveSystem.SaveHighScore();
        OnPlayerDeath?.Invoke(m_playerData);
    }

    void OnEnemyDeath(EnemiesType enemyType)
    {
        switch (enemyType)
        {
            case EnemiesType.BigEnemy:
                m_playerData.score += 10;
                break;
            case EnemiesType.MediumEnemy:
                m_playerData.score += 5;
                break;
            case EnemiesType.SmallEnemy:
                m_playerData.score += 2;
                break;
        }

        OnPlayerScoreChanged?.Invoke(m_playerData.score);
    }

    void ResetAbilities()
    {
        m_playerData.bulletQty = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PowerUp powerUp))
        {
            switch (powerUp.GetAbility())
            {
                case PowerUpAbility.BulletsQTY:
                    m_playerData.bulletQty = 3;
                    m_playerData.m_abilityTimer = m_playerData.m_abilityDuration;
                    break;
            }

            Destroy(other.gameObject);
        }
    }

    public int GetHealth()
    {
        return m_playerData.health;
    }
    public int GetScore()
    {
        return m_playerData.score;
    }
    
    
    public void Save(ref PlayerSaveData data)
    {
        data.clasePlayerData = m_playerData;
        data.position = transform.position;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.position;
        m_playerData = data.clasePlayerData;
        m_health = m_playerData.health;
    }
}
[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 position;
    public PlayerData clasePlayerData;
}