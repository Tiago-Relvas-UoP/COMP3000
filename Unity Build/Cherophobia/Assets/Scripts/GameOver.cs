using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Handles the Game Over sequence
public class GameOver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Image entitySprite;
    [SerializeField] public TextMeshProUGUI Text;
    [SerializeField] public TextMeshProUGUI mysteriousVoice;
    [SerializeField] public TextMeshProUGUI skipText;
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
        skipText.text = "";
        entitySprite.color = new Color(entitySprite.color.r, entitySprite.color.g, entitySprite.color.b, _alphaValue);
    }

    // Update is called once per frame
    private void Update()
    {
        FadeInEntity(); 

        if (_EntityShown) // If entity sprite was shown, call upon UpdateText to show text and fade in an background music.
        {
            UpdateText();
            mysteriousVoice.text = "You hear a mysterious voice...";
            skipText.text = "(Press Left Mouse Button/Space to skip)";

            if (audioSource.volume < _audioVolume) 
            {
                audioSource.volume += Time.deltaTime * 0.05f;
            }
        }
        
    }

    // Slowly fades in the entity sprite, and enables a flag once its fully visible to start text sequence.
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

    // Shows the cause of death text. Dialogue shown is based on current Player Preference "lastdeathcause" value.
    private void UpdateText() 
    {
        DetectPlayerInput();

        _deathCause = PlayerPrefs.GetFloat("lastDeathCause");
        _textCountdown += Time.deltaTime;

        // Disable "Tips" txet and enable interactable menu once all tips have been shown.
        if (_tipsCompleted && !_TextRunning) 
        {
            tipsMenu.SetActive(false);
            GameOverMenu.SetActive(true);
        }

        // If death cause has not yet been shown, show it according to death caused, fetched by PlayerPrefs.
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

            Text.text = "You were killed by: " + cause; // Dynamic death cause
        } 
        else if (!_TextRunning && !_tipsCompleted) // If text is not running and tips have not been completed, call upon Sentences() with a dialogue intext to update current text.
        {
            _TextRunning = true;

            switch (_deathCause)
            {
                case 0f:
                    Text.text = Sentences(_dialogueIndex - 1).text;
                    break;
                case 1f:
                    Text.text = Sentences(_dialogueIndex + 2).text;
                    break;
                case 2f:
                    Text.text = Sentences(_dialogueIndex + 5).text;
                    break;
            }
        }

        // Once enough time has passed, reset the appropriate flags and increment dialogue index to show next appropriate dialogue on next text call.
        if (_textCountdown >= textDuration) 
        {
            _dialogueIndex++;
            _textCountdown = 0.0f;
            _TextRunning = false;
        }
    }

    // Based on input, returns a TextMeshProUGUI text string with appropriate dialogue.
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
                break;
            case 2: // Happy Memory: First Sentence
                dialogue.text = "You deserve love. You deserve meaning.\n\nDo not listen to it.\n\nKeep going, I believe in you.";
                _tipsCompleted = true;
                break;
            case 3: // Happy Memory: First Sentence
                dialogue.text = "These are objects scattered around the environment (Such as Artistic Objects, and other items) that hold sentimental value to you.";
                break;
            case 4: // Happy Memory: Second Sentence
                dialogue.text = "Avoid staying around them for too long, as they will gradually increase your Happiness Meter.\n\nExcess Happiness will kill you.";
                break;
            case 5: // Happy Memory: Second Sentence
                dialogue.text = "These were not always to be feared.\n\nWith time, I hope you heal, and realise that you deserve happiness.\n\nI am proud of you.";
                _tipsCompleted = true;
                break;
            case 6: // Trigger Trap: First Sentence
                dialogue.text = "These are scattered around the environment.\n\nStepping on them will trigger a sequence of Happy Memories, significantly increasing your Happiness Status.";
                break;
            case 7: // Trigger Trap: Second Sentence
                dialogue.text = "Navigate the Environment carefully, watching where you step.\n\nJump over these Traps to avoid them.";
                break;
            case 8: // Trigger Trap: Second Sentence
                dialogue.text = "You are making significant progress.\n\nKeep at it, I am proud of you.\n\nIgnore it, do not let its thoughts consume you from the inside.";
                _tipsCompleted = true;
                break;
            default:
                dialogue.text = "I'm not entirely sure what you died to.\n\nSorry but I wont be able to help you!";
                _tipsCompleted = true;
                break;
        }

        /*
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
        */

        return dialogue;
    }

    // Checks for appropriate button presses to skip current dialogue
    private void DetectPlayerInput() 
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) _textCountdown = 9999f;
    }

    // Resets the appropriate flags and values to default once scene is initialized.
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
