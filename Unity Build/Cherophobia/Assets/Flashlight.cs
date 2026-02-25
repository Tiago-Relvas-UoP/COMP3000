using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject light;
    private bool isLightOn;

    private void Start()
    {
        isLightOn = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isLightOn = !isLightOn;
            Debug.Log("Flashlight on? - " + isLightOn);
        }

        SetActive();
    }

    private void SetActive()
    {
        switch (isLightOn)
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
