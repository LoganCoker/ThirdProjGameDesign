using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    _instance = obj.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    public enum AudioCategory
    {
        Music,
        SFX,
        Ambience,
        UI,
        Footsteps
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public AudioCategory category;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.1f, 3f)]
        public float pitch = 1f;

        [Range(0f, 1f)]
        public float spatialBlend = 0f; 

        public bool loop = false;
        public bool playOnAwake = false;

        [Range(0f, 0.5f)]
        public float randomPitchVariation = 0f;

        [HideInInspector]
        public AudioSource source;
    }

    [Header("Audio Settings")]
    [SerializeField] private Sound[] sounds;

    [Header("Mixer Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup ambienceMixerGroup;
    [SerializeField] private AudioMixerGroup uiMixerGroup;
    [SerializeField] private AudioMixerGroup voiceMixerGroup;
    [SerializeField] private AudioMixerGroup footstepsMixerGroup;

    [Header("Audio Pooling")]
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> sourcePool;

    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float ambienceVolume = 1f;
    private float uiVolume = 1f;
    private float masterVolume = 1f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioPool();

        foreach (Sound sound in sounds)
        {
            SetupSound(sound);

            soundDictionary[sound.name] = sound;
        }
    }

    private void InitializeAudioPool()
    {
        sourcePool = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject sourceObj = new GameObject("AudioSource_" + i);
            sourceObj.transform.SetParent(transform);
            AudioSource source = sourceObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sourcePool.Add(source);
        }
    }

    private void SetupSound(Sound sound)
    {
        if (sound.source == null)
        {
            GameObject sourceObj = new GameObject("Sound_" + sound.name);
            sourceObj.transform.SetParent(transform);
            sound.source = sourceObj.AddComponent<AudioSource>();
        }

        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
        sound.source.spatialBlend = sound.spatialBlend;
        sound.source.playOnAwake = sound.playOnAwake;

        AssignMixerGroup(sound);

        if (sound.playOnAwake)
        {
            sound.source.Play();
        }
    }

    private void AssignMixerGroup(Sound sound)
    {
        if (audioMixer == null) return;

        switch (sound.category)
        {
            case AudioCategory.Music:
                sound.source.outputAudioMixerGroup = musicMixerGroup;
                break;
            case AudioCategory.SFX:
                sound.source.outputAudioMixerGroup = sfxMixerGroup;
                break;
            case AudioCategory.Ambience:
                sound.source.outputAudioMixerGroup = ambienceMixerGroup;
                break;
            case AudioCategory.UI:
                sound.source.outputAudioMixerGroup = uiMixerGroup;
                break;
            case AudioCategory.Footsteps:
                sound.source.outputAudioMixerGroup = footstepsMixerGroup;
                break;
        }
    }

    
    public void Play(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            PlaySound(sound);
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found in AudioManager!");
        }
    }

    
    public void PlayAtPosition(string name, Vector3 position)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            AudioSource source = GetPooledAudioSource();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch + Random.Range(-sound.randomPitchVariation, sound.randomPitchVariation);
            source.spatialBlend = 1f; 
            source.loop = false; 

            source.transform.position = position;

            switch (sound.category)
            {
                case AudioCategory.Music:
                    source.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case AudioCategory.SFX:
                    source.outputAudioMixerGroup = sfxMixerGroup;
                    break;
                case AudioCategory.Ambience:
                    source.outputAudioMixerGroup = ambienceMixerGroup;
                    break;
                case AudioCategory.UI:
                    source.outputAudioMixerGroup = uiMixerGroup;
                    break;
                case AudioCategory.Footsteps:
                    source.outputAudioMixerGroup = footstepsMixerGroup;
                    break;
            }

            source.Play();

            if (sound.clip != null)
            {
                StartCoroutine(ReturnToPoolAfterPlay(source, sound.clip.length));
            }
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found in AudioManager!");
        }
    }

    private void PlaySound(Sound sound)
    {
        if (sound.randomPitchVariation > 0)
        {
            sound.source.pitch = sound.pitch + Random.Range(-sound.randomPitchVariation, sound.randomPitchVariation);
        }

        sound.source.Play();
    }

    
    public void Stop(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found in AudioManager!");
        }
    }

    
    public void FadeIn(string name, float duration)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            StartCoroutine(FadeInCoroutine(sound, duration));
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found in AudioManager!");
        }
    }

    
    public void FadeOut(string name, float duration)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            StartCoroutine(FadeOutCoroutine(sound, duration));
        }
        else
        {
            Debug.LogWarning("Sound " + name + " not found in AudioManager!");
        }
    }

    private IEnumerator FadeInCoroutine(Sound sound, float duration)
    {
        float currentTime = 0;
        float targetVolume = sound.volume;

        sound.source.volume = 0;
        sound.source.Play();

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
            yield return null;
        }

        sound.source.volume = targetVolume;
    }

    private IEnumerator FadeOutCoroutine(Sound sound, float duration)
    {
        float currentTime = 0;
        float startVolume = sound.source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            sound.source.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        sound.source.Stop();
        sound.source.volume = sound.volume; 
    }

    
    public void SetCategoryVolume(AudioCategory category, float volume)
    {
        volume = Mathf.Clamp01(volume);

        switch (category)
        {
            case AudioCategory.Music:
                musicVolume = volume;
                if (audioMixer != null)
                    audioMixer.SetFloat("MusicVolume", ConvertToDecibel(volume));
                break;
            case AudioCategory.SFX:
                sfxVolume = volume;
                if (audioMixer != null)
                    audioMixer.SetFloat("SFXVolume", ConvertToDecibel(volume));
                break;
            case AudioCategory.Ambience:
                ambienceVolume = volume;
                if (audioMixer != null)
                    audioMixer.SetFloat("AmbienceVolume", ConvertToDecibel(volume));
                break;
            case AudioCategory.UI:
                uiVolume = volume;
                if (audioMixer != null)
                    audioMixer.SetFloat("UIVolume", ConvertToDecibel(volume));
                break;
        }

        if (audioMixer == null)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.category == category)
                {
                    sound.source.volume = sound.volume * volume;
                }
            }
        }
    }

    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);

        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", ConvertToDecibel(volume));
        }
        else
        {
            foreach (Sound sound in sounds)
            {
                float categoryVolume = 1f;

                switch (sound.category)
                {
                    case AudioCategory.Music:
                        categoryVolume = musicVolume;
                        break;
                    case AudioCategory.SFX:
                        categoryVolume = sfxVolume;
                        break;
                    case AudioCategory.Ambience:
                        categoryVolume = ambienceVolume;
                        break;
                    case AudioCategory.UI:
                        categoryVolume = uiVolume;
                        break;
                }

                sound.source.volume = sound.volume * categoryVolume * masterVolume;
            }
        }
    }

    
    private float ConvertToDecibel(float linearVolume)
    {
        return linearVolume <= 0 ? -80f : Mathf.Log10(linearVolume) * 20f;
    }

    
    public void AddSound(string name, AudioClip clip, AudioCategory category,
                         float volume = 1f, float pitch = 1f, bool loop = false,
                         float spatialBlend = 0f, bool playOnAwake = false,
                         float randomPitchVariation = 0f)
    {
        if (soundDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Sound with name " + name + " already exists in the AudioManager!");
            return;
        }

        Sound sound = new Sound
        {
            name = name,
            clip = clip,
            category = category,
            volume = volume,
            pitch = pitch,
            loop = loop,
            spatialBlend = spatialBlend,
            playOnAwake = playOnAwake,
            randomPitchVariation = randomPitchVariation
        };

        SetupSound(sound);
        soundDictionary[name] = sound;
    }

    
    private AudioSource GetPooledAudioSource()
    {
        foreach (AudioSource source in sourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        GameObject sourceObj = new GameObject("AudioSource_" + sourcePool.Count);
        sourceObj.transform.SetParent(transform);
        AudioSource newSource = sourceObj.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        sourcePool.Add(newSource);

        return newSource;
    }

    
    private IEnumerator ReturnToPoolAfterPlay(AudioSource source, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        source.clip = null;
        source.volume = 1f;
        source.pitch = 1f;
        source.spatialBlend = 0f;
        source.loop = false;
        source.outputAudioMixerGroup = null;
    }

   
    public void PlayRandom(string[] soundNames)
    {
        if (soundNames.Length == 0) return;

        int randomIndex = Random.Range(0, soundNames.Length);
        Play(soundNames[randomIndex]);
    }

    
    public void PlaySceneTransition(string exitSoundName, string entrySoundName, float crossfadeDuration = 0f)
    {
        GameObject transitionObj = new GameObject("SceneTransitionAudio");
        DontDestroyOnLoad(transitionObj);
        SceneTransitionAudio transitionAudio = transitionObj.AddComponent<SceneTransitionAudio>();

        if (soundDictionary.TryGetValue(exitSoundName, out Sound exitSound) &&
            soundDictionary.TryGetValue(entrySoundName, out Sound entrySound))
        {
            transitionAudio.SetupTransition(exitSound.clip, entrySound.clip, crossfadeDuration);
        }
        else
        {
            Debug.LogWarning("One or both scene transition sounds not found!");
        }
    }
}

public class SceneTransitionAudio : MonoBehaviour
{
    private AudioSource exitSource;
    private AudioSource entrySource;
    private float crossfadeDuration;
    private bool isTransitioning = false;

    void Awake()
    {
        exitSource = gameObject.AddComponent<AudioSource>();
        entrySource = gameObject.AddComponent<AudioSource>();
    }

    public void SetupTransition(AudioClip exitClip, AudioClip entryClip, float fadeTime)
    {
        exitSource.clip = exitClip;
        entrySource.clip = entryClip;
        crossfadeDuration = fadeTime;

        exitSource.Play();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (crossfadeDuration > 0 && !isTransitioning)
        {
            StartCoroutine(CrossfadeAudio());
        }
        else
        {
            exitSource.Stop();
            entrySource.Play();
        }

        StartCoroutine(CleanUpAfterPlay());

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator CrossfadeAudio()
    {
        isTransitioning = true;
        float timer = 0;

        entrySource.volume = 0;
        entrySource.Play();

        while (timer < crossfadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / crossfadeDuration;

            exitSource.volume = Mathf.Lerp(1, 0, t);
            entrySource.volume = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        
        exitSource.Stop();
        entrySource.volume = 1;
        isTransitioning = false;
    }

    private IEnumerator CleanUpAfterPlay()
    {
        
        float waitTime = 0;

        if (exitSource.clip != null && exitSource.isPlaying)
        {
            waitTime = exitSource.clip.length - exitSource.time;
        }

        if (entrySource.clip != null)
        {
            waitTime = Mathf.Max(waitTime, entrySource.clip.length);
        }

        yield return new WaitForSeconds(waitTime + 0.5f); 

        Destroy(gameObject);
    }
}
