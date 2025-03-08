using System;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI m_scoreText;
    private int m_score;
    private void Start()
    {
        PlayerPrefs.GetInt("CurrentScore", m_score);
    }
}
