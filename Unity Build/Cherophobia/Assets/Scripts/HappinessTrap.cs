using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessTrap : MonoBehaviour
{
    [Header("Values")]
    public int HappinessReceived = 1;
    public float DelayPerDot = 1.0f;
    public float TimeBeforeActive = 5.0f;

    [Header("References")]
    public HappinessController happinessController;
    public Collider collider;

    private float _timeSincePlayerInRange = 0.0f;
    private float _timeSinceLastIncrease = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider collider)
    {
        bool initialDebug = false;

        /*
        if (!initialDebug)
        {
            Debug.Log("Player has entered Trap zone!");
            initialDebug = !initialDebug; // Flip bool value
        }*/

        if (collider.gameObject.tag == "Player")
        {
            _timeSincePlayerInRange += Time.deltaTime;

            if (_timeSincePlayerInRange >= TimeBeforeActive)
            {
                _timeSinceLastIncrease += Time.deltaTime;

                if (_timeSinceLastIncrease >= DelayPerDot)
                {
                    _timeSinceLastIncrease = 0.0f;
                    happinessController.IncreaseHappiness(HappinessReceived);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Debug.Log("Player has left trap zone!");
            _timeSincePlayerInRange = 0.0f;
        }
    }
}
