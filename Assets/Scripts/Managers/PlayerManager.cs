using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager Instance {
        get; private set;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        } Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    int playerCar = 0;
    public void SetPlayerCar(int value) { // ENCAPSULATION
        playerCar = value;
    }

    public int GetPlayerCar() { // ENCAPSULATION
        return playerCar;
    }
}