using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public string playerName;
    public string topPlayerName;
    public int topScore;
    public int selectedFood;
    public float volume;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    [System.Serializable]
    class SaveData
    {
        public string topPlayerName;
        public int topScore;
        public int selectedFood;
        public float volume;
    }

    public void SaveGameData()
    {
        SaveData data = new SaveData();
        data.topPlayerName = topPlayerName;
        data.topScore = topScore;
        data.selectedFood = selectedFood;
        data.volume = GetComponent<AudioSource>().volume;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            topPlayerName = data.topPlayerName;
            topScore = data.topScore;
            volume = data.volume;
            GetComponent<AudioSource>().volume = volume;
            selectedFood = 0;
        }
    }
}
