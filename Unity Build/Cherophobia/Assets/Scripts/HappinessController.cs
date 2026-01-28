using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class HappinessController : MonoBehaviour
{
    [Header("Happiness Value")]
    [Range(0f, 100f)]
    public float happinessSlider = 0f;
    public static float happinessValue;

    [Header("Happiness Thresholds")]
    public const float unhappyThreshold = 0f;
    public const float neutralThreshold = 25f;
    public const float happyThreshold = 50f;
    public const float overjoyedThreshold = 75f;

    [Header("Time before Happiness Decay")]
    public float decayTimer = 0f; // Interval Between Decays
    public float decayRate = 1f; // How 

    [Header("Visual Overlay")]
    public HealthBar healthBar;

    private float currentThreshold; // Current Happiness Threshold.

    public HappinessState state;
    public enum HappinessState
    {
        unhappy,
        neutral,
        happy,
        overjoyed
    }

    public void Start()
    {
        happinessValue = happinessSlider;
        healthBar.SetHealth(happinessValue);
    }

    public void Update()
    {
        happinessValue = happinessSlider;

        if (happinessSlider > currentThreshold)
        {
            StartCoroutine("happinessdecay");
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseHappiness(20);
        }

        StateHandler();
    }

    void IncreaseHappiness(int addedHap)
    {
        happinessSlider += addedHap;
        healthBar.SetHealth(happinessValue); // Visual for when Happiness Levels increase. It will increase Alpha levels of the set overlay (Ignore name, as its used for Health visuals aswell)
    }

    // Function that handles happiness decay if its value is not equal to the threshold.
    IEnumerator happinessdecay()
    {
        yield return new WaitForSeconds(decayTimer); // Wait x seconds
        
        while (happinessSlider > currentThreshold) // Execute while Slider value is higher than currentThreshold
        {
            happinessSlider -= decayRate;
        }
    }

    // Manages Happiness States. When reaching a new Threshold, the current threshold is set to that to change Happiness Decay Limit
    private void StateHandler()
    {
        if (happinessSlider == unhappyThreshold)
        {
            state = HappinessState.unhappy;
            currentThreshold = unhappyThreshold;

        } 
        else if (happinessSlider == neutralThreshold)
        {
            state = HappinessState.neutral;
            currentThreshold = neutralThreshold;
        }
        else if (happinessSlider == happyThreshold) 
        {
            state = HappinessState.happy;
            currentThreshold = happyThreshold;
        }
        else if (happinessSlider == overjoyedThreshold)
        {
            state = HappinessState.overjoyed;
            currentThreshold = overjoyedThreshold;
        }
    }
}