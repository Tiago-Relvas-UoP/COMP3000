using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCode : MonoBehaviour, IInteractable
{
    // OLD CODEPIN SCRIPT. THIS IS NOT USED ANYMORE IN THE PROGRAM

    private GameManager gameManager;
    private GameObject itemComponent;

    public void Start()
    {
        // Define Game Manager
        var gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();

        itemComponent = GameObject.FindGameObjectWithTag("CollectibleCode");
       
    }

    public void Interact()
    {
        gameManager.hasCode = true;
        itemComponent.SetActive(false);
    }
}
