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

    protected override void Awake()
    {
        base.Awake();
        m_playerData = new PlayerData();
    }

    void Update()
    {
        m_movementInput = UserInputsManager.Instance.GetMovementInput();
        // if(transform.position.x > (GameManager.Instance.Width) || transform.position.x < -GameManager.Instance.Width)
        // {
        //     m_movementInput.x = 0;
        // }
        // if(transform.position.y>=GameManager.Instance.Height || transform.position.y <= -GameManager.Instance.Height)
        // {
        //     m_movementInput.y = 0;
        // }
        GoingRight = m_movementInput.x > 0;
        IsMoving = m_movementInput.x != 0;
        if (UserInputsManager.Instance.ShootInput)
        {
            BulletPoolSystem.Instance.GetBullet(transform.position, transform.up,BulletType.PlayerBullet);
        }
        
    }

    private void FixedUpdate()
    {
        m_rb.linearVelocity = m_movementInput * m_playerData.speed;
    }

    public override void TakeDamage(float damage)
    {
        var mainCamera = Camera.main;
        mainCamera.DOShakePosition(0.25f);
        mainCamera.DOShakeRotation(0.25f);
        base.TakeDamage(damage);
    }
}