using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject MasterDoor;
    private GameObject hidingSpots;

    // Master Door
    [Header("Master Door")]
    [SerializeField] public bool IsMasterUnlocked;
    [SerializeField] public bool placedFuse;
    [SerializeField] public int removedPlanks;
    [SerializeField] public bool placedCode;

    // Found objects
    [Header("Collectibles")]
    [SerializeField] public bool hasFuse;
    [SerializeField] public bool hasCrowbar;
    [SerializeField] public bool hasCode;

    [Header("Is Player Hiding?")]
    [SerializeField] public bool hidingInLocker;


    // Start is called before the first frame update
    void Start()
    {
        MasterDoor = GameObject.FindGameObjectWithTag("MasterDoor");

        IsMasterUnlocked = false;

        hasFuse = false;
        hasCrowbar = false;
        hasCode = false;
        
        placedFuse = false;
        removedPlanks = 0;
        placedCode = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (placedFuse && removedPlanks == 3 && placedCode)
        {
            IsMasterUnlocked = true; // Makes Master Door available for interaction
        }
    }
}
