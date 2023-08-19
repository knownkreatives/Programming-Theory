using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI currentUserMainMenuText;
    [SerializeField] TextMeshProUGUI currentUserSaveAndLoadText;
    [SerializeField] TextMeshProUGUI bestTimeAndHighscoreText;
    [SerializeField] TextMeshProUGUI numberOfGamesPlayedText;
    [SerializeField] TMP_InputField saveUsernameField;
    [SerializeField] CanvasGroup infoScreen;
    [SerializeField] TextMeshProUGUI infoText;

    void Start() {
        LoadMenuData(); // ABSTRACTION
    }

    public void LoadMenuData() {
        SaveManager.Instance.Load();
        InitMenu(); // ABSTRACTION
    }
    public void LoadMenuData(string uName) {
        if (string.IsNullOrEmpty(uName)) {
            infoScreen.alpha = Mathf.Lerp(1, 0, 10f);
            GameError.Error(0);
            return;
        }
        else {
            SaveManager.Instance.Load(uName);
            InitMenu(); // ABSTRACTION
        }
    }
    public void LoadMenuData(TMP_InputField uName) {
        if (string.IsNullOrEmpty(uName.text)) {
            infoScreen.alpha = Mathf.Lerp(1, 0, 10f);
            infoText.text = GameError.Error(0);
            return;
        }
        else {
            SaveManager.Instance.Load(uName.text);
            InitMenu(); // ABSTRACTION
        }
    }

    public void SaveMenuData() {
        if (!string.IsNullOrEmpty(saveUsernameField.text)) {
            SaveManager.Instance.SetUsername(saveUsernameField.text); // ENCAPSULATION
            SaveManager.Instance.Save(); // ABSTRACTION
        }
    }

    public void PlayAudioClip(string clipName) {
        AudioManager.Instance.PlayAudioClip(clipName);
    }

    void InitMenu() {


        SetCurrentUserText(); // ABSTRACTION
        SetBestTimeAndHighScoreText(); // ABSTRACTION
        SetNumberOfGamesPlayedTexts(); // ABSTRACTION
    }

    void SetCurrentUserText() {
        string uName = SaveManager.Instance.GetUsername(); // ABSTRACTION
        if (uName == SaveManager.SaveData.Default.Username) {
            currentUserMainMenuText.text = "Hello!!!";
            currentUserSaveAndLoadText.text = "Make your own user to play with";
        }
        else {
            currentUserMainMenuText.text = currentUserSaveAndLoadText.text = $"Hello <color=#A4C0CC>{uName}</color>";
        }
    }

    void SetBestTimeAndHighScoreText() {
        string uName = SaveManager.Instance.GetUsername(); // ABSTRACTION
        if (uName == SaveManager.SaveData.Default.Username) {
            bestTimeAndHighscoreText.text = SaveManager.Instance.HasPlayedBefore() ? $"Your's Stats:\n Best Time = <color=#35B3E3>{SaveManager.Instance.GetBestTime()}s</color>, Highscore = <color=#E3CF35>{SaveManager.Instance.GetHighScore()}</color>" : "*Press <color=#CF4E45>Continue</color> to play the game and get your stats*";
        }
        else {
            bestTimeAndHighscoreText.text = SaveManager.Instance.HasPlayedBefore() ? $"{uName}'s Stats:\n Best Time = <color=#35B3E3>{SaveManager.Instance.GetBestTime()}s</color>, Highscore = <color=#E3CF35>{SaveManager.Instance.GetHighScore()}</color>" : "*Press <color=#35E390>Continue</color> to play the game and get your stats*";
        }
    }

    void SetNumberOfGamesPlayedTexts() {
        int numGames = SaveManager.Instance.GetNumOfGames(); // ABSTRACTION
        if (numGames == 0) {
            numberOfGamesPlayedText.text = "Welcome, continue and play your fist game!";
        }
        else if (numGames == 1) {
            numberOfGamesPlayedText.text = "Wow you have realy outdone yourself and played your fist game!! Continue on and reach the higest score ever!!";
        }
        else {
            numberOfGamesPlayedText.text = $"You have now played {numGames} games!!! You can do it!!!";
        }
    }

    public void Play(int difficulty) {
        DifficultyManager.Instance.SetDifficulty(difficulty); // ABSTRACTION
        SaveManager.Instance.UpdateGamesPlayed(); // ABSTRACTION
        LoadManager.Instance.LoadScene(1); // ABSTRACTION
    }

    public void Exit() {
        SaveMenuData(); // ABSTRACTION

        LoadManager.Instance.Exit(); // ABSTRACTION
    }
}
