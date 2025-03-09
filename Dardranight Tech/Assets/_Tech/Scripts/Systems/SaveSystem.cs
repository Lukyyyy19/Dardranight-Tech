using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static SaveData m_saveData = new SaveData();
    [System.Serializable]
    public struct SaveData
    {
        public PlayerSaveData playerData;
        public GameData gameData;
        public HighScoreData highScoreData;
    }
    


    public static string SaveFileName()
    {
        var saveFileName = Application.persistentDataPath + "/saveData.save";
        return saveFileName;
    }
    
    public static string HighScoreFileName()
    {
        var highScoreFileName = Application.persistentDataPath + "/highScoreData.save";
        return highScoreFileName;
    }

    public static void SaveHighScore()
    {
        GameManager.Instance.SaveScore(ref m_saveData.highScoreData);
        File.WriteAllText(HighScoreFileName(), JsonUtility.ToJson(m_saveData.highScoreData,true));
        
    }
    
    public static HighScoreData LoadHighScore()
    {
        m_saveData.highScoreData = JsonUtility.FromJson<HighScoreData>(File.ReadAllText(HighScoreFileName()));
        return m_saveData.highScoreData;
    }
    public static void Save()
    {
        GameManager.Instance.PlayerController.Save(ref m_saveData.playerData);
        GameManager.Instance.SaveGameData(ref m_saveData.gameData);
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(m_saveData,true));
    }
    
    public static void Load()
    {
        if (File.Exists(SaveFileName()))
        {
            m_saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(SaveFileName()));
            Debug.Log(m_saveData.playerData.position);
            GameManager.Instance.PlayerController.Load(m_saveData.playerData);
            GameManager.Instance.LoadGameData(m_saveData.gameData);
        }
    }
}