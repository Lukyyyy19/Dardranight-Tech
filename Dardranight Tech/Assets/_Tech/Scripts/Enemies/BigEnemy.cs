using UnityEngine;

public class BigEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        m_maxHealth = 5;
        m_health = m_maxHealth;
    }
}
