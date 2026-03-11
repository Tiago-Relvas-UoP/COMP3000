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
    private float lastHappinessValue;

    [Header("Happiness Thresholds")]
    public float unhappyThreshold = 0f;
    public float neutralThreshold = 25f;
    public float happyThreshold = 50f;
    public float overjoyedThreshold = 75f;

    [Header("Happiness Decay")]
    public float timeToWait = 5.0f;
    public float done = 0.0f;
    public float decayInterval = 1.0f;
    public int decayRate = 0;
    private float timeSinceLastIncrease = 0.0f;

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public float maxVolume = 0.5f;

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
        UpdateVisuals();
    }

    public void Update()
    {
        UpdateVisuals();
        StateHandler();

        timeSinceLastIncrease += Time.deltaTime;

        if (timeSinceLastIncrease > timeToWait && happinessSlider > currentThreshold)
        {
            if (Time.time > done)
            {
                done = Time.time + decayInterval;
                DecreaseHappiness(decayRate);
            }
        }

        audioSource.volume = (happinessSlider / 100) * maxVolume;
       
        // Debug
        if (Input.GetKeyDown(KeyCode.L))
        {
            IncreaseHappiness(20);
        }

    }

    void UpdateVisuals() // Update visual indicator for happiness level
    {
        happinessValue = happinessSlider;
        healthBar.SetHealth(happinessSlider);
    }

    public void IncreaseHappiness(int addedHap)
    {
        happinessSlider += addedHap;
        timeSinceLastIncrease = 0.0f;
        // healthBar.SetHealth(happinessSlider); // Visual for when Happiness Levels increase. It will increase Alpha levels of the set overlay (Ignore name, as its used for Health visuals aswell)
    }

    public void DecreaseHappiness(int addedHap)
    {
        happinessSlider -= addedHap;
        // healthBar.SetHealth(happinessSlider); // Visual for when Happiness Levels increase. It will increase Alpha levels of the set overlay (Ignore name, as its used for Health visuals aswell)
    }

    // Manages Happiness States. When reaching a new Threshold, the current threshold is set to that to change Happiness Decay Limit
    private void StateHandler()
    {
        if (happinessSlider <= unhappyThreshold)
        {
            state = HappinessState.unhappy;
            currentThreshold = unhappyThreshold;

        } 
        else if (happinessSlider >= neutralThreshold && happinessSlider < happyThreshold)
        {
            state = HappinessState.neutral;
            currentThreshold = neutralThreshold;
        }
        else if (happinessSlider >= happyThreshold && happinessSlider < overjoyedThreshold) 
        {
            state = HappinessState.happy;
            currentThreshold = happyThreshold;
        }
        else if (happinessSlider >= overjoyedThreshold)
        {
            state = HappinessState.overjoyed;
            currentThreshold = overjoyedThreshold;
        }
    }
}