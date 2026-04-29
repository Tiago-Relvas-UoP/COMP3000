using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public HealthController healthController;
    [SerializeField] public PauseMenu pauseMenu;
    [SerializeField] public AudioMixer audioSFX;
    [SerializeField] public GameObject blackBackground;
    [SerializeField] public GameObject[] backgroundMusic;
    [SerializeField] public AudioMixer masterMix;
    private GameObject MasterDoor;
    private GameObject hidingSpots;

    
    // Master Door
    [Header("Master Door")]
    [SerializeField] public bool IsMasterUnlocked;
    [SerializeField] public int placedFuses;
    [SerializeField] public int removedPlanks;
    [SerializeField] public bool placedCode;

    // Found objects
    [Header("Collectibles")]
    [SerializeField] public bool hasFuse;
    [SerializeField] public bool hasCrowbar;
    [SerializeField] public bool hasCode;

    [Header("Game Interactions")]
    [SerializeField] public bool hidingInLocker;
    [SerializeField] public bool IsKeypadBeingUsed;

    [Header("Items")]
    [SerializeField] public int currentFuses;
    [SerializeField] public int currentCrowbars;
    [SerializeField] public int currentCodes; // not used

    [Header("Has player Escaped?")]
    [SerializeField] public bool playerEscaped;

    [Header("Inside Happiness Zone")]
    [SerializeField] public bool insideTrap;

    public bool _gameOver;
    private GameObject _enemy;

    // Game Over Countdowns
    public float _firstCountdown;
    public float _secondCountdown;

    // Start is called before the first frame update
    void Start()
    {
        SetupFlags();
        SetVolume();

        // Ensures enemy is active at start of the game
        _enemy = GameObject.FindGameObjectWithTag("MainEnemy");
        _enemy.SetActive(true);

        blackBackground.SetActive(false);

        MasterDoor = GameObject.FindGameObjectWithTag("MasterDoor");

        // Set-up Player Pref related to last known player death cause
        if (!PlayerPrefs.HasKey("lastDeathCause"))
        {
            PlayerPrefs.SetFloat("lastDeathCause", 0f); // 0f = By Enemy/Mimic, 1f = By Proximity Trap, 2f = By Trigger Trap
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFuses > 0) hasFuse = true;
        else if (currentFuses <= 0) hasFuse = false;

        if (currentCrowbars > 0) hasCrowbar = true;
        else if (currentCrowbars <= 0) hasCrowbar = false;

        
        switch (Time.timeScale) 
        {
            case 1f:
                AudioListener.pause = false;
                break;

            case 0f:
                AudioListener.pause = true;
                break;
        }


        if (placedFuses == 3 && removedPlanks == 3 && placedCode)
        {
            IsMasterUnlocked = true; // Makes Master Door available for interaction
        }

        if (playerEscaped)
        {
            pauseMenu.WinScreen();
        }

        if (healthController.currentHealth <= 0f)
        {
            _firstCountdown += Time.deltaTime;
            Debug.Log("Cause of death:" + PlayerPrefs.GetFloat("lastDeathCause"));
            blackBackground.SetActive(true);

            for (int i = 0; i < backgroundMusic.Length; i++)
            {
                backgroundMusic[i].SetActive(false);
            }

            Debug.Log("Countdown " + _firstCountdown);
            if (_firstCountdown >= 1f)
            {
                AudioListener.pause = true;

                if (_firstCountdown >= 3f)
                {
                    SceneManager.LoadScene("GameOver");
                }
            }
        }
    }

    private void SetupFlags() 
    {
        // Reset time scale
        Time.timeScale = 1f;

        playerEscaped = false;
        IsMasterUnlocked = false;

        // No items
        hasFuse = false;
        hasCrowbar = false;
        hasCode = false;

        placedFuses = 0;
        removedPlanks = 0;
        placedCode = false;

        _gameOver = false;
        _firstCountdown = 0f;
        _secondCountdown = 0f;
    }

    private void SetVolume() 
    {
        masterMix.SetFloat("master", volumeCalc(PlayerPrefs.GetFloat("masterVolume")));
        masterMix.SetFloat("sfx", volumeCalc(PlayerPrefs.GetFloat("sfxVolume")));
        masterMix.SetFloat("music", volumeCalc(PlayerPrefs.GetFloat("musicVolume")));
    }

    // Calculation needed for the logarithmic scale of the volume slider, as the AudioMixer uses a logarithmic scale for volume control
    public float volumeCalc(float volume)
    {
        return Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
    }
}
