using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessController : MonoBehaviour
{
    [Header("Happiness Value")]
    [Range(1, 4)]
    public int happinessSlider = 1;
    public static float happinessValue;

    private void Start()
    {
        happinessValue = happinessSlider;
    }

    private void Update()
    {
        happinessValue = happinessSlider;
    }
}