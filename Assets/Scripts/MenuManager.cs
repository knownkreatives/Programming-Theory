using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI currentUser;
    [SerializeField] TextMeshProUGUI textInfo;
    [SerializeField] TMP_InputField saveUsernameField;

    void Start() {
        LoadMenuData(); // ABSTRACTION
    }

    private void LoadMenuData() {
        SaveManager.Instance.Load();
        textInfo.text = "*loaded " + SaveManager.Instance.GetUsername();
        SetCurrentUserText(); // ABSTRACTION
    }
    public void LoadMenuData(string uName) {
        SaveManager.Instance.Load(uName);
        textInfo.text = "*loaded " + uName;
        SetCurrentUserText(); // ABSTRACTION
    }
    public void LoadMenuData(TMP_InputField uName) {
        SaveManager.Instance.Load(uName.text);
        textInfo.text = "*loaded " + uName.text;
        SetCurrentUserText(); // ABSTRACTION
    }

    public void SaveMenuData() {
        SaveManager.Instance.SetUsername(saveUsernameField.text); // ENCAPSULATION
        textInfo.text = "*saved " + SaveManager.Instance.GetUsername(); // ABSTRACTION
        SaveManager.Instance.Save(); // ABSTRACTION
    }
    
    public void SetCurrentUserText() {
        currentUser.text = "Hello " + SaveManager.Instance.GetUsername() + "."; // ABSTRACTION
    }

    public void Play(int difficulty) {
        DifficultyManager.Instance.SetDifficulty(difficulty); // ABSTRACTION
        SceneManager.LoadScene(1);
    }

    public void Exit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        SaveMenuData(); // ABSTRACTION
    }
}
