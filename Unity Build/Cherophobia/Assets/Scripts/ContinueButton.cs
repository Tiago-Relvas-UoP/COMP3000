using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script handles the "Continue" button inside the Main Menu, in the instructions screen. It toggles the interactability of it after a set amount of time
public class ContinueButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button continueButton;
    [SerializeField] private float duration;

    private float _time = 0.0f;
    private bool _startTimer = false;

    private void Start()
    {
        _time = 0.0f;
    }

    private void Update()
    {
        // Start countdown when flag is true
        if (_startTimer) _time += Time.deltaTime;

        // If enough time has passed, reset timer, disable flag and enable button interactability.
        if (_time >= duration)
        {
            _startTimer = false;
            _time = 0.0f;
            continueButton.interactable = true;
        }
    }

    // Called by the "Play" button in the Main Menu to enable the flag and start the timer.
    public void StartTimer() 
    {
        _startTimer = true;
    }
}
