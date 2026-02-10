using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject hideText, showText;
    [SerializeField] public GameObject normalPlayer, hidingPlayer;
    [SerializeField] public EnemyController enemyController;

    [Header("Transforms")]
    [SerializeField] public Transform enemyTransform;

    [Header("Bools")]
    [SerializeField] public bool interactable, hiding;

    [Header("Distance")]
    [SerializeField] public float loseDistance;

    public GameObject PlayerTransformOnHide;
    public GameObject PositionBeforeHiding;

    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;
        hiding = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(true);
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            hideText.SetActive(false);
            interactable = false;
        }
    }


    // Update is called once per frame
    private void Update()
    {
        if(interactable == true) 
        { 
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                hiding = true;

                hideText.SetActive(true);
                showText.SetActive(true);

                hidingPlayer.SetActive(true);
                normalPlayer.SetActive(false);

                interactable = false;

                // enemyController._state = EnemyState.Patrolling;
            }
        }

        if(hiding == true) 
        { 
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                hideText.SetActive(false);
                normalPlayer.SetActive(true);
                hidingPlayer.SetActive(false);
                hiding = false;
            }
        }
    }
}
