using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Audio;


public enum SoundType
{
    PlayerShoot,
    PlayerHit,
    EnemyShoot,
    EnemyHit,
    PowerUp,
    Explosion,
    GameOver
}

public class SoundFXManager : SingeltonMonoBehaviourScript<SoundFXManager>
{
    [SerializeField] AudioSource m_musicAudioSource;
    [SerializeField] AudioSource m_sfxAudioSourcePrefab;
    [SerializeField] SoundsList[] m_soundsList;
    [SerializeField] AudioMixer m_audioMixer;

    [SerializeField] private AudioClip[] m_musicClips;

    protected override void Awake()
    {
        m_dontDestroyOnLoad = true;
        base.Awake();
    }

    private void Start()
    {
        ScreenManager.Instance.OnStartGame += OnStartGame;
        ScreenManager.Instance.OnGoBackToMainMenu += OnGoBackToMainMenu;
        SetMasterVolumeDirectly(PlayerPrefs.GetFloat("MasterVolume", 0.75f));
        SetMusicVolumeDirectly(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        SetSFXVolumeDirectly(PlayerPrefs.GetFloat("SFXVolume", 0.75f));
    }

    public void PlaySound(SoundType soundType)
    {
        AudioClip[] clips = m_soundsList[(int)soundType].clips;
        AudioClip clip = clips[0];
        var audioSource = Instantiate(m_sfxAudioSourcePrefab);
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource.gameObject, clip.length);
    }

    public void PlayMusic(AudioClip clip)
    {
        m_musicAudioSource.clip = clip;
        m_musicAudioSource.Play();
    }

    void OnStartGame()
    {
        PlayMusic(m_musicClips[1]);
    }

    void OnGoBackToMainMenu()
    {
        PlayMusic(m_musicClips[0]);
    }

    public void SetMasterVolume(float volume)
    {
        var newVolume = Mathf.Log10(volume) * 20;
        PlayerPrefs.SetFloat("MasterVolume", newVolume);
        m_audioMixer.SetFloat("Master", newVolume);
    }
    public void SetMasterVolumeDirectly(float volume)
    {
        m_audioMixer.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        var newVolume = Mathf.Log10(volume) * 20;
        PlayerPrefs.SetFloat("MusicVolume", newVolume);
        m_audioMixer.SetFloat("MusicFX", newVolume);
    }
    
    void SetMusicVolumeDirectly(float volume)
    {
        m_audioMixer.SetFloat("MusicFX", volume);
    }

    public void SetSFXVolume(float volume)
    {
        var newVolume = Mathf.Log10(volume) * 20;
        PlayerPrefs.SetFloat("SFXVolume", newVolume);
        m_audioMixer.SetFloat("SoundFX", newVolume);
    }
    
    void SetSFXVolumeDirectly(float volume)
    {
        m_audioMixer.SetFloat("SoundFX", volume);
    }
    
}

[Serializable]
public struct SoundsList
{
    public SoundType name;
    public AudioClip[] clips;
}