using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Collider trapCollider;
    [SerializeField] public GameObject DoorBlock;
    [SerializeField] public GameObject HappinessTrap;
    [SerializeField] public GameObject FakeItem;

    [Header("Properties")]
    [SerializeField] public float trapDuration = 5.0f;
    [SerializeField] public bool trapActive;

    public bool isPlayerTrapped = false;
    public float _timeSincePlayerTrapped = 0.0f;

    // Start is called before the first frame update
    private void Start()
    {
        trapActive = true;
        FakeItem.SetActive(true);

        DoorBlock.SetActive(false);
        HappinessTrap.SetActive(false);
    }

    private void OnTriggerEnter(Collider trapCollider)
    {
        if (trapActive) 
        {
            trapActive = false; // Deactivate Trap
            isPlayerTrapped = true;
            FakeItem.SetActive(false);

            DoorBlock.SetActive(true);
            HappinessTrap.SetActive(true);
            // DoorBlock.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerTrapped)
        {
            _timeSincePlayerTrapped += Time.deltaTime;
        }

        if (_timeSincePlayerTrapped >= trapDuration)
        {
            isPlayerTrapped = false;
            DoorBlock.SetActive(false);
        }
    }
}
