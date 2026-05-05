using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamFix : MonoBehaviour
{
    // This script was an attempt at fixing an issue with the camera where it would be turning away from its set position on game start.

    public Transform objectTrans;
    public float _timer = 0.03f;
    private float _time;
    private bool _done = false;
    private Quaternion _originalRotation;

    private void Start()
    {
        _originalRotation = objectTrans.transform.rotation;
    }
    private void Update()
    {
        if (!_done) 
        {
            _time += Time.deltaTime;
            Debug.Log(_time);
        }

        if (_time > _timer && !_done) 
        {
            Vector3 euler = _originalRotation.eulerAngles;
            euler.y = 0f;

            objectTrans.transform.rotation = Quaternion.Euler(euler);

            _done = true;
        }
    }
}
