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
            receiveDamage(20);
            Debug.Log("Player received damage (HP: " +  currentHealth + ")");
        }
    }

    void UpdateVisuals() // Update visual indicator for health level
    {
        healthBar.SetHealth(currentHealth);
    }

    // Function that once called, reduces player hp by amount
    void receiveDamage(int damage)
    {
        currentHealth -= damage;
    }

    // Function that once called, heals the players hp by amount
    void heal(int heal)
    {
        currentHealth += heal;
    }
}
