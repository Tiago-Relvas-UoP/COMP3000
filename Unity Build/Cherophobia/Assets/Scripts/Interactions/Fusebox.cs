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

    private int _fuseIndex;


    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;
        _fuseIndex = 0;

        fuseMesh = this.GetComponent<MeshRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        interactionUI.SetActive(false);
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (gameManager.hasFuse)
                {
                    gameManager.placedFuses++;
                    gameManager.currentFuses--;
                    fuseMesh.enabled = true;
                    interactionUI.SetActive(false);
                }
            }
        }
    }
}
