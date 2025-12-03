using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessTrap : MonoBehaviour
{
    [Header("Is Trap active?")]
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // other.gameObject.GetComponent<HappinessController>().happinessSlider += 1;
            other.GetComponentInParent<HappinessController>().happinessSlider += 1;

            isActive = false;
            Debug.Log("Entered collision zone!");
        }
    }
}
