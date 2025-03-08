using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager m_instance;
    public static ScreenManager Instance => m_instance;
    public Action OnPause;
    public Action OnResume;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (UserInputsManager.Instance.PauseInput && GameManager.Instance!=null)
        {
            Pause();
        }
    }

    public void Pause()
    {
        OnPause?.Invoke();
        Time.timeScale = 0;
    }

    public void Resume()
    {
        OnResume?.Invoke();
        Time.timeScale = 1;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif PLATFORM_STANDALONE
        Application.Quit();
#endif
    }
}