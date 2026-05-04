using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fusebox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject interactionUI;
    [SerializeField] private GameManager gameManager;

    [Header("Item Properties")]
    [SerializeField] private MeshRenderer fuseMesh;

    [Header("Bools")]
    [SerializeField] public bool interactable;

    [Header("Failed Interaction")]
    [SerializeField] private float textDuration;
    [SerializeField] private string failedText;
    private InteractionFailed _interactionFailed;

    private int _fuseIndex;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;
        _fuseIndex = 0;

        fuseMesh = this.GetComponent<MeshRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _audioSource = this.GetComponent<AudioSource>();

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
        if (interactable && !fuseMesh.enabled)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                if (gameManager.hasFuse)
                {
                    gameManager.placedFuses++;
                    gameManager.currentFuses--;
                    fuseMesh.enabled = true;
                    interactionUI.SetActive(false);

                    _audioSource.Play();
                }
                else 
                {
                    _interactionFailed.failedInteractionText(textDuration, failedText);
                }
            }
        }
    }
}
