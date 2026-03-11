using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MasterDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject interactionUI;
    [SerializeField] private GameManager gameManager;

    [Header("Item Properties")]
    [SerializeField] private GameObject masterDoor;

    [Header("Bools")]
    [SerializeField] public bool interactable;

    private int _fuseIndex;


    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;

        masterDoor = this.gameObject;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        interactionUI.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameManager.IsMasterUnlocked)
        {
            if (other.CompareTag("MainCamera"))
            {
                interactionUI.SetActive(true);
                interactable = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameManager.IsMasterUnlocked)
        {
            if (other.CompareTag("MainCamera"))
            {
                interactionUI.SetActive(false);
                interactable = false;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(interactable && Input.GetKeyDown(KeyCode.E)) 
        { 
            if (gameManager.IsMasterUnlocked) 
            {
                gameManager.playerEscaped = true;
            }
        }
    }
}

