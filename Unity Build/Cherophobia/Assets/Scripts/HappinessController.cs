using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

// This script handles the Happiness Meter.
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

    [Header("Insanity Settings")]
    [SerializeField] private float insanityDeathTime = 2.0f;
    [SerializeField] private AudioClip damageSFX;
    [SerializeField, Range(0f, 1f)] private float damageVolume;

    private float _deathTimer;
    private HealthController _healthController;

    [Header("Happiness Decay")]
    public float timeToWait = 5.0f;
    public float done = 0.0f;
    public float decayInterval = 1.0f;
    public int decayRate = 0;
    private float timeSinceLastIncrease = 0.0f;

    [Header("Happiness SFX Settings")]
    [SerializeField] public AudioSource audioSource; // HappinessTrack audioSource
    [SerializeField] private AnimationCurve musicCurve;
    [SerializeField] public float maxVolume = 0.5f;
    [SerializeField] public float smoothness = 0.6f;
    private float _velocity;

    [Header("Laugh SFX Settings")]
    [SerializeField] private AudioSource laughSource; 
    [SerializeField] private AnimationCurve laughCurve;
    [SerializeField] private float maxLaughVolume;
    [SerializeField] private float laughSmoothness;

    [Header("Heartbeat SFX Settings")]
    [SerializeField] private AudioSource hbSource;
    [SerializeField] private AnimationCurve hbVolumeCurve;
    [SerializeField] private float hbVolumeSmoothness;
    [SerializeField] private float maxHbVolume;
    [SerializeField] private AnimationCurve hbPitchCurve; 
    [SerializeField] private float maxHbPitch;
    [SerializeField] private float hbPitchSmoothness;

    [Header("Clue Text Settings")]
    [SerializeField] private string clueText;
    [SerializeField] private float clueDuration;
    private bool _wasClueShown;
    private InteractionFailed _interactionFailed;

    private float _musicProgress;
    private float _laughProgress; // laugh volume progress
    private float _hbVolumeProgress; 
    private float _hbPitchProgress; 

    private float _laughVelocity;
    private float _hbVolumeVelocity;
    private float _hbPitchVelocity;      

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

        _deathTimer = 0.0f;
        _healthController = this.GetComponent<HealthController>();

        _wasClueShown = false;
        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
    }

    // Handles all essential methods each frame and functionality
    public void Update()
    {
        UpdateVisuals();
        StateHandler();
        HeartbeatSFX();
        HappinessSFX();
        LaughSFX();

        timeSinceLastIncrease += Time.deltaTime;

        // If enough time has passed since the last happiness increase & current happiness is bigger than current treshold AND clue hasnt been shown yet, then show clue to player
        if (timeSinceLastIncrease > Mathf.Floor(timeToWait / 2.5f) && happinessSlider > currentThreshold && !_wasClueShown)
        { 
            _interactionFailed.failedInteractionText(clueDuration, clueText); // uses method from failedInteraction script
            _wasClueShown = true; // enable flag to prevent further clues from being shown
        }

        // Decay happiness if its higher than current threshold and enough time has passed since last increase.
        if (timeSinceLastIncrease > timeToWait && happinessSlider > currentThreshold)
        {
            if (Time.time > done)
            {
                done = Time.time + decayInterval;
                DecreaseHappiness(decayRate);
            }
        }

        // Call upon method in HealthController to continously damage the player is happiness is capped. Also calls upon PlaySFX from audio manager to play special SFX.
        if (happinessSlider >= 100f) 
        {
            _deathTimer += Time.deltaTime;
            Debug.Log(_deathTimer + "(Death Timer)");

            // Damage is performed in ticks
            if (_deathTimer >= insanityDeathTime) 
            {
                _healthController.ReceiveDamage(10, false, true);
                AudioManager.instance.PlaySFX(damageSFX, damageVolume);
                _deathTimer = 0.0f;
            }

        } else // Once not capped anymore, decreases the death timer until its back to 0.
        {
            _deathTimer -= Time.deltaTime;

            if (_deathTimer < 0.0f) _deathTimer = 0.0f;
        }

        // Ensures happiness never goes above/below its maximum/minimum values.
        if (happinessSlider > 100f) happinessSlider = 100f;
        if (happinessSlider < 0f) happinessSlider = 0f;
    }

    // Dynamically changes volume/pitch of the hearbeat SFX.
    private void HeartbeatSFX() 
    {
        // Heartbeat Volume
        _hbVolumeProgress = Mathf.Clamp01(happinessSlider / 100f); // Clamp progress between 0 and 1 based on current happiness level
        float targetVolume = Mathf.Lerp(0.0f, maxHbVolume, hbVolumeCurve.Evaluate(_hbVolumeProgress)); // Get target volume from set animation curve
        hbSource.volume = Mathf.SmoothDamp(hbSource.volume, targetVolume, ref _hbVolumeVelocity, hbVolumeSmoothness); // Smoothen out volume transition between changes

        // Heartbeat Pitch
        _hbPitchProgress = Mathf.Clamp01(happinessSlider / 100f); // Clamp progress between 0 and 1 based on current happiness level
        float targetPitch = Mathf.Lerp(1.0f, maxHbPitch, hbPitchCurve.Evaluate(_hbPitchProgress)); // Get target pitch from set animation curve
        hbSource.pitch = Mathf.SmoothDamp(hbSource.pitch, targetPitch, ref _hbPitchVelocity, hbPitchSmoothness); // Smoothen out pitch transition between changes
    }

    // Dynamically changes volume of Happiness track. Follows similiar approach to HeartbeatSFX method.
    private void HappinessSFX()
    {
        _musicProgress = Mathf.Clamp01(happinessSlider / 100f);
        float targetVolume = Mathf.Lerp(0.0f, maxVolume, musicCurve.Evaluate(_musicProgress));
        audioSource.volume = Mathf.SmoothDamp(audioSource.volume, targetVolume, ref _velocity, smoothness);
    }

    // Dynamically changes volume of Laugh/Whisper SFX. Follows similiar approach to HeartbeatSFX method.
    private void LaughSFX() 
    {
        // Laugh Volume
        _laughProgress = Mathf.Clamp01(happinessSlider / 100f);
        float targetVolume = Mathf.Lerp(0.0f, maxLaughVolume, laughCurve.Evaluate(_laughProgress));
        laughSource.volume = Mathf.SmoothDamp(laughSource.volume, targetVolume, ref _laughVelocity, laughSmoothness);
    }

    // Calls upon method in HealthBar script, which is responsible for updating Happiness Visuals (Opacity/Scale) based on current happiness meter value. Hands in the current Happiness Value parameter
    void UpdateVisuals() // Update visual indicator for happiness level
    {
        happinessValue = happinessSlider;
        healthBar.SetHealth(happinessSlider); // "Health Bar" refers to a misnamed components that changes visuals. Name change pending once I organize project
    }

    // When called, increases Happiness Meter based on given amount and resets timer that handles last happiness increase
    public void IncreaseHappiness(int addedHap)
    {

        happinessSlider += addedHap;
        timeSinceLastIncrease = 0.0f;
        
        // healthBar.SetHealth(happinessSlider); // Visual for when Happiness Levels increase. It will increase Alpha levels of the set overlay (Ignore name, as its used for Health visuals aswell)
    }

    // Decreases Happiness Meter based on given amount. A bool is in place to determine if decay should bypass current threshold or not (For self-injure mechanic)
    public void DecreaseHappiness(int addedHap, bool decreaseToThreshhold = false)
    {
        if (!decreaseToThreshhold)
        {
            happinessSlider -= addedHap;
        }
        else 
        {
            happinessSlider = currentThreshold;
            happinessSlider -= addedHap;
        }
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