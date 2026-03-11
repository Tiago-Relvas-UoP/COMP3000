using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject interactionUI;
    [SerializeField] private GameManager gameManager;

    [Header("Item Properties")]
    [SerializeField] private GameObject item;
    [Range(1, 2)]
    [SerializeField] public int itemType; // 1 = Crowbar; 2 = Fuse; 

    [Header("Bools")]
    [SerializeField] public bool interactable;
    [SerializeField] public bool behindPuzzle;

    // Start is called before the first frame update
    private void Start()
    {
        item = this.gameObject;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        interactable = false;
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
        if (interactable && item.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ObtainItem(itemType);
            }
        }
    }

    private void ObtainItem(int _itemType)
    {
        switch (itemType)
        {
            case 1:
                if (gameManager.hasCrowbar == false)
                {
                    gameManager.currentCrowbars++;
                }

                break;
            case 2:

                gameManager.currentFuses++;
                break;
        }

        item.SetActive(false);
        interactionUI.SetActive(false);
    }
}
