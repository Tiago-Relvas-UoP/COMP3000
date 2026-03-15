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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisuals();

        if (Input.GetKeyDown(KeyCode.P))
        {
            SelfInjure();
        }

        // Debug - Remove before playtest
        if (Input.GetKeyDown(KeyCode.O))
        {
            ReceiveDamage(9999);
        }
    }

    void UpdateVisuals() // Update visual indicator for health level
    {
        healthBar.SetHealth(currentHealth);
    }

    // Function that once called, reduces player hp by amount
    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
    }

    // Function that once called, reduces HP but also reduces Happiness Slider.
    public void SelfInjure() 
    { 
        if (currentHealth > 25f && happinessController.happinessSlider > 24f)
        {
            ReceiveDamage(25);
            happinessController.DecreaseHappiness(25);
            AudioManager.instance.PlaySFX(selfInjureSFX);
            Debug.Log("Self-injure activated");
        } else
        {
            Debug.Log("Self-injure error: Player either is too low, or has no Happiness at all.");
            Debug.Log("Current Health:" + currentHealth + " && Current Happiness: " + happinessController.happinessSlider);
        }
    }

    // Function that once called, heals the players hp by amount
    void heal(int heal)
    {
        currentHealth += heal;
    }
}
