using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenManager : MonoBehaviour
{
    private static ScreenManager m_instance;
    public static ScreenManager Instance => m_instance;
    
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
    }
    
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}