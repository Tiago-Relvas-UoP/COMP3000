using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// OLD INTERACTABLE SYSTEM, NOT IN USE ANYMORE
interface IInteractable
{
    public void Interact();
}

public class Interactable : MonoBehaviour
{
    public Transform InteractorSource; // Object source
    public float InteractRange; // Range for interactable

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray raycast = new Ray(InteractorSource.position, InteractorSource.forward);

            if (Physics.Raycast(raycast, out RaycastHit hitInfo, InteractRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
