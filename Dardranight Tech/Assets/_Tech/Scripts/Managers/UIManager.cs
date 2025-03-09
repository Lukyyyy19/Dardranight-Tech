using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject m_pausePanel;
    [SerializeField] TextMeshProUGUI m_scoreText;
    [SerializeField] TextMeshProUGUI m_healthText;

    private void Start()
    {
        ScreenManager.Instance.OnPause += OnPause;
        ScreenManager.Instance.OnResume += OnResume;
    }

    void OnPause()
    {
        m_pausePanel.SetActive(true);
    }

    void OnResume()
    {
        m_pausePanel.SetActive(false);
    }

    private void OnDisable()
    {
        ScreenManager.Instance.OnPause -= OnPause;
    }

    public void UpdateScore(int score)
    {
        m_scoreText.text = score.ToString();
    }

    public void UpdateHealth(int health)
    {
        m_healthText.text = health.ToString();
    }
}