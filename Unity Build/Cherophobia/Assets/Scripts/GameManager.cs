using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public HealthController healthController;
    [SerializeField] public PauseMenu pauseMenu;
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

    [Header("Is Player Hiding?")]
    [SerializeField] public bool hidingInLocker;

    public int currentFuses;
    public int currentCrowbars;
    public int currentCodes;

    public bool playerEscaped;

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
        if (currentCrowbars > 0) hasCrowbar = true;
        if (currentCodes > 0) hasCode = true;


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
