using UnityEngine;
using System.IO;
using System;

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
    }

    // Start username handler
    string Username;
    public void SetUsername(string name) { // ENCAPSULATION
        if (!string.IsNullOrEmpty(name))
            Username = name;
    }

    public string GetUsername() { // ENCAPSULATION
        return Username;
    }
    // End username handler

    // Start best time handler
    int BestTime;
    public void SetBestTime(int time) { // ENCAPSULATION
        if (time != 0 && time > 0)
            BestTime = BestTime > time ? BestTime : time;
    }

    public int GetBestTime() { // ENCAPSULATION
        return BestTime;
    }
    // End best time handler

    // Start high score handler
    int HighScore;
    public void SetHighScore(int score) { // ENCAPSULATION
        if (score != 0 && score > 0)
            HighScore = HighScore > score ? HighScore : score;
    }

    public int GetHighScore() { // ENCAPSULATION
        return HighScore;
    }
    // End high score handler

    // Start player data
    int NumOfGames;
    public void SetNumOfGames(int value) { // ENCAPSULATION
        if (value != 0 && value > 0)
            NumOfGames = NumOfGames > value ? NumOfGames : value;
    }

    public int GetNumOfGames() { // ENCAPSULATION
        return NumOfGames;
    }

    public void UpdateGamesPlayed() {
        SetNumOfGames(GetNumOfGames() + 1);
    }

    public bool HasPlayedBefore() {
        return NumOfGames > 0;
    }
    // End player data

    // Start general save & load methods

    [System.Serializable]
    public class SaveData {
        public string Username;
        public int BestTime;
        public int HighScore;
        public int NumOfGames;

        public static readonly SaveData Default = new() {
            Username = "User_",
            BestTime = 0,
            HighScore = 0,
            NumOfGames = 0
        };

        public string ToJson() {
            return JsonUtility.ToJson(this);
        }
    }

    public void Save() {
        SaveData data = new() {
            Username = GetUsername(),  // ABSTRACTION
            BestTime = GetBestTime(),  // ABSTRACTION
            HighScore = GetHighScore(),  // ABSTRACTION
            NumOfGames = GetNumOfGames()  // ABSTRACTION
        };

        string json = data.ToJson();
        string path = Application.persistentDataPath + $"/{GetUsername().ToLower()}_save.json";  // ABSTRACTION

        File.WriteAllText(path, json);
        File.SetAttributes(path, FileAttributes.Normal);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }
    public void Save(string uName) {
        uName ??= "Player";

        SaveData data = new() {
            Username = uName,
            BestTime = GetBestTime(),  // ABSTRACTION
            HighScore = GetHighScore(),  // ABSTRACTION
            NumOfGames = GetNumOfGames()  // ABSTRACTION
        };

        string json = data.ToJson();
        string path = Application.persistentDataPath + $"/{GetUsername().ToLower()}_save.json";  // ABSTRACTION

        File.WriteAllText(path, json);
        File.SetAttributes(path, FileAttributes.Normal);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }
    public void Save(SaveData data) {
        string json = data.ToJson();
        string path = Application.persistentDataPath + $"/{GetUsername().ToLower()}_save.json";  // ABSTRACTION

        File.WriteAllText(path, json);
        File.SetAttributes(path, FileAttributes.Normal);

        Debug.Log("Data has been saveed at " + path + ", with the data" + json);
    }

    public void Load() {
        string json = "";
        string path = Application.persistentDataPath;

        SaveData data = new() { };

        if (Directory.GetFiles(path) != null) {
            string latestFilePath = "";
            DateTime latestFileAccesDate = new();

            foreach (string file in Directory.GetFiles(path)) {
                DateTime fileAccesDate = Directory.GetLastAccessTime(file);

                if (fileAccesDate > latestFileAccesDate) {
                    latestFileAccesDate = fileAccesDate;
                    latestFilePath = file;
                }
            }

            if (!string.IsNullOrEmpty(latestFilePath)) {
                json = File.ReadAllText(latestFilePath);
                data = JsonUtility.FromJson<SaveData>(json);

                Debug.Log($"{latestFilePath} has been loaded");
            }
            else {
                DefaultLoad();
            }
        }
        else {
            DefaultLoad();
        }

        UpdateData(data);

    }
    public void Load(string uName) {
        string path = Application.persistentDataPath + $"/{uName.ToLower()}_save.json";

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            if (json != null && json != "{}") {
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                UpdateData(data);

                Debug.Log($"{path} has been loaded");
                return;
            }
            else {
                SaveData data = SaveData.Default;
                UpdateData(data);
            }
        }
        else {
            UpdateData(new() {
                Username = uName,
                HighScore = 0,
                BestTime = 0,
                NumOfGames = 0
            });
        }
    }

    void DefaultLoad() {
        SaveData data = SaveData.Default;

        string json = data.ToJson();
        string path = Application.persistentDataPath + "/_defaultsave.json";  // ABSTRACTION

        File.WriteAllText(path, json);
        File.SetAttributes(path, FileAttributes.Normal);

        Debug.Log("Default path has been loaded");
    }

    void UpdateData(SaveData data) {
        SetUsername(data.Username); // ABSTRACTION
        Debug.Log(data.Username);
        SetBestTime(data.BestTime); // ABSTRACTION
        Debug.Log(data.BestTime);
        SetHighScore(data.HighScore); // ABSTRACTION
        Debug.Log(data.HighScore);
        SetNumOfGames(data.NumOfGames); // ABSTRACTION
        Debug.Log(data.NumOfGames);
    }
    // End general save & load methods
}
