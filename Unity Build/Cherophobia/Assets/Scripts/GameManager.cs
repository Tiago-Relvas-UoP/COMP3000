using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public HealthController healthController;
    [SerializeField] public PauseMenu pauseMenu;
    [SerializeField] public AudioMixer audioSFX;
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

    // Start is called before the first frame update
    void Start()
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

        MasterDoor = GameObject.FindGameObjectWithTag("MasterDoor");
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
            pauseMenu.GameOverScreen();
        }
    }
}
