using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button continueButton;
    [SerializeField] private float duration;

    private float _time = 0.0f;
    private bool _startTimer = false;

    private void Update()
    {
        if (_startTimer) _time += Time.deltaTime;

        if (_time >= duration)
        {
            _startTimer = false;
            continueButton.interactable = true;
        }
    }

    public void StartTimer() 
    {
        _startTimer = true;
    }
}
