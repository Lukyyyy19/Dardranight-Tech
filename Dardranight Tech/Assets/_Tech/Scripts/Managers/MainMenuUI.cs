using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]private GameObject m_newGamePanel;
    [SerializeField]private GameObject m_wantToContinuePanel;

    private void Start()
    {
        ScreenManager.Instance.OnPlay += ShowPanels;
    }

    private void OnDisable()
    {
        ScreenManager.Instance.OnPlay -= ShowPanels;
    }

    void ShowPanels(bool hasSave)
    {
        m_newGamePanel.SetActive(!hasSave);
        m_wantToContinuePanel.SetActive(hasSave);
    }

}