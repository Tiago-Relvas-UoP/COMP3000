using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // sensitivity
    public float sensX;
    public float sensY;

    public Transform orientation;

    // rotation
    private float xRotation;
    private float yRotation;

    // Mouse Axis
    private float mouseX;
    private float mouseY;

    private GameObject _object;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Fixes issue with camera on start
        Vector3 rot = transform.rotation.eulerAngles;
        yRotation = rot.y;
        xRotation = rot.x;

    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}