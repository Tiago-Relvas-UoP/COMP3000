using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeadBob : MonoBehaviour
{
    [Header("Head Bob Settings")]
    [SerializeField] public float amount = 0.002f;
    [SerializeField, Range(1f, 30f)] public float frequency = 10.0f;
    [SerializeField, Range(10f, 100f)] public float smooth = 10.0f;

    Vector3 StartPos;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForHeadBobTrigger();
        StopHeadBob();
    }

    private void CheckForHeadBobTrigger() 
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob() 
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2f) * amount * 1.6f, smooth * Time.deltaTime);

        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadBob() 
    {
        if (transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, 1 * Time.deltaTime);
    }
}
