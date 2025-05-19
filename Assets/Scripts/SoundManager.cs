using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Sounds {
    Walking,
    Ambient,
    Attack,
}

public class SoundCollection {
    private AudioClip[] clips;
    private int lastClipIndex;

    public SoundCollection(params string[] clipNames) {
        this.clips = new AudioClip[clipNames.Length];
        for (int i = 0; i < clips.Length; i++) {
            clips[i] = Resources.Load<AudioClip>(clipNames[i]);
            if (clips[i] == null) {
                Debug.Log($"can't find audio clip {clipNames[i]}");
            }
        }
        lastClipIndex = -1;
    }

    public AudioClip GetRandClip() {
        if (clips.Length == 0) {
            Debug.Log("No clips to give");
            return null;
        } else if (clips.Length == 1) {
            return clips[0];
        } else {
            int index = lastClipIndex;
            while (index == lastClipIndex) {
                index = Random.Range(0, clips.Length);
            }
            lastClipIndex = index;
            return clips[index];
        }
    }

}

public class SoundManager : MonoBehaviour {
    [SerializeField] Slider volumeSlider;

    public float mainVolume = 1.0f;
    private Dictionary<Sounds, SoundCollection> sounds;
    private AudioSource audioSrc;

    public static SoundManager Instance { get; private set; }

    void Awake() {
        Instance = this;
        audioSrc = GetComponent<AudioSource>();
        sounds = new() { 
            { Sounds.Walking, new() }, 
        };
    }

    public void Start(){
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else{
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load(){
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");  
    }

    private void Save(){
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }


    public void Play(Sounds type, AudioSource audioSrc = null)
    {
        if (sounds.ContainsKey(type))
        {
            audioSrc ??= this.audioSrc;
            audioSrc.volume = Random.Range(0.70f, 1.0f) * mainVolume;
            audioSrc.pitch = Random.Range(0.75f, 1.25f);
            audioSrc.clip = sounds[type].GetRandClip();
            audioSrc.Play();
        }
    }
}
