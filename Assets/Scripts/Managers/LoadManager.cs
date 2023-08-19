using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadManager : MonoBehaviour {
    public static LoadManager Instance {
        get; private set;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] Animator transition;
    [Min(0.1f)] public float transitionTime = 1;
    [SerializeField] string transitionTriggerStart = "Start", transitionTriggerEnd = "End";

    void Start() {
        transitionTriggerStart ??= "Start";
        transitionTriggerEnd ??= "End";
    }

    public void LoadScene(int scene) {
        StartCoroutine(Load(scene));
    }

    IEnumerator Load(int scene) {
        transition.SetTrigger(transitionTriggerStart);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(scene);
        transition.SetTrigger(transitionTriggerEnd);
    }

    public void Exit() {
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame() {
        transition.SetTrigger(transitionTriggerStart);
        yield return new WaitForSeconds(transitionTime);

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
