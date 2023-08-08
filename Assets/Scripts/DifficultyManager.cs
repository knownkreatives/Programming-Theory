using UnityEngine;

public class DifficultyManager : MonoBehaviour {
    public static DifficultyManager Instance {
        get; private set;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        } Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    int levelDificulty = 1;
    public void SetDifficulty(int value) { // ENCAPSULATION
        levelDificulty = value;
    }

    public int GetDifficulty() { // ENCAPSULATION
        return levelDificulty;
    }
}