using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    private void Update()
    {
        _progress = Mathf.Clamp01(hapController.happinessSlider / 100f);
        float targetFoV = Mathf.Lerp(_defaultFoV, maximumFoV, fovCurve.Evaluate(_progress));
        _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, targetFoV, ref _velocity, smoothing);
    }
}
