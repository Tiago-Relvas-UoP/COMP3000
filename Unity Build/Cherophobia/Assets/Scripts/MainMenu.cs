using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() 
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SettingsMenu() 
    { 
        
    }

    public void QuitGame() 
    {
        Application.Quit();
    }
}
