using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Properties")]
    public float maxHealth = 100f; // Max health value
    [Range(0f, 100f)]
    public float currentHealth; // Current health

    [Header("Visual Overlay")]
    public HealthBar healthBar;

    [Header("References")]
    public HappinessController happinessController;

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
            Debug.Log("Self-injure activated.");
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
