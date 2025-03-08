using UnityEngine;

public class SmallEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        m_maxHealth = 1;
        m_health = m_maxHealth;
        m_speed = 6;
    }
}
