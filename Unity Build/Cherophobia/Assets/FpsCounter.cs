using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        float fps = 1.0f / Time.deltaTime;
        _delay += Time.deltaTime;

        counterStatus();

        switch (_enabled)
        {
            case true:
                if (_delay >= 0.2f)
                {
                    _delay = 0.0f;
                    textMeshPro.text = (int)fps + " FPS";
                }
                break;
            case false:
                textMeshPro.text = "";
                break;
        }
        
    }

    private void counterStatus()
    {
        if (Input.GetKeyDown(KeyCode.P)) _enabled = !_enabled;
    }
}
