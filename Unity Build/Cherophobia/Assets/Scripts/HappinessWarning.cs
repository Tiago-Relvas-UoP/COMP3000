using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

// Displays a warning message to the player once Happiness reaches a certain threshold. Calls upon the InteractionFailed script to display the warning text.
public class HappinessWarning : MonoBehaviour
{
    [Header("Warning Settings")]
    [SerializeField] private HappinessController happinessController;
    [SerializeField] private float happinessThreshold = 80f;

    [Header("Warning Settings")]
    [SerializeField] private float warningCd = 5.0f;
    [SerializeField] private float warningDuration = 3.0f;
    [SerializeField] private string warningText;
    [SerializeField] private string InsaneText;
    [SerializeField] private TMP_FontAsset InsaneFont;

    private InteractionFailed _interactionFailed;
    private bool _warningOnCooldown;
    private float _time;

    // Start is called before the first frame update
    private void Start()
    {
        _warningOnCooldown = false;
        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
    }

    // Update is called once per frame
    private void Update()
    {
        // If happiness is above the threshold and the warning is not on cooldown, display the warning text and start the cooldown.
        if (happinessController.happinessSlider > happinessThreshold && !_warningOnCooldown) 
        {
            _interactionFailed.failedInteractionText(warningDuration, warningText);
            _warningOnCooldown = true;
        }

        // Else, if its below the threshold but its on cooldown then start countdown
        if (happinessController.happinessSlider < happinessThreshold && _warningOnCooldown) 
        {
            _time += Time.deltaTime;
        }

        // Reset cooldown flag once countdown is over
        if (_time >= warningCd) 
        {
            _warningOnCooldown = false;
            _time = 0.0f;
        }

        // Displays specialized message once Happiness is maxed out, indicating that the player is injuring themselves.
        if (happinessController.happinessSlider >= 100f) _interactionFailed.failedInteractionText(0.5f, InsaneText);
    }
}
