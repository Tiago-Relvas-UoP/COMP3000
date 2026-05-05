using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// Handles behaviour for the Trap Room, which contains the cake

public class TrapRoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Collider trapCollider;
    [SerializeField] public GameObject DoorBlock;
    [SerializeField] public GameObject HappinessTrap;
    [SerializeField] public GameObject FakeItem;
    [SerializeField] public TextMeshPro TakeThisText;

    [Header("Properties")]
    [SerializeField] public float trapDuration = 5.0f;
    [SerializeField] public float rangeDuration = 10.0f;
    [SerializeField] public bool trapActive;

    private bool isPlayerTrapped = false;
    private float _timeSincePlayerTrapped = 0.0f;

    private MeshRenderer WallText;
    private Material glowMaterial;

    private Light _light;
    private float m_previousRange;

    // Start is called before the first frame update
    private void Start()
    {
        trapActive = true;
        FakeItem.SetActive(true);

        DoorBlock.SetActive(false);
        HappinessTrap.SetActive(false);

        _light = GameObject.FindWithTag("TrapRoomLight").GetComponent<Light>();
    }

    // Entering the trigger zone marks the trap as active, changing the fake item mesh with an Happiness Object (Cake)
    // and traps the player inside by turning on the game game object. The light inside the room also changes, and text is shown around the room.
    private void OnTriggerEnter(Collider trapCollider)
    {
        if (trapActive) 
        {
            trapActive = false; // Deactivate Trap
            isPlayerTrapped = true;
            FakeItem.SetActive(false);

            DoorBlock.SetActive(true);
            HappinessTrap.SetActive(true);

            m_previousRange = _light.range;

            _light.color = new Color32(255, 112, 112, 255);
            _light.range = 20f;

            TakeThisText.text = "HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN";
            // DoorBlock.SetActive(false);
        }
    }

    private void Update()
    {
        // if player is trapped, start countdown.
        if (isPlayerTrapped)
        {
            _timeSincePlayerTrapped += Time.deltaTime;
        }

        // If enough time has passed, disable gate that blocks the room so player can flee
        if (_timeSincePlayerTrapped >= trapDuration)
        {
            DoorBlock.SetActive(false);
        }

        // if enough time has passed since player was trapped, reset the light range on the room.
        if (_timeSincePlayerTrapped >= rangeDuration)
        {

            Debug.Log("Light range (before): " + _light.range);
            _light.range = m_previousRange;
            Debug.Log("Light range (after): " + _light.range);
            isPlayerTrapped = false;
        }    
    }
}
