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
        if(!GameManager.Instance)return;
        m_pausePanel.SetActive(true);
    }

    void OnResume()
    {
        if(!GameManager.Instance)return;
        m_pausePanel.SetActive(false);
    }

    private void OnDisable()
    {
        ScreenManager.Instance.OnPause -= OnPause;
        ScreenManager.Instance.OnResume -= OnResume;
    }

    public void UpdateScore(int score)
    {
        m_scoreText.text = score.ToString();
    }

    public void UpdateHealth(int health)
    {
        m_healthText.text = health.ToString();
    }

    public void SetMasterVolume(float volume)
    {
        SoundFXManager.Instance.SetMasterVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        SoundFXManager.Instance.SetMusicVolume(volume);
    }
    public void SetSoundFxVolume(float volume)
    {
        SoundFXManager.Instance.SetSFXVolume(volume);
    }
    

    #region Scene Management Buttons
    public void ReturnToMainMenu()
    {
        ScreenManager.Instance.GoBackToMainMenu();
    }

    public void ReturnToMainMenuFromGameOver()
    {
        ScreenManager.Instance.ChangeScene("Main");
    }

    public void Resume()
    {
        ScreenManager.Instance.Resume();
    }

    public void StartGame()
    {
        ScreenManager.Instance.StartGame();
    }

    public void PlayGame(bool continueGame)
    {
        ScreenManager.Instance.PlayGame(continueGame);
    }
    
    public void QuitGame()
    {
        ScreenManager.Instance.QuitGame();
    }
    #endregion
}