using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessController : MonoBehaviour
{
    [Header("Happiness Value")]
    [Range(1, 4)]
    public int happinessSlider = 1;
    public static float happinessValue;

    public void Start()
    {
        happinessValue = happinessSlider;
    }

    public void Update()
    {
        happinessValue = happinessSlider;
    }
}