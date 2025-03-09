using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider m_masterVolume;
    [SerializeField] private Slider m_SFXVolume;
    [SerializeField] private Slider m_musicVolume;

    private void OnEnable()
    {
        var masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1);
        m_masterVolume.value = Mathf.Pow(10, masterVolume / 20);
        var sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        m_SFXVolume.value = Mathf.Pow(10, sfxVolume / 20);
        var musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1);
        m_musicVolume.value = Mathf.Pow(10, musicVolume / 20);
    }
}