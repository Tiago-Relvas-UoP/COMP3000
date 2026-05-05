using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles smooth FOV changes based on the current Happiness Meter value.

public class FieldOfViewUpdate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public HappinessController hapController;

    [Header("Settings")]
    [SerializeField] public float maximumFoV;
    [SerializeField] public AnimationCurve fovCurve;
    [SerializeField, Range(0f, 1f)] public float smoothing = 0.15f;

    private Camera _camera;
    private float _defaultFoV;
    private float _progress;
    private float _velocity;

    private void Start()
    {
        _camera = this.GetComponent<Camera>();
        _defaultFoV = _camera.fieldOfView;
    }

    // FOV update calculation
    private void Update()
    {
        _progress = Mathf.Clamp01(hapController.happinessSlider / 100f); // Clamps happiness progress between 0 and 1
        float targetFoV = Mathf.Lerp(_defaultFoV, maximumFoV, fovCurve.Evaluate(_progress)); // Evaluates current FOV value based on default and maximum based on an animation curve set in the inspector.
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, targetFoV, ref _velocity, smoothing); // Smoothly changes current FOV to new value based on the smoothing variable set in the inspector, once Happiness Meter changes.
    }
}
