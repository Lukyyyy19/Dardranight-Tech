using System;
using UnityEngine;
public enum BulletType
{
    PlayerBullet,
    EnemyBullet
}
public class BulletPoolSystem : MonoBehaviour
{
    static BulletPoolSystem m_instance;
    public static BulletPoolSystem Instance => m_instance;
    [SerializeField] private BulletPool m_bulletPool;
    [SerializeField] private Bullet[] m_bulletPrefab;
    private BulletType id;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }

        m_bulletPool.IntantiateBullets((int)BulletType.PlayerBullet, m_bulletPrefab[0]);
        m_bulletPool.IntantiateBullets((int)BulletType.EnemyBullet, m_bulletPrefab[1]);
    }

    public Bullet GetBullet(Vector3 pos, Vector3 dir, BulletType id)
    {
        return m_bulletPool.Get((int)id, pos, dir);
    }
}