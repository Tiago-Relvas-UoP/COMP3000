using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Image entitySprite;
    [SerializeField] public TextMeshProUGUI Text;
    [SerializeField] public TextMeshProUGUI mysteriousVoice;
    [SerializeField] public GameObject tipsMenu;
    [SerializeField] public GameObject GameOverMenu;
    [SerializeField] public AudioSource audioSource;

    [Header("Overlay Settings")]
    [SerializeField] public float waitTime;
    [SerializeField] public float fadeInTime;
    [SerializeField] public AnimationCurve transitionCurve;

    [Header("Text Settings")]
    [SerializeField] public float textDuration;

    private float _alphaValue;
    private float _countdown;
    private float _transitionProgress;
    private float _deathCause;
    private int _dialogueIndex;

    private bool _DeathCauseShown;
    private bool _EntityShown;
    private bool _TextRunning;
    private bool _tipsCompleted;

    private float _textCountdown;
    private float _audioVolume;

    // Start is called before the first frame update
    private void Start()
    {
        SetupFlags();

        Text.text = "";
        mysteriousVoice.text = "";
        entitySprite.color = new Color(entitySprite.color.r, entitySprite.color.g, entitySprite.color.b, _alphaValue);
    }

    // Update is called once per frame
    private void Update()
    {
        FadeInEntity();

        if (_EntityShown) 
        {
            UpdateText();
            mysteriousVoice.text = "You hear a mysterious voice...";

            if (audioSource.volume < _audioVolume) 
            {
                audioSource.volume += Time.deltaTime * 0.05f;
            }
        }
        
    }

    private void FadeInEntity() 
    {
        _countdown += Time.deltaTime;

        if (_countdown > waitTime)
        {
            _transitionProgress = Mathf.Clamp01((_countdown - waitTime) / fadeInTime);
            _alphaValue = transitionCurve.Evaluate(_transitionProgress);
            entitySprite.color = new Color(entitySprite.color.r, entitySprite.color.g, entitySprite.color.b, _alphaValue);
        }

        if (entitySprite.color.a >= 1f) _EntityShown = true;

    }

    private void UpdateText() 
    {
        DetectPlayerInput();

        _deathCause = PlayerPrefs.GetFloat("lastDeathCause");
        _textCountdown += Time.deltaTime;

        if (_tipsCompleted && !_TextRunning) 
        {
            tipsMenu.SetActive(false);
            GameOverMenu.SetActive(true);
        }

        if (!_DeathCauseShown) 
        {
            string cause;
            _DeathCauseShown = true;

            switch (_deathCause) 
            {
                case 0f:
                    cause = "The Mimic";
                    break;
                case 1f:
                    cause = "Happy Memory";
                    break;
                case 2f:
                    cause = "Happiness Trap";
                    break;
                default:
                    cause = "Unknown";
                    break;
            }

            Text.text = "You were killed by: " + cause;
        } 
        else if (!_TextRunning && !_tipsCompleted) 
        {
            _TextRunning = true;

            switch (_deathCause)
            {
                case 0f:
                    Text.text = Sentences(_dialogueIndex - 1).text;
                    break;
                case 1f:
                    Text.text = Sentences(_dialogueIndex + 1).text;
                    break;
                case 2f:
                    Text.text = Sentences(_dialogueIndex + 3).text;
                    break;
            }
        }

        if (_textCountdown >= textDuration) 
        {
            _dialogueIndex++;
            _textCountdown = 0.0f;
            _TextRunning = false;
        }
    }

    private TextMeshProUGUI Sentences(int number) 
    {
        TextMeshProUGUI dialogue;
        dialogue = new TextMeshProUGUI();

        switch (number) 
        {
            case 0: // The Mimic: First Sentence
                dialogue.text = "The Mimic roams the environment, making a tremendous amount of noise wherever it goes.";
                break;
            case 1: // The Mimic: Second Sentence
                dialogue.text = "If it spots you, run away and hide as soon as possible!";
                _tipsCompleted = true;
                break;
            case 2: // Happy Memory: First Sentence
                dialogue.text = "These are objects scattered around the environment (Such as Artistic Objects, and other items) that hold sentimental value to you";
                break;
            case 3: // Happy Memory: Second Sentence
                dialogue.text = "Avoid staying around them for too long, as they will gradually increase your Happiness Meter.\n\nExcess Happiness will kill you";
                _tipsCompleted = true;
                break;
            case 4: // Trigger Trap: First Sentence
                dialogue.text = "These are scattered around the environment.\n\nStepping on them will trigger a sequence of Happy Memories, significantly increasing your Happiness Status.";
                break;
            case 5: // Trigger Trap: Second Sentence
                dialogue.text = "Navigate the Environment carefully, watching where you step.\n\nJump over these Traps to avoid them";
                _tipsCompleted = true;
                break;
            default:
                dialogue.text = "I'm not entirely sure what you died to.\n\nSorry but I wont be able to help you!";
                _tipsCompleted = true;
                break;
        }

        return dialogue;
    }

    private void DetectPlayerInput() 
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) _textCountdown = 9999f;
    }

    private void SetupFlags() 
    {
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _alphaValue = 0;
        _countdown = 0f;
        _textCountdown = 0f;
        _dialogueIndex = 0;

        _DeathCauseShown = false;
        _EntityShown = false;
        _TextRunning = true;
        _tipsCompleted = false;
        _audioVolume = audioSource.volume;

        audioSource.volume = 0f;
    }
}
