using UnityEngine;

using System.IO;

public class SaveManager : MonoBehaviour {
    public static SaveManager Instance {
        get; private set;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SaveData data = new() {
            username = "User"
        };

        Save(data);
    }

    string username;

    public void SetUsername(string name) {
        username = name;
    }

    public string GetUsername() {
        return username;
    }

    [System.Serializable]
    public class SaveData {
        public string username;
    }

    public void Save() {
        SaveData data = new() {
            username = username
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/{username}_save.json";
        File.WriteAllText(Application.persistentDataPath + $"/{username}_save.json", json);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Save(string uName) {
        SaveData data = new() {
            username = uName
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/{username}_save.json";
        File.WriteAllText(Application.persistentDataPath + $"/{uName}_save.json", json);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Save(SaveData data) {
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/{username}_save.json";
        File.WriteAllText(path, json);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Load() {
        string path = Application.persistentDataPath + "/deafaultsave.json";
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Debug.Log(path + " has been loaded");
        username = data.username;
    }

    public void Load(string uName) {
        string path = Application.persistentDataPath + $"/{uName}_save.json";

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log(path + " has been loaded");
            username = data.username;
        }
        else {
            Debug.LogError(path + " does not exist, Failed to load data");
            username = "";
        }
    }
}
