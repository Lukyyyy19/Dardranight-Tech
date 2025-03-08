using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int m_id;
    private bool m_move;
    [SerializeField] private float m_speed = 10;
    [SerializeField] private float m_lifeTime = 2;
    public float timer { get; set; }

    private Vector2 m_direction;
    private Action<int, Bullet> notify;

    public void Configure(int id, Action<int, Bullet> bullet)
    {
        m_id = id;
        notify = bullet;
    }

    private void Update()
    {
        if (!m_move) return;
        timer += Time.deltaTime;
        if (timer >= m_lifeTime)
        {
            TimeCompleted();
        }

        transform.Translate(m_direction * m_speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(1);
        }

        TimeCompleted();
    }

    public void Move(Vector2 pos, Vector2 dir)
    {
        transform.position = pos;
        m_direction = dir.normalized;
    }

    protected virtual void TimeCompleted()
    {
        timer = 0;
        ResetStats();
    }

    protected virtual void ResetStats()
    {
        timer = 0;
        notify.Invoke(m_id, this);
    }

    public void TurnOn()
    {
        m_move = true;
    }

    public void TurnOff()
    {
        m_move = false;
    }
}