using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateVisuals();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateVisuals();
        UpdateDamageModifier();

        Debug.Log("Damage Modifier: " + _damageModifier);

        if (Input.GetKeyDown(KeyCode.P))
        {
            SelfInjure();
        }
    }

    public void UpdateVisuals() // Update visual indicator for health level
    {
        healthBar.SetHealth(currentHealth);
    }

    // Function that once called, reduces player hp by amount
    public void ReceiveDamage(int damage, bool isSelfInjure = false)
    {
        if (isSelfInjure)
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
        if (currentHealth > 25f && happinessController.happinessSlider > 24f)
        {
            ReceiveDamage(20, true);
            happinessController.DecreaseHappiness(25);
            AudioManager.instance.PlaySFX(selfInjureSFX);
            Debug.Log("Self-injure activated");
        } else
        {
            Debug.Log("Self-injure error: Player either is too low, or has no Happiness at all.");
            Debug.Log("Current Health:" + currentHealth + " && Current Happiness: " + happinessController.happinessSlider);
        }
    }

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
