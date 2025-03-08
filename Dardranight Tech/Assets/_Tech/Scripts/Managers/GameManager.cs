using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager m_instance;
    public static GameManager Instance => m_instance;
    [SerializeField] Enemy[] m_enemyPrefab;
    float m_spawnRate = 1;
    float m_spawnTimer = 0;
    [SerializeField] int m_enemyCount = 0;
    int m_maxEnemyCount = 10;

    [SerializeField] private float m_width;
    [SerializeField] private float m_height;
    [SerializeField] Vector2 m_offset;

    public float Width => m_width + m_offset.x;
    public float Height => m_height + m_offset.y;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }

    private void Update()
    {
        m_spawnTimer += Time.deltaTime;
        if (m_spawnTimer >= m_spawnRate && m_enemyCount < m_maxEnemyCount)
        {
            SpawnEnemy();
            m_spawnTimer = 0;
        }
    }

    void SpawnEnemy()
    {
        Enemy enemy = Instantiate(m_enemyPrefab[0],
            new Vector3(UnityEngine.Random.Range(-m_width, m_width), UnityEngine.Random.Range(3, 5.5f), 0),
            Quaternion.identity);
        enemy.OnDeath += () => m_enemyCount--;
        m_enemyCount++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-m_width, -m_height, 0) + (Vector3)m_offset,
            new Vector3(m_width, -m_height, 0) + (Vector3)m_offset);
        Gizmos.DrawLine(new Vector3(-m_width, m_height, 0) + (Vector3)m_offset,
            new Vector3(m_width, m_height, 0) + (Vector3)m_offset);
        Gizmos.DrawLine(new Vector3(-m_width, -m_height, 0) + (Vector3)m_offset,
            new Vector3(-m_width, m_height, 0) + (Vector3)m_offset);
        Gizmos.DrawLine(new Vector3(m_width, -m_height, 0) + (Vector3)m_offset,
            new Vector3(m_width, m_height, 0) + (Vector3)m_offset);
    }
}