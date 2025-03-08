using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    static GameManager m_instance;
    public static GameManager Instance => m_instance;
    private bool m_canSpawn = true;
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
    [SerializeField] EnemiesPool m_enemiesPool;

    public Action<EnemiesType> OnEnemyDeath;
    [SerializeField]PlayerController m_playerController;
    [SerializeField] UIManager m_uiManager;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }

        m_enemiesPool.IntantiateEnemies((int)EnemiesType.BigEnemy, m_enemyPrefab[0]);
        m_enemiesPool.IntantiateEnemies((int)EnemiesType.MediumEnemy, m_enemyPrefab[1]);
        m_enemiesPool.IntantiateEnemies((int)EnemiesType.SmallEnemy, m_enemyPrefab[2]);
    }

    private void Start()
    {
        m_playerController.OnPlayerDeath += OnplayerDeath;
        m_playerController.OnPlayerLooseHealth += PlayerLooseHealth;
        m_playerController.OnPlayerScoreChanged += PlayerScoreChanged;
    }


    private void Update()
    {
        if (!m_canSpawn) return;
        m_spawnTimer += Time.deltaTime;
        if (m_spawnTimer >= m_spawnRate && m_enemyCount < m_maxEnemyCount)
        {
            SpawnEnemy();
            m_spawnTimer = 0;
        }
    }

    void PlayerLooseHealth(int health)
    {
        m_uiManager.UpdateHealth(health);
    }

    void PlayerScoreChanged(int score)
    {
        m_uiManager.UpdateScore(score);
    }

    private void OnplayerDeath(PlayerData playerData)
    {
        m_enemyCount = 0;
        m_canSpawn = false;
        PlayerPrefs.SetInt("CurrentScore", playerData.score);
        ScreenManager.Instance.Pause();
        ScreenManager.Instance.ChangeScene("GameOver");
    }

    void SpawnEnemy()
    {
        var spawnPos = new Vector3(UnityEngine.Random.Range(-m_width, m_width), UnityEngine.Random.Range(3, 5.5f), 0);
        Enemy enemy = m_enemiesPool.Get(Random.Range(0, 3), spawnPos);
        // Enemy enemy = Instantiate(m_enemyPrefab[0],
        //     new Vector3(UnityEngine.Random.Range(-m_width, m_width), UnityEngine.Random.Range(3, 5.5f), 0),
        //     Quaternion.identity);
        enemy.OnDeath += () =>
        {
            m_enemyCount--;
            OnEnemyDeath?.Invoke(enemy.EnemyType);
        };
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