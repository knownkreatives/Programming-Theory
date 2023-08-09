using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance {
        get; private set;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        } Instance = this;
    }

    void Start() {
        SetGameSpeed(DifficultyManager.Instance.GetDifficulty()); // ABSTRACTION
        StartCoroutine(SpawnObstacles(true)); // ABSTRACTION 
        StartCoroutine(UpdateTime()); // ABSTRACTION

        actualSpawnTime = spawnTime;
    }

    void Update() {
        UpdateLives(); // ABSTRACTION
    }

    void FixedUpdate() {
        livesText.text = "Lives: " + GetLives();
        timeText.text = "Time: " + GetTime();
    }

    void LateUpdate() {
        if (Input.GetKey(boostKey1) || Input.GetKey(boostKey2)) {
            SetGameSpeed(DifficultyManager.Instance.GetDifficulty() * 5); // ABSTRACTION
            isBoosted = true;
        }
        else {
            SetGameSpeed(DifficultyManager.Instance.GetDifficulty()); // ABSTRACTION
            isBoosted = false;
        } // boost
    }

    // Start game data
    float gameSpeed = 1;
    bool isBoosted = false, isGameOver = false;
    [SerializeField] GameObject gameoverScreen;
    [SerializeField] KeyCode boostKey1 = KeyCode.W, boostKey2 = KeyCode.UpArrow;
    public void SetGameSpeed(float value) { // ENCAPSULATION
        gameSpeed = value;
    }
    public float GetGameSpeed() { // ENCAPSULATION
        return gameSpeed;
    }

    public void SetGameState(bool value) { // ENCAPSULATION
        isGameOver = value;
    }
    public bool GetGameState() { // ENCAPSULATION
        return isGameOver;
    }

    public void GoToMenu() {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // End game data

    // Start player handler
    [SerializeField][Min(1)] int playerLives;
    [SerializeField] TextMeshProUGUI livesText;
    public void SetLives(int value) { // ENCAPSULATION
        playerLives = value;
    }
    public int GetLives() { // ENCAPSULATION
        return playerLives;
    }

    public void LoseLive() {
        playerLives--;
    }
    public void LoseLive(int value) {
        playerLives -= value;
    }

    public void GainLive() {
        playerLives++;
    }
    public void GainLive(int value) {
        playerLives += value;
    }

    public void UpdateLives() {
        if (GetLives() <= 0) {
            SetGameState(true); // ABSTRACTION
            gameoverScreen.SetActive(true);
        }
    }
    // End player handler

    // Start obstacle spawner
    [SerializeField] GameObject[] obstacles;
    [SerializeField] Vector3 offset;
    [SerializeField][Min(0.1f)] float spawnTime;
    [SerializeField][Min(1)] float difficultyFactor;
    float actualSpawnTime;
    IEnumerator SpawnObstacles(bool isContinued) {
        if (isContinued && !GetGameState()) {
            yield return new WaitForSeconds(actualSpawnTime / DifficultyManager.Instance.GetDifficulty());

            int index = RandomLaneIndex();
            Instantiate(obstacles [index], RandomLanePosition(), obstacles [index].transform.rotation, transform); // ABSTRACTION

            actualSpawnTime /= difficultyFactor;
            SetGameSpeed(GetGameSpeed() * difficultyFactor); // ABSTRACTION
            StartCoroutine(SpawnObstacles(isContinued));

        }
        else {
            yield return null;
        }
    }

    Vector3 RandomLanePosition(int min = 1, int max = 4) {
        Vector3 spawnPos = new Vector3();
        int rand = Random.Range(min, max);

        spawnPos = GetPlayerLanePostition(rand) + offset; // ABSTRACTION

        return spawnPos;
    }

    int RandomLaneIndex() {
        int index = Random.Range(0, obstacles.Length);

        return index;
    }
    // End obstacle spawner

    // Start lane handler
    [SerializeField] Vector3 lane1Position, lane2Position, lane3Position;
    int playerLane = 2;
    public int GetPlayerLane() { // ENCAPSULATION
        return playerLane;
    }

    Vector3 GetPlayerLanePostition(int value) { // ENCAPSULATION
        Vector3 lanePos = new Vector3();

        if (value == 1) {
            lanePos = lane1Position;
        }
        else if (value == 2) {
            lanePos = lane2Position;
        }
        else if (value == 3) {
            lanePos = lane3Position;
        }

        return lanePos;
    }

    public void ChangeLaneNum(int operation) {
        if (CheckLaneValidity(operation)) { // ABSTRACTION
            playerLane += operation;
        }
    }

    bool CheckLaneValidity(int operation) {
        return !((playerLane + operation == 4) || (playerLane + operation == 0));
    }

    public void SetTranformToLane(Transform _transform) {
        if (playerLane == 1) {
            SetPosition(_transform, lane1Position); // ABSTRACTION
        }
        else if (playerLane == 2) {
            SetPosition(_transform, lane2Position); // ABSTRACTION
        } 
        else if (playerLane == 3) {
            SetPosition(_transform, lane3Position);
        }
    }

    void SetPosition(Transform _transform, Vector3 _position) {
        _transform.position = _position;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = (GetPlayerLane() == 1) ? Color.green : Color.red; // ABSTRACTION
        Gizmos.DrawLine(lane1Position, lane1Position + new Vector3(0, 0, 80));

        Gizmos.color = (GetPlayerLane() == 2) ? Color.green : Color.red; // ABSTRACTION
        Gizmos.DrawLine(lane2Position, lane2Position + new Vector3(0, 0, 80));

        Gizmos.color = (GetPlayerLane() == 3) ? Color.green : Color.red; // ABSTRACTION
        Gizmos.DrawLine(lane3Position, lane3Position + new Vector3(0, 0, 80));

        Gizmos.color = Color.black;
        Gizmos.DrawLine(offset + new Vector3(lane1Position.x, 0, 0), offset + new Vector3(lane3Position.x, 0, 0));
    }
    // End lane handler

    // Start time & score handler
    int time = 0;
    [SerializeField] TextMeshProUGUI scoreText, timeText;
    public void SetTime(int value) { // ENCAPSULATION
        time = value;
    }

    public int GetTime() { // ENCAPSULATION
        return time;
    }

    IEnumerator UpdateTime() {
        if (!GetGameState()) { // ABSTRACTION
            yield return new WaitForSeconds(1 * (isBoosted ? 1/2.5f : 1));
            SetTime(1 + GetTime()); // ABSTRACTION
            StartCoroutine(UpdateTime()); // ABSTRACTION
        }
    }
    // End time & score handler
}
