using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

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
        if (happinessController.happinessSlider > happinessThreshold && !_warningOnCooldown) 
        {
            _interactionFailed.failedInteractionText(warningDuration, warningText);
            _warningOnCooldown = true;
        }

        if (happinessController.happinessSlider < happinessThreshold && _warningOnCooldown) 
        {
            _time += Time.deltaTime;
        }

        if (_time >= warningCd) 
        {
            _warningOnCooldown = false;
            _time = 0.0f;
        }

        if (happinessController.happinessSlider >= 100f) _interactionFailed.failedInteractionText(0.5f, InsaneText);
    }
}
