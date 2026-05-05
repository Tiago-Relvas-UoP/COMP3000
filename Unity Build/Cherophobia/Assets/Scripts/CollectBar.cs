using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBar : MonoBehaviour, IInteractable
{
    private GameManager gameManager;
    private GameObject itemComponent;


    public void Start()
    {
        // Define Game Manager
        var gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();

        itemComponent = GameObject.FindGameObjectWithTag("CollectibleCrowbar");
       
    }

    public void Interact()
    {
        gameManager.hasCrowbar = true;
        itemComponent.SetActive(false);
    }
}
