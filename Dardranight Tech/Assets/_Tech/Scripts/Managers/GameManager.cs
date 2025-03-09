using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingeltonMonoBehaviourScript<GameManager>
{
    private bool m_canSpawn = true;
    [SerializeField] Enemy[] m_enemyPrefab;
    [SerializeField] PowerUp m_healthPowerUpPrefab;
    private int scoreToSpawnHealth = 500;
    float m_spawnRate = 1;
    float m_spawnTimer = 0;
    [SerializeField] int m_enemyCount = 0;
    int m_maxEnemyCount = 10;
    List<Enemy> m_enemies = new List<Enemy>();

    [SerializeField] private float m_width;
    [SerializeField] private float m_height;
    [SerializeField] Vector2 m_offset;

    public float Width => m_width + m_offset.x;
    public float Height => m_height + m_offset.y;
    [SerializeField] EnemiesPool m_enemiesPool;

    public Action<EnemiesType> OnEnemyDeath;
    public Action OnGameOver;
    [SerializeField] PlayerController m_playerController;
    [SerializeField] UIManager m_uiManager;
    public PlayerController PlayerController => m_playerController;

    protected override void Awake()
    {
        base.Awake();

        m_enemiesPool.IntantiateEnemies((int)EnemiesType.BigEnemy, m_enemyPrefab[0]);
        m_enemiesPool.IntantiateEnemies((int)EnemiesType.MediumEnemy, m_enemyPrefab[1]);
        m_enemiesPool.IntantiateEnemies((int)EnemiesType.SmallEnemy, m_enemyPrefab[2]);

        if (PlayerPrefs.GetInt("HasSave") == 1)
        {
            SaveSystem.Load();
            m_uiManager.UpdateHealth(m_playerController.GetHealth());
            m_uiManager.UpdateScore(m_playerController.GetScore());
        }
    }

    private void Start()
    {
        m_playerController.OnPlayerDeath += OnplayerDeath;
        m_playerController.OnPlayerHealthChanged += PlayerHealthChanged;
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

    void PlayerHealthChanged(int health)
    {
        m_uiManager.UpdateHealth(health);
    }

    void PlayerScoreChanged(int score)
    {
        m_uiManager.UpdateScore(score);
        PlayerPrefs.SetInt("CurrentScore", score);
        if (m_playerController.GetScore() >= scoreToSpawnHealth)
        {
            Instantiate(m_healthPowerUpPrefab, new Vector3(Random.Range(-m_width, m_width), Random.Range(3, 5.5f), 0),
                Quaternion.identity);
            scoreToSpawnHealth += 500;
        }
    }

    private void OnplayerDeath(PlayerData playerData)
    {
        m_enemyCount = 0;
        m_canSpawn = false;
        //SaveSystem.Save();
        OnGameOver?.Invoke();
        ScreenManager.Instance.Resume();
        ScreenManager.Instance.ChangeScene("GameOver");
        PlayerPrefs.SetInt("HasSave", 0);
    }

    void SpawnEnemy()
    {
        var spawnPos = new Vector3(UnityEngine.Random.Range(-m_width, m_width), UnityEngine.Random.Range(3, 5.5f), 0);
        Enemy enemy = m_enemiesPool.Get(Random.Range(0, 3), spawnPos);
        enemy.OnDeath += (enemy) =>
        {
            m_enemyCount--;
            OnEnemyDeath?.Invoke(enemy.EnemyType);
            m_enemies.Remove(enemy);
        };
        m_enemyCount++;
        m_enemies.Add(enemy);
    }

    public void SaveScore(ref HighScoreData highScoreData)
    {
        if (highScoreData.scores == null)
            highScoreData.scores = new List<int>();
        if (highScoreData.names == null)
            highScoreData.names = new List<string>();

        if (highScoreData.names.Contains(PlayerPrefs.GetString("PlayerName")))
        {
            if (m_playerController.GetScore() >
                highScoreData.scores[highScoreData.names.IndexOf(PlayerPrefs.GetString("PlayerName"))])
            {
                highScoreData.scores[highScoreData.names.IndexOf(PlayerPrefs.GetString("PlayerName"))] =
                    m_playerController.GetScore();
            }

            return;
        }

        highScoreData.count++;
        highScoreData.scores.Add(m_playerController.GetScore());
        highScoreData.names.Add(PlayerPrefs.GetString("PlayerName"));
    }


    public void SaveGameData(ref GameData gameData)
    {
        gameData.enemyCount = m_enemyCount;
        gameData.positions = m_enemies.Select(x => x.transform.position).ToList();
        gameData.types = m_enemies.Select(x => x.EnemyType).ToList();
    }

    public void LoadGameData(GameData gameData)
    {
        m_enemyCount = gameData.enemyCount;
        for (int i = 0; i < gameData.enemyCount; i++)
        {
            var enemy = /*Instantiate(m_enemyPrefab[(int)gameData.types[i]],
                gameData.positions[i], Quaternion.identity);*/
                m_enemiesPool.Get((int)gameData.types[i], gameData.positions[i]);
            enemy.SetTimeToShoot(Random.Range(1f, 3f));
            enemy.OnDeath += (enemy) =>
            {
                m_enemyCount--;
                OnEnemyDeath?.Invoke(enemy.EnemyType);
                m_enemies.Remove(enemy);
            };
            m_enemies.Add(enemy);
        }
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

[System.Serializable]
public struct GameData
{
    public int enemyCount;
    public List<Vector3> positions;
    public List<EnemiesType> types;
}

[Serializable]
public struct HighScoreData
{
    public int count;
    public List<string> names;
    public List<int> scores;
}