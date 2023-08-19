using System.Collections;
using UnityEngine;
using TMPro;

public enum PowerupType {
    None, Movement
}

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
        if (DifficultyManager.Instance != null)
            Instantiate(playerCars [PlayerManager.Instance.GetPlayerCar()], playerCars [PlayerManager.Instance.GetPlayerCar()].transform.position, playerCars [PlayerManager.Instance.GetPlayerCar()].transform.rotation, transform);
        else
            Instantiate(playerCars [0], playerCars [0].transform.position, playerCars [0].transform.rotation, transform);

        StartCoroutine(StartGame());
    }

    void Update() {
        UpdateLives(); // ABSTRACTION
        UpdateScore(); // ABSTRACTION
    }

    // Start game data
    float gameSpeed = 0;
    [SerializeField] GameObject[] playerCars;
    bool isGameOver = false, hasGameStarted = false;
    [SerializeField] GameObject gameoverScreen;
    public void SetGameSpeed(float value) { // ENCAPSULATION
        gameSpeed = value;
    }
    public float GetGameSpeed() { // ENCAPSULATION
        return gameSpeed;
    }

    static float startGameSpeed;

    public void UpdateGameSpeed(float factor) {
        SetGameSpeed(startGameSpeed * factor); // ABSTRACTION
    }

    public void ResetGameSpeed() {
        SetGameSpeed(startGameSpeed);
    }

    public void SetGameState(bool value) { // ENCAPSULATION
        isGameOver = value;
    }
    public bool GetGameState() { // ENCAPSULATION
        return isGameOver && hasGameStarted;
    }

    [SerializeField] float gameDelay = 1;
    IEnumerator StartGame() {
        yield return new WaitForSeconds(LoadManager.Instance.transitionTime + gameDelay);
        hasGameStarted = true;
        InitGame();
    }

    public void GoToMenu() {
        SaveData(); // ABSTRACTION

        LoadManager.Instance.LoadScene(0); // ABSTRACTION
    }

    public void RestartLevel() {
        SaveData(); // ABSTRACTION

        LoadManager.Instance.LoadScene(1); // ABSTRACTION
    }

    void SaveData() {
        if (SaveManager.Instance != null) {
            SaveManager.Instance.SetBestTime(time); // ABSTRACTION
            SaveManager.Instance.SetHighScore(score); // ABSTRACTION
            SaveManager.Instance.UpdateGamesPlayed(); // ABSTRACTION
        }
    }

    void InitGame() {
        if (DifficultyManager.Instance != null)
            SetGameSpeed(DifficultyManager.Instance.GetDifficulty()); // ABSTRACTION
        else
            SetGameSpeed(1);

        startGameSpeed = GetGameSpeed();

        StartCoroutine(SpawnObstacles(true)); // ABSTRACTION 
        StartCoroutine(SpawnPowerups(true)); // ABSTRACTION 
        StartCoroutine(UpdateTime()); // ABSTRACTION

        actualObstacleSpawnTime = obstacleSpawnTime;
        actualPowerupSpawnTime = powerupSpawnTime;
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
        else {
            livesText.text = "Lives: " + GetLives();
        }
    }

    float col_time;
    public void MovePlayer() {
        if (!GetGameState()) {
            col_time = Time.time;
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && CheckCooldown()) { // ABSTRACTION
                ChangeLaneNum(-1); // ABSTRACTION
                UpdateCooldown(); // ABSTRACTION
            }

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && CheckCooldown()) { // ABSTRACTION
                ChangeLaneNum(1);
                UpdateCooldown(); // ABSTRACTION
            }
        }
    }
    float cooldownTime = 0.2f;
    float remaningTime = 0;

    void UpdateCooldown() {
        remaningTime = col_time + cooldownTime;
    }

    bool CheckCooldown() {
        return remaningTime < col_time;
    }
    // End player handler

    // Start obstacle & powerup spawner
    [SerializeField] GameObject[] obstacles;
    [SerializeField] Vector3 obstacleOffset;
    [SerializeField][Min(0.1f)] float obstacleSpawnTime;
    [SerializeField][Min(1)] float obstacleDifficultyFactor;
    float actualObstacleSpawnTime;

    IEnumerator SpawnObstacles(bool isContinued) {
        if (isContinued && !GetGameState()) {
            Vector3 obstaclePos = RandomLanePosition(1, 4, obstacleOffset);

            if (DifficultyManager.Instance != null)
                yield return new WaitForSeconds(actualObstacleSpawnTime / DifficultyManager.Instance.GetDifficulty());
            else
                yield return new WaitForSeconds(actualObstacleSpawnTime);

            int index = RandomObjectIndex(obstacles);
            Instantiate(obstacles [index], obstaclePos, obstacles [index].transform.rotation, transform); // ABSTRACTION

            actualObstacleSpawnTime /= obstacleDifficultyFactor;
            StartCoroutine(SpawnObstacles(isContinued));
        }
        else {
            yield break;
        }
    }

    [SerializeField] GameObject [] powerups;
    [SerializeField] Vector3 powerupOffset;
    [SerializeField][Min(0.1f)] float powerupSpawnTime;
    [SerializeField][Min(1)] float powerupDifficultyFactor;
    [SerializeField] GameObject indicator;
    float actualPowerupSpawnTime;
    PowerupType powerupType = PowerupType.None;

    public void SetPowerupType(PowerupType type) {
        powerupType = type;
    }
    public PowerupType GetPowerupType() {
        return powerupType;
    }

    IEnumerator SpawnPowerups(bool isContinued) {
        if (isContinued && !GetGameState()) {
            Vector3 powerupPos = RandomLanePosition(1, 4, powerupOffset);

            if (DifficultyManager.Instance != null)
                yield return new WaitForSeconds(actualPowerupSpawnTime / DifficultyManager.Instance.GetDifficulty());
            else
                yield return new WaitForSeconds(actualPowerupSpawnTime);

            int index = RandomObjectIndex(powerups);
            Instantiate(powerups [index], powerupPos, powerups [index].transform.rotation, transform); // ABSTRACTION

            actualPowerupSpawnTime /= powerupDifficultyFactor;
            StartCoroutine(SpawnPowerups(isContinued));
        }
        else {
            yield break;
        }
    }

    Vector3 RandomLanePosition(int min, int max, Vector3 offset) {
        Vector3 spawnPos = new Vector3();
        int rand = Random.Range(min, max);

        spawnPos = GetLanePostition(rand) + offset; // ABSTRACTION

        return spawnPos;
    }

    public void SetIndicatorVisibility(bool value) {
        indicator.SetActive(value);
    }

    int RandomObjectIndex(GameObject [] objects) {
        int index = Random.Range(0, objects.Length);

        return index;
    }
    // End obstacle & powerup spawner

    // Start lane handler
    [SerializeField] Vector3 lane1Position, lane2Position, lane3Position;
    int playerLane = 2;
    public int GetPlayerLane() { // ENCAPSULATION
        return playerLane;
    }

    Vector3 GetLanePostition(int value) { // ENCAPSULATION
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
    public void SetTranformToLane(Transform _transform, Vector3 pos1, Vector3 pos2, Vector3 pos3) {
        if (playerLane == 1) {
            SetPosition(_transform, pos1); // ABSTRACTION
        }
        else if (playerLane == 2) {
            SetPosition(_transform, pos2); // ABSTRACTION
        } 
        else if (playerLane == 3) {
            SetPosition(_transform, pos3);
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
        Gizmos.DrawLine(obstacleOffset + new Vector3(lane1Position.x, 0, 0), obstacleOffset + new Vector3(lane3Position.x, 0, 0));
    }
    // End lane handler

    // Start time & score handler
    int time = 0;
    int score = 0;
    [SerializeField] TextMeshProUGUI scoreText, timeText;
    public void SetTime(int value) { // ENCAPSULATION
        time = value;
    }

    public int GetTime() { // ENCAPSULATION
        return time;
    }

    IEnumerator UpdateTime() {
        if (!GetGameState()) { // ABSTRACTION
            yield return new WaitForSeconds(1);
            SetTime(1 + GetTime()); // ABSTRACTION
            StartCoroutine(UpdateTime()); // ABSTRACTION

            timeText.text = $"Time: {GetTime()}";
        }
    }

    public void SetScore(int value) { // ENCAPSULATION
        score = value;
    }

    public int GetScore() { // ENCAPSULATION
        return score;
    }

    public void AddScore(int value) { // ENCAPSULATION
        SetScore(GetScore() + value);
    }

    public void UpdateScore() {
        scoreText.text = $"Score: {GetScore()}";
    }
    // End time & score handler

    // Start UI manager
    public void PlayAudioClip(string clipName) {
        AudioManager.Instance.PlayAudioClip(clipName);
    }
    // End UI manager
}
