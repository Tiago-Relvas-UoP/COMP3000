using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject keypad; 
    [SerializeField] public GameObject numText;
    [SerializeField] public GameObject incorrectText;
    [SerializeField] public GameObject correctText;
    [SerializeField] public GameObject noAccessText;
    [SerializeField] public GameObject accessGrantedText;
    [SerializeField] public GameObject interactionUI;

    [Header("Player References")]
    [SerializeField] public PlayerMovement playerMov;
    [SerializeField] public PlayerCam playerCam;
    [SerializeField] public Rigidbody playerRigid;

    [Header("Code Properties")]
    [SerializeField] public TextMeshProUGUI numTex;
    [SerializeField] public string codeString, correctCode;
    [SerializeField] public int stringCharacters = 0;
    [SerializeField] public bool interactable, codeDone, keypadActive;
    [SerializeField] public bool isMasterKeypad = true;

    [Header("Button References")]
    public Button but1, but2, but3, but4, but5, but6, but7, but8, but9;

    [Header("Audio Clips")]
    [SerializeField] public AudioClip buttonPressSFX;
    [SerializeField] public AudioClip accessGrantedSFX;
    [SerializeField] public AudioClip accessDeniedSFX;

    private int token = 0;
    private GameManager gameManager;
    private GameObject _blockedDoor;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Similiar approach to the HidingLocker.cs script.
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (codeDone == false)
            {
                interactionUI.SetActive(true);
                interactable = true;
            }
        }
    }

    // Similiar approach to the HidingLocker.cs script.
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactionUI.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        if (interactable == true) // If within interaction range
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) // if appropriate keys are pressed, open keypad game object
            {
                // Set keypad active & disable interactable bool
                keypadActive = true;
                keypad.SetActive(true);
                interactable = false;

                gameManager.IsKeypadBeingUsed = true;

                // Disable players movement
                playerMov.enabled = false;
                playerCam.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if (keypadActive == true) // If keypad active
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // Disable if escape is pressed
            {
                // Disable keypad
                numText.SetActive(true);
                correctText.SetActive(false);
                incorrectText.SetActive(false);
                gameManager.IsKeypadBeingUsed = false;

                stringCharacters = 0;
                codeString = "";

                but1.interactable = true;
                but2.interactable = true;
                but3.interactable = true;
                but4.interactable = true;

                keypadActive = false;

                but5.interactable = true;
                but6.interactable = true;
                but7.interactable = true;

                token = 0;

                but8.interactable = true;
                but9.interactable = true;

                keypad.SetActive(false);

                // Enable players movement
                playerMov.enabled = true;
                playerCam.enabled = true;
                interactable = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            numTex.text = codeString;
            if (stringCharacters == 4) // If 4 characters have been input, check if code is correct
            {
                if (codeString == correctCode) // If code is correct, disable keypad components and disable access.
                {
                    numText.SetActive(false);
                    correctText.SetActive(true);
                    but1.interactable = false;
                    but2.interactable = false;
                    but3.interactable = false;
                    but4.interactable = false;
                    but5.interactable = false;
                    but6.interactable = false;
                    but7.interactable = false;
                    but8.interactable = false;
                    but9.interactable = false;
                    codeDone = true;
                    gameManager.IsKeypadBeingUsed = false;

                    accessGrantedText.SetActive(true);
                    noAccessText.SetActive(false);

                    // GameManager: Set code as placed
                    if (isMasterKeypad) gameManager.placedCode = true;
                    else if (!isMasterKeypad)
                    {
                        _blockedDoor = GameObject.FindWithTag("BlockedDoor");
                        _blockedDoor.SetActive(false);
                    }

                    // Calls upon method to disable keypad canvas component after a set time.
                    if (token == 0)
                    {
                        StartCoroutine(endSesh());
                        token = 1;
                        AudioManager.instance.PlaySFX(accessGrantedSFX, 0.1f);
                    }

                }
                else // If code is wrong, reset keypad components.
                {
                    numText.SetActive(false);
                    incorrectText.SetActive(true);
                    but1.interactable = false;
                    but2.interactable = false;
                    but3.interactable = false;
                    but4.interactable = false;
                    but5.interactable = false;
                    but6.interactable = false;
                    but7.interactable = false;
                    but8.interactable = false;
                    but9.interactable = false;
                    gameManager.IsKeypadBeingUsed = false;

                    // Calls upon method to disable keypad canvas component after a set time.
                    if (token == 0)
                    {
                        StartCoroutine(endSesh());
                        token = 1;
                        AudioManager.instance.PlaySFX(accessDeniedSFX, 0.5f);
                    }
                }
            }
        }
    }

    // When called, appropriately resets all keypad components and returns player to the key, locking cursor in place again.
    IEnumerator endSesh()
    {
        yield return new WaitForSeconds(2.5f);
        numText.SetActive(true);
        correctText.SetActive(false);
        incorrectText.SetActive(false);
        stringCharacters = 0;
        codeString = "";
        but1.interactable = true;
        but2.interactable = true;
        but3.interactable = true;
        but4.interactable = true;
        keypadActive = false;
        but5.interactable = true;
        but6.interactable = true;
        but7.interactable = true;
        token = 0;
        but8.interactable = true;
        but9.interactable = true;
        keypad.SetActive(false);
        playerMov.enabled = true;
        playerCam.enabled = true;
        interactable = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // When called by the canvas components by player interaction, inputs digit to keypad screen based on number that was pressed.

    public void pressedButton(string digit) 
    {
        codeString = codeString + digit;
        stringCharacters = stringCharacters + Convert.ToInt32(digit);
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }

    // Old Button methods
    public void pressedOne()
    {
        codeString = codeString + "1";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedTwo()
    {
        codeString = codeString + "2";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedThree()
    {
        codeString = codeString + "3";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedFour()
    {
        codeString = codeString + "4";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedFive()
    {
        codeString = codeString + "5";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedSix()
    {
        codeString = codeString + "6";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedSeven()
    {
        codeString = codeString + "7";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedEight()
    {
        codeString = codeString + "8";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
    public void pressedNine()
    {
        codeString = codeString + "9";
        stringCharacters = stringCharacters + 1;
        AudioManager.instance.PlaySFX(buttonPressSFX, 0.2f);
    }
}
