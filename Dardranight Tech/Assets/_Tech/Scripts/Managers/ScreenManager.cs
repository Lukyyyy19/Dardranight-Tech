using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : SingeltonMonoBehaviourScript<ScreenManager>
{
    public Action OnPause;
    public Action OnResume;
    public Action<bool> OnPlay;
    public Action OnStartGame;
    public Action OnGoBackToMainMenu;
    [SerializeField] private bool m_isPaused;
    protected override void Awake()
    {
        m_dontDestroyOnLoad = true;
        base.Awake();
    }

    private void Update()
    {
        if (UserInputsManager.Instance.PauseInput)
        {
            switch (m_isPaused)
            {
                case true:
                    Resume();
                    break;
                case false:
                    Pause();
                    break;
            }
        }
    }

    public void Pause()
    {
        m_isPaused = true;
        Time.timeScale = 0;
        OnPause?.Invoke();
    }

    public void Resume()
    {
        m_isPaused = false;
        Time.timeScale = 1;
        OnResume?.Invoke();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Resume();
    }

    public void GoBackToMainMenu()
    {
        SaveSystem.Save();
        PlayerPrefs.SetInt("HasSave", 1);
        OnGoBackToMainMenu?.Invoke();
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif PLATFORM_STANDALONE
        Application.Quit();
#endif
    }

    public void PlayGame(bool wantToContinue)
    {
        if (!wantToContinue)
        {
            PlayerPrefs.SetInt("HasSave", 0);
        }

        OnStartGame?.Invoke();
        ChangeScene("SampleScene");
    }

    public void StartGame()
    {
        var hasSave = PlayerPrefs.GetInt("HasSave") == 1;
        OnPlay?.Invoke(hasSave);
    }
}