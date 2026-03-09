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
    [SerializeField] public bool placedFuse;
    [SerializeField] public int removedPlanks;
    [SerializeField] public bool placedCode;

    // Found objects
    [Header("Collectibles")]
    [SerializeField] public bool hasFuse;
    [SerializeField] public bool hasCrowbar;
    [SerializeField] public bool hasCode;

    [Header("Is Player Hiding?")]
    [SerializeField] public bool hidingInLocker;

    private bool _playerEscaped;


    // Start is called before the first frame update
    void Start()
    {
        // Reset time scale
        Time.timeScale = 1f;

        _playerEscaped = false;
        IsMasterUnlocked = false;

        hasFuse = false;
        hasCrowbar = false;
        hasCode = false;

        placedFuse = false;
        removedPlanks = 0;
        placedCode = false;

        MasterDoor = GameObject.FindGameObjectWithTag("MasterDoor");
    }

    // Update is called once per frame
    void Update()
    {
        if (placedFuse && removedPlanks == 3 && placedCode)
        {
            IsMasterUnlocked = true; // Makes Master Door available for interaction
        }

        if (_playerEscaped)
        {
            pauseMenu.WinScreen();
        }

        if (healthController.currentHealth <= 0f)
        {
            pauseMenu.GameOverScreen();
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            _playerEscaped = true;
        }
    }
}
