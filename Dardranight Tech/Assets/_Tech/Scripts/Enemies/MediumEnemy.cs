using UnityEngine;

public class MediumEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        m_maxHealth = 3;
        m_health = m_maxHealth;
    }
}
