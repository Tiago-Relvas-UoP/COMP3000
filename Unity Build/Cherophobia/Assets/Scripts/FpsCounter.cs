using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Enables FPS counter on Top-right corner. Hidden by default, and hot-key not revealed to player.

public class FpsCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private bool _enabled;
    private float _delay;

    // Start is called before the first frame update
    private void Start()
    {
        _enabled = false;
        _delay = 0.0f;

        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        float fps = 1.0f / Time.deltaTime; // FPS is calculated by dividing 1 with current deltaTime
        _delay += Time.deltaTime;

        counterStatus();

        switch (_enabled)
        {
            case true:
                if (_delay >= 0.2f) // Adds delay to FPS display, so it doesnt update constantly.
                {
                    _delay = 0.0f;
                    textMeshPro.text = (int)fps + " FPS"; // Converts fps to int (No decimal values)
                }
                break;
            case false:
                textMeshPro.text = "";
                break;
        }
        
    }

    // Handles appropriate button press to enable/disable FPS counter.
    private void counterStatus()
    {
        if (Input.GetKeyDown(KeyCode.P)) _enabled = !_enabled;
    }
}
