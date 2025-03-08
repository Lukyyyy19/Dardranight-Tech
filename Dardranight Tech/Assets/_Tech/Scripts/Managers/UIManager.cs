using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject m_pausePanel;

    private void Start()
    {
        ScreenManager.Instance.OnPause += OnPause;
    }

    void OnPause()
    {
        m_pausePanel.SetActive(true);
    }

    private void OnDisable()
    {
        ScreenManager.Instance.OnPause -= OnPause;
    }
}
