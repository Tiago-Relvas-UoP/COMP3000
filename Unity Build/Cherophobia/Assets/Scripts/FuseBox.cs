using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour, IInteractable
{
    private GameManager gameManager;

    public void Start()
    {
        var gameObject = GameObject.Find("GameManager");

        if (gameObject == null)
        {
            Debug.Log("could not find gameObject");
            return;
        }
        gameManager = gameObject.GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.Log("could not find script on gameObject");
            return;
        }
    }

    public void Interact()
    {
        if (gameManager.hasFuse && !gameManager.placedFuse) 
        {
            gameManager.placedFuse = true;
            Debug.Log("Interaction successful!");
        } else
        {
            Debug.Log("Player does not have a fuse!");
        }
    }
}
