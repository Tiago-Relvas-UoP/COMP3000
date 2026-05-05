using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMaster : MonoBehaviour, IInteractable
{
    // THIS IS THE OLD MASTER DOOR SCRIPT, ITS NOT IN USE ANYMORE

    private GameManager gameManager;
    private GameObject itemComponent;

    public void Start()
    {
        var gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();

        itemComponent = GameObject.FindGameObjectWithTag("MasterDoor");

    }

    public void Interact()
    {
        if (gameManager.IsMasterUnlocked) 
        {
            Debug.Log("Interaction successful!");
            itemComponent.SetActive(false);
        } else
        {
            Debug.Log("Player is yet to unlock the door!");
        }
    }
}
