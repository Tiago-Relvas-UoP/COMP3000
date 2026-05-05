using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    public GameObject light;

    private PlayerFootsteps playerFootsteps;
    private bool _isLightOn;


    private void Start()
    {
        playerFootsteps = GetComponent<PlayerFootsteps>();
        _isLightOn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isLightOn = !_isLightOn;
            playerFootsteps.PlayFlashlightSFX();
            Debug.Log("Flashlight on? - " + _isLightOn);
        }

        SetActive();
    }

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
