using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Handles behaviour of buttons/sliders in settings menu.
public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer masterMix;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    // Start is called before the first frame update
    // Set AudioMixer sliders based on the appropriate PlayerPrefs values
    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    // Saves volume settings locally, and in the PlayerPrefs.
    public void Save() 
    {
        // Save Prefs
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);

        // Set AudioMixer values
        masterMix.SetFloat("master", volumeCalc(masterSlider.value));
        masterMix.SetFloat("sfx", volumeCalc(sfxSlider.value));
        masterMix.SetFloat("music", volumeCalc(musicSlider.value)); 

        Debug.Log("New Volumes: Master (" + masterSlider.value + "), SFX (" + sfxSlider.value + "), Music (" + musicSlider.value + ")");
    }

    // Calculation needed for the logarithmic scale of the volume slider, as the AudioMixer uses a logarithmic scale for volume control
    public float volumeCalc(float volume) 
    { 
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
    }
}