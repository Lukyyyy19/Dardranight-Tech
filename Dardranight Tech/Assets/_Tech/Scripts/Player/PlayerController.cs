using System;
using UnityEngine;

public class PlayerController : Entity
{
    PlayerData m_playerData;
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
        GoingRight = UserInputsManager.Instance.GetMovementInput().x > 0;
        IsMoving = UserInputsManager.Instance.GetMovementInput().x != 0;
        if (UserInputsManager.Instance.ShootInput)
        {
            BulletPoolSystem.Instance.GetBullet(transform.position, transform.up,BulletType.PlayerBullet);
        }
    }

    private void FixedUpdate()
    {
        m_rb.linearVelocity = UserInputsManager.Instance.GetMovementInput() * m_playerData.speed;
    }
}