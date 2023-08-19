using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance {
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

    AudioSource audioSource;

    [SerializeField] List<AudioClip> audioClips;
    static Dictionary<string, AudioClip> namedAudioClips = new();

    void Start() {
        audioSource = GetComponent<AudioSource>();

        foreach (var clip in audioClips) {
            namedAudioClips.Add(clip.name, clip);
        }
    }

    public void PlayAudioClip(string clipName) {
        try {
            audioSource.clip = namedAudioClips [clipName];
            audioSource.Play();
            return;
        }
        catch {
            audioSource.clip = audioClips [0];
            audioSource.Play();
        }
    }
}
