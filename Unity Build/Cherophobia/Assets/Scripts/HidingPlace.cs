using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject hideText, showText, showText2;
    [SerializeField] public GameObject normalPlayer, hidingPlayer;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public EnemyController enemyController;
    [SerializeField] public HappinessController happinessController;
    [SerializeField] public GameObject hidingCamera;
    [SerializeField] private GameObject normalCamera;

    [Header("Failed Interaction")]
    [SerializeField] private float textDuration;
    [SerializeField] private string failedText;

    [Header("Transforms")]
    [SerializeField] public Transform enemyTransform;

    [Header("Bools")]
    [SerializeField] public bool interactable, hidingHere;

    [Header("Distance")]
    [SerializeField] public float loseDistance;

    public GameObject _light;
    private InteractionFailed _interactionFailed;

    private PlayerMovement _playerMovement;

    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;
        hidingHere = false;

        hidingCamera.SetActive(false);

        _playerMovement = normalPlayer.GetComponentInChildren<PlayerMovement>();

        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
    }

    // Show text if player interactable collider is touching the interactable, and enable interaction text
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(true);
            interactable = true;
        }
    }

    // ~Hide text if player interactable collider is not touching the interactable, and disable interaction text
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(false);
            interactable = false;
        }
    }


    // Update is called once per frame
    private void Update()
    {
        // If interactable and appropriate button is pressed, hide.
        if(interactable == true) 
        { 
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) 
            {
                if (happinessController.happinessSlider < 75f) // If happiness is below certain threshold, allow interaction.
                {
                    hidingHere = true;
                    _light.SetActive(true);
                    gameManager.hidingInLocker = true;

                    hideText.SetActive(false);
                    showText.SetActive(true);
                    showText2.SetActive(true);

                    _playerMovement.enabled = false;
                    normalCamera.SetActive(false);
                    hidingCamera.SetActive(true);
                    Debug.Log("Player is hiding!");



                    interactable = false;
                } else // Else, call upon method in failedInteraction.cs to show failed interaction text.
                {
                    _interactionFailed.failedInteractionText(textDuration, failedText);
                }
            }
        }

        if(hidingHere == true) // If hiding
        { 
            // If appropriat key is pressed, stop hiding.
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                hidingHere = false;
                _light.SetActive(false);
                gameManager.hidingInLocker = false;

                showText.SetActive(false);
                showText2.SetActive(false);

                _playerMovement.enabled = true;

                normalCamera.SetActive(true);
                hidingCamera.SetActive(false);

                Debug.Log("Player is no longer hiding!");

                //normalPlayer.SetActive(true);
                //hidingPlayer.SetActive(false);

            }
        }
    }
}
