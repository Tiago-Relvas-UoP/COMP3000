using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles Key-item collection based on Key-item type.
public class KeyItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject interactionUI;
    [SerializeField] private GameManager gameManager;
    [SerializeField] public AudioClip grabItemSFX;

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

    // Uses same approach compared to HidingPlace.cs
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactionUI.SetActive(true);
            interactable = true;
        }
    }

    // Uses same approach compared to HidingPlace.cs
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
        // If interactable and item is active
        if (interactable && item.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) // if appropriate button is pressed, collect item and play SFX
            {
                AudioManager.instance.PlaySFX(grabItemSFX);
                ObtainItem(itemType);
            }
        }
    }

    // Determines item type to add the appropriate amount to the inventory on collection.
    private void ObtainItem(int _itemType)
    {
        switch (itemType)
        {
            case 1:
                gameManager.currentCrowbars++;
                break;

            case 2:

                gameManager.currentFuses++;
                break;
        }

        // Disable item on collection, alongside interaction UI
        item.SetActive(false);
        interactionUI.SetActive(false);
    }
}
