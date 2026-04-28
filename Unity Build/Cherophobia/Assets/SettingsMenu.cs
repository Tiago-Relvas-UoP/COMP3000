using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer masterMix;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetupPrefs();
        masterMix.SetFloat("master", volumeCalc(PlayerPrefs.GetFloat("masterVolume")));
        masterMix.SetFloat("sfx", volumeCalc(PlayerPrefs.GetFloat("sfxVolume")));
        masterMix.SetFloat("music", volumeCalc(PlayerPrefs.GetFloat("musicVolume")));
    }

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

    // Setup prefs on first boot
    private void SetupPrefs()
    {
        // Master Volume
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 1f);
        }

        // SFX Volume
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1f);
        }

        // Music Volume
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f);
        }
    }
}
