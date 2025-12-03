using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePad : MonoBehaviour, IInteractable
{
    private GameManager gameManager;
    private GameObject itemComponent;

    public void Start()
    {
        var gameObject = GameObject.Find("GameManager");
        gameManager = gameObject.GetComponent<GameManager>();

        itemComponent = GameObject.FindGameObjectWithTag("MainCode");

    }

    public void Interact()
    {
        if (gameManager.hasCode && !gameManager.placedCode) 
        {
            Debug.Log("Interaction successful!");
            gameManager.placedCode = true;
        } else
        {
            Debug.Log("Player does not have a code!");
        }
    }
}
