using TMPro;
using UnityEngine;

public class ScoreBlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_playerNameText;
    
    
    public void SetScore(int score)
    {
        m_scoreText.text = score.ToString();
    }
    
    public void SetPlayerName(string playerName)
    {
        m_playerNameText.text = playerName;
    }
}