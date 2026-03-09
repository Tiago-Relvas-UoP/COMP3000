using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject gameOverMenu;
    [SerializeField] public GameObject winScreen;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public static bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused) 
            {
                ResumeGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }

    public void GameOverScreen() 
    { 
        Time.timeScale = 0f;
        UnlockCursor();

        gameOverMenu.SetActive(true);
    }

    public void WinScreen() 
    {
        Time.timeScale = 0f;
        UnlockCursor();

        winScreen.SetActive(true);
    }

    public void PauseGame() 
    {
        UnlockCursor();
        GameState(true, 0f);
    }

    public void ResumeGame()
    {
        LockCursor();
        GameState(false, 1f);
    }

    public void RestartGame() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainMenu() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameState(bool IsGamePaused, float timeScale) 
    {
        pauseMenu.SetActive(IsGamePaused);
        Time.timeScale = timeScale;
        isPaused = IsGamePaused;
    }

    private void LockCursor() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
