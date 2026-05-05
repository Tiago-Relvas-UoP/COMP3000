using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFuse : MonoBehaviour, IInteractable
{
    private GameManager gameManager;
    private GameObject fuseComponent;


    public void Start()
    {
        // Define Game Manager
        var gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();

        fuseComponent = GameObject.FindGameObjectWithTag("CollectibleFuse");
       
    }

    public void Interact()
    {
        gameManager.hasFuse = true;
        fuseComponent.SetActive(false);
    }
}
