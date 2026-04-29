using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public AudioMixer masterMix;

    private float _countdown;
    private bool _startGame;

    private void Start()
    {
        SetVolume();
        _countdown = 0.0f;
    }

    private void Update()
    {
        if (_startGame) 
        {
            _countdown += Time.deltaTime;
        }

        if (_countdown >= 9f) SceneManager.LoadScene("SampleScene");
    }

    public void PlayGame() 
    {
        _startGame = true;
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    private void SetVolume()
    {
        SetupPrefs();
        masterMix.SetFloat("master", volumeCalc(PlayerPrefs.GetFloat("masterVolume")));
        masterMix.SetFloat("sfx", volumeCalc(PlayerPrefs.GetFloat("sfxVolume")));
        masterMix.SetFloat("music", volumeCalc(PlayerPrefs.GetFloat("musicVolume")));
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

    // Calculation needed for the logarithmic scale of the volume slider, as the AudioMixer uses a logarithmic scale for volume control
    public float volumeCalc(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
    }
}
