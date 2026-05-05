using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningScreen : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float warningDuration;

    private float _countdown;

    // Start is called before the first frame update
    void Start()
    {
        _countdown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _countdown += Time.deltaTime;

        if (_countdown >= warningDuration)
        {
            SceneManager.LoadScene("MainMenu");
        } 
    }
}
