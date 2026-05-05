using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the behaviour of multiple buttons in the pause menu, aswell as Game Over/Win screens.
public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject gameOverMenu;
    [SerializeField] public GameObject winScreen;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject settingsMenu;
    [SerializeField] public static bool isPaused;

    private GameObject _generalAudio;
    private GameObject _enemyAudio;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winScreen.SetActive(false);

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _generalAudio = GameObject.FindGameObjectWithTag("AudioObjects");
        _enemyAudio = GameObject.FindGameObjectWithTag("EnemyAudioObject");
    }

    // Update is called once per frame
    void Update()
    {
        // Pause/Unpause game when escape is pressed if conditions are met (E.g. not interacting with Keypad).
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.IsKeypadBeingUsed && !gameManager._gameOver) 
        {
            if (isPaused) 
            {
                ResumeGame();
                settingsMenu.SetActive(false);
            }
            else 
            {
                PauseGame();
            }
        }
    }

    // Handles behaviour of game over screen.
    public void GameOverScreen() 
    { 
        Time.timeScale = 0f;
        UnlockCursor();

        gameOverMenu.SetActive(true);
        PauseAudio();
    }

    public void WinScreen() 
    {
        Time.timeScale = 0f;
        UnlockCursor();

        winScreen.SetActive(true);
    }

    // Handles behaviour when pausing game.
    public void PauseGame() 
    {
        UnlockCursor();
        GameState(true, 0f);
    }

    // Handles behaviour when resuming game.
    public void ResumeGame()
    {
        LockCursor();
        GameState(false, 1f);
    }

    // Restarts the gameplay level scene when called
    public void RestartGame() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    // Goes to the main menu scene when called
    public void GoToMainMenu() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Quits application/game
    public void QuitGame()
    {
        Application.Quit();
    }

    // Changes game state based on pause status, and sets timescale accordingly
    private void GameState(bool IsGamePaused, float timeScale) 
    {
        pauseMenu.SetActive(IsGamePaused);
        Time.timeScale = timeScale;
        isPaused = IsGamePaused;
    }

    // Locks cursor when called
    private void LockCursor() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Unlocks cursor when called
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Trying this code
    private void ResumeAudio() 
    {
        _generalAudio.SetActive(true);
        _enemyAudio.SetActive(true);
    }

    // Pauses audio objects by disabling their respective game objects when called.
    private void PauseAudio() 
    {
        _generalAudio.SetActive(false);
        _enemyAudio.SetActive(false);
    }

}
