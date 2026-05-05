using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles Player Health, aswell as damage modifier based on happiness meter state.
public class HealthController : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField] public float maxHealth = 100f; // Max health value
    [Range(0f, 100f)]
    [SerializeField] public float currentHealth; // Current health

    [Header("Visual Overlay")]
    [SerializeField] public HealthBar healthBar;

    [Header("References")]
    [SerializeField] public HappinessController happinessController;
    [SerializeField] public AudioClip selfInjureSFX;

    private float _damageModifier;
    private InteractionFailed _interactionFailed;
    private GameManager _gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateVisuals();

        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateVisuals(); 
        UpdateDamageModifier(); 

        // Debug.Log("Damage Modifier: " + _damageModifier);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SelfInjure();
        }
    }

    // Calls on responsible screen that updates HP visuals based on current HP value.
    public void UpdateVisuals() // Update visual indicator for health level
    {
        healthBar.SetHealth(currentHealth);
    }

    // Function that once called, reduces player hp by amount. Has different flags to ensure different outcome based on damage type
    public void ReceiveDamage(int damage, bool isSelfInjure = false, bool insanityDamage = false)
    {
        if (isSelfInjure || insanityDamage)
        {
            currentHealth -= damage;
        } 
        else if (!isSelfInjure)
        {
            currentHealth -= damage * _damageModifier;
        }
    }

    // Function that once called, reduces HP but also reduces Happiness Slider.
    public void SelfInjure() 
    {
        if (currentHealth > 25f && happinessController.happinessSlider > 24f && !_gameManager.insideTrap)
        {
            ReceiveDamage(20, true);
            happinessController.DecreaseHappiness(25, true);
            AudioManager.instance.PlaySFX(selfInjureSFX);
            Debug.Log("Self-injure activated");
        } else
        {
            if (currentHealth <= 25f) _interactionFailed.failedInteractionText(3, "\"I'm already bleeding alot\"");
            else if (_gameManager.insideTrap && happinessController.happinessSlider >= 25f) _interactionFailed.failedInteractionText(3, "\"I'm inside an happy zone, I need to leave first.\"");
            else if (happinessController.happinessSlider < 25f) _interactionFailed.failedInteractionText(3, "\"This is not necessary right now.\"");
            // Debug.Log("Self-injure error: Player either is too low, or has no Happiness at all.");
            // Debug.Log("Current Health:" + currentHealth + " && Current Happiness: " + happinessController.happinessSlider);
        }
    }

    // Updates damage modifier based on current Happiness state, set in HappinessController script.
    private void UpdateDamageModifier()
    {
        switch (happinessController.state)
        {
            case HappinessController.HappinessState.unhappy:
                _damageModifier = 1;
                break;

            case HappinessController.HappinessState.neutral:
                _damageModifier = 2;
                break;

            case HappinessController.HappinessState.happy:
                _damageModifier = 3;
                break;

            case HappinessController.HappinessState.overjoyed:
                _damageModifier = 9999; // instakill
                break;
        }
    }

    // Function that once called, heals the players hp by amount
    void heal(int heal)
    {
        currentHealth += heal;
    }
}
