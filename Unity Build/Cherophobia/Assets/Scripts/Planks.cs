using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planks : MonoBehaviour, IInteractable
{
    private GameManager gameManager;
    private GameObject itemComponent;

    public void Start()
    {
        var gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();

        itemComponent = GameObject.FindGameObjectWithTag("Planks");

    }

    public void Interact()
    {
        if (gameManager.hasCrowbar && !gameManager.removedPlanks) 
        {
            Debug.Log("Interaction successful!");
            gameManager.removedPlanks = true;
            itemComponent.SetActive(false);
        } else
        {
            Debug.Log("Player does not have a crowbar!");
        }
    }
}
