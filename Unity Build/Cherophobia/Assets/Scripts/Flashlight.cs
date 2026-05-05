using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles flashlight feature in the players side.

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    public GameObject light;

    private PlayerFootsteps playerFootsteps;
    private bool _isLightOn;


    private void Start()
    {
        playerFootsteps = GetComponent<PlayerFootsteps>();
        _isLightOn = true; // Enable light by default
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isLightOn = !_isLightOn;
            playerFootsteps.PlayFlashlightSFX(); // Play SFX. Method is in PlayerFootsteps.cs
            Debug.Log("Flashlight on? - " + _isLightOn);
        }

        SetActive();
    }

    // Switch flashlight game object state based on internal flag.
    private void SetActive()
    {
        switch (_isLightOn)
        {
            case true:
                light.SetActive(true);
                break;

            case false:
                light.SetActive(false);
                break;
        }
    }
}
