using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject collectText;
    [SerializeField] public GameManager gameManager;

    [Header("Item Properties")]
    [SerializeField] public GameObject item;
    [Range(1, 3)]
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
            collectText.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            collectText.SetActive(false);
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
                    gameManager.hasCrowbar = true;
                }

                break;
            case 2:
                gameManager.hasFuse = true;
                break;
            case 3:
                gameManager.hasCode = true;
                break;
        }

        item.SetActive(false);
        collectText.SetActive(false);
    }
}
