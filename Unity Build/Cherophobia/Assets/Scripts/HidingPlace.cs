using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingPlace : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject hideText, showText;
    [SerializeField] public GameObject normalPlayer, hidingPlayer;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public EnemyController enemyController;

    [Header("Transforms")]
    [SerializeField] public Transform enemyTransform;

    [Header("Bools")]
    [SerializeField] public bool interactable, hidingHere;

    [Header("Distance")]
    [SerializeField] public float loseDistance;


    // Start is called before the first frame update
    private void Start()
    {
        interactable = false;
        hidingHere = false;
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
                hidingHere = true;
                gameManager.hidingInLocker = true;

                hideText.SetActive(false);
                showText.SetActive(true);

                hidingPlayer.SetActive(true);
                normalPlayer.SetActive(false);

                interactable = false;

                // enemyController._state = EnemyState.Patrolling;
            }
        }

        if(hidingHere == true) 
        { 
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                hidingHere = false;
                gameManager.hidingInLocker = false;

                showText.SetActive(false);

                normalPlayer.SetActive(true);
                hidingPlayer.SetActive(false);

            }
        }
    }
}
