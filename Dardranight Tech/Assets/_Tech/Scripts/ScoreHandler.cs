using System;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField;
    [SerializeField] ScoreBlock m_scoreText;
    [SerializeField] Transform m_scoreParent;
    
    private void Start()
    {
        m_inputField.onEndEdit.AddListener(OnEndEdit);
        var scores = SaveSystem.LoadHighScore();
        for (int i = 0; i < scores.count; i++)
        {
            var scoreBlock = Instantiate(m_scoreText, m_scoreParent);
            scoreBlock.SetScore(scores.scores[i]);
            scoreBlock.SetPlayerName(scores.names[i]);
        }
    }
    
    void OnEndEdit(string text)
    {
        PlayerPrefs.SetString("PlayerName", text);
    }
}