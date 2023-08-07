using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour {
    public TextMeshProUGUI currentUser;
    public TMP_InputField saveUsernameField;

    void Start() {
        LoadMenuData();
    }

    private void LoadMenuData() {
        SaveManager.Instance.Load();
        Debug.Log("Load");
        SetCurrentUserText();
    }
    
    public void LoadMenuData(string uName) {
        SaveManager.Instance.Load(uName);
        SetCurrentUserText();
    }
    public void LoadMenuData(TMP_InputField uName) {
        SaveManager.Instance.Load(uName.text);
        SetCurrentUserText();
    }

    public void SaveMenuData() {
        SaveManager.Instance.SetUsername(saveUsernameField.text);
        SaveManager.Instance.Save();
    }
    
    public void SetCurrentUserText() {
        currentUser.text = "Hello " + SaveManager.Instance.GetUsername() + ".";
    }

    public void Play(int difficulty) {
        DifficultyManager.Instance.SetDifficulty(difficulty);
        SceneManager.LoadScene(1);
    }

    public void Exit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        SaveMenuData();
    }
}
