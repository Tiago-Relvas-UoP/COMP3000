using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningScreen : MonoBehaviour
{
    // Shows a warning screen when booting up the game for the first time.

    [Header("Settings")]
    [SerializeField] private float warningDuration;

    private float _countdown;

    // Start is called before the first frame update
    void Start()
    {
        _countdown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Start countdown
        _countdown += Time.deltaTime;

        // load main menu scene once enough time has elapsed.
        if (_countdown >= warningDuration)
        {
            SceneManager.LoadScene("MainMenu");
        } 
    }
}
