using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField;
    [SerializeField] ScoreBlock m_scoreText;
    [SerializeField] Transform m_scoreParent;
    HighScoreData m_highScoreData;

    private void Start()
    {
        m_inputField.onEndEdit.AddListener(OnEndEdit);
        m_highScoreData = SaveSystem.LoadHighScore();
        var m_highScoreDictionary = m_highScoreData.names.Zip(m_highScoreData.scores, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
        var orederedScores = m_highScoreDictionary.OrderByDescending(pair => pair.Value)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        for (int i = 0; i < m_highScoreData.count; i++)
        {
            var scoreBlock = Instantiate(m_scoreText, m_scoreParent);
            scoreBlock.SetScore(orederedScores.ElementAt(i).Value);
            scoreBlock.SetPlayerName(orederedScores.ElementAt(i).Key);
        }
    }

    void OnEndEdit(string text)
    {
        PlayerPrefs.SetString("PlayerName", text);
        if (m_highScoreData.names.Contains(text))
        {
        }
    }
}