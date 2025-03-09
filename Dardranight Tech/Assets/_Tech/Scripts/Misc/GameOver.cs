using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI m_scoreText;
    private int m_score;
    private void Start()
    {
        m_scoreText.text = PlayerPrefs.GetInt("CurrentScore").ToString();
    }
}
