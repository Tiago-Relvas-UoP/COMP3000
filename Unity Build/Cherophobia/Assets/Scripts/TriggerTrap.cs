using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerTrap : MonoBehaviour
{
    [Header("Values")]
    public int HappinessReceived = 25;
    public bool isTrapActive;
    public GameObject trapMesh;

    [Header("References")]
    public HappinessController happinessController;
    public Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        ShowMesh();

        trapMesh.SetActive(ShowMesh());
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        if (isTrapActive)
        {
            happinessController.IncreaseHappiness(25);
            isTrapActive = false;
        }
    }
    private bool ShowMesh()
    {
        if (isTrapActive)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
