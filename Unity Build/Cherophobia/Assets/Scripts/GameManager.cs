using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject MasterDoor;

    // Master Door
    [SerializeField]
    [Header("Master Door")]
    public bool IsMasterUnlocked;
    public bool placedFuse;
    public bool removedPlanks;
    public bool placedCode;

    // Found objects
    [SerializeField]
    [Header("Collectibles")]
    public bool hasFuse;
    public bool hasCrowbar;
    public bool hasCode;


    // Start is called before the first frame update
    void Start()
    {

        MasterDoor = GameObject.FindGameObjectWithTag("MasterDoor");

        IsMasterUnlocked = false;

        hasFuse = false;
        hasCrowbar = false;
        hasCode = false;
        
        placedFuse = false;
        removedPlanks = false;
        placedCode = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (placedFuse && removedPlanks && placedCode)
        {
            IsMasterUnlocked = true; // Makes Master Door available for interaction
        }
    }
}
