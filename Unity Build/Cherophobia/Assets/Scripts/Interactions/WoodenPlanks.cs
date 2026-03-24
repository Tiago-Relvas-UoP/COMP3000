using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlanks : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject interactionUI;
    [SerializeField] public AudioClip PlanksSFX;
    [SerializeField] private GameManager gameManager;

    [Header("Item Properties")]
    [SerializeField] private GameObject woodenPlank;

    [Header("Bools")]
    [SerializeField] public bool interactable;

    [Header("Failed Interaction")]
    [SerializeField] private float textDuration;
    [SerializeField] private string failedText;
    private InteractionFailed _interactionFailed;


    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;

        woodenPlank = this.gameObject;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        interactionUI.SetActive(false);

        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactionUI.SetActive(true);
            interactable = true;
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactionUI.SetActive(false);
            interactable = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (interactable && woodenPlank.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (gameManager.hasCrowbar) 
                {
                    AudioManager.instance.PlaySFX(PlanksSFX);
                    gameManager.removedPlanks++;
                    gameManager.currentCrowbars--;
                    woodenPlank.SetActive(false);
                    interactionUI.SetActive(false);
                } else 
                {
                    _interactionFailed.failedInteractionText(textDuration, failedText);
                }
            }
        }
    }
}