using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableItems : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject interactText;
    [SerializeField] public GameManager gameManager;

    [Header("Item Properties")]
    [SerializeField] public GameObject item;
    [Range(1, 4)]
    [SerializeField] public int itemType; // 1 = Crowbar; 2 = Fuse; 3 = Code

    [Header("Bools")]
    [SerializeField] public bool interactable;

    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactText.SetActive(true);
            interactable = true;
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactText.SetActive(false);
            interactable = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (interactable == true && item.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptRemove(itemType);
            }
        }
    }

    // 1 = Planks; 2 = FuseBox; 3 = Code; 4 = Master Door
    private void AttemptRemove(int _itemType)
    {
        switch (_itemType)
        {
            case 1:
                if (gameManager.hasCrowbar == true)
                {
                    gameManager.hasCrowbar = false;
                    gameManager.removedPlanks += 1;

                    item.SetActive(false);
                    interactText.SetActive(false);
                }

                break;
            case 2:
                if (gameManager.hasFuse == true)
                {
                    gameManager.hasFuse = false;
                    gameManager.placedFuse = true;

                    item.SetActive(false);
                    interactText.SetActive(false);
  
                }
                break;
            case 3:
                if (gameManager.hasCode == true)
                {
                    gameManager.hasCode = false;
                    gameManager.placedCode = true;

                    item.SetActive(false);
                    interactText.SetActive(false);

                }
                break;
            case 4:
                if (gameManager.IsMasterUnlocked == true)
                {
                    item.SetActive(false);
                    interactText.SetActive(false);
                }

                break;
        }

    }
}
