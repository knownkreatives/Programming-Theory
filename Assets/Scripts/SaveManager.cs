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
        } Instance = this;

        DontDestroyOnLoad(gameObject);

        SaveData data = new() {
            username = "User"
        }; Save(data); // ABSTRACTION
    }

    // Start username handler
    string username;
    public void SetUsername(string name) { // ENCAPSULATION
        username = name;
    }

    public string GetUsername() { // ENCAPSULATION
        return username;
    }
    // End username handler

    // Start general save & load methods
    [System.Serializable]
    public class SaveData {
        public string username;
    }

    public void Save() {
        SaveData data = new() {
            username = GetUsername()  // ABSTRACTION
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/{GetUsername()}_save.json";  // ABSTRACTION
        File.WriteAllText(path, json);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Save(string uName) {
        SaveData data = new() {
            username = uName
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/{GetUsername()}_save.json";  // ABSTRACTION
        File.WriteAllText(path, json);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Save(SaveData data) {
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + $"/{GetUsername()}_save.json";  // ABSTRACTION
        File.WriteAllText(path, json);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Load() {
        string path = Application.persistentDataPath + "/deafaultsave.json";
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Debug.Log(path + " has been loaded");
        SetUsername(data.username);  // ABSTRACTION
    }

    public void Load(string uName) {
        string path = Application.persistentDataPath + $"/{uName}_save.json";

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log(path + " has been loaded");
            SetUsername(data.username); // ABSTRACTION
        }
        else {
            Debug.LogError("Failed to load data because " + path + " does not exist.");
            SetUsername(""); // ABSTRACTIONq
        }
    }
    // End general save & load methods
}
