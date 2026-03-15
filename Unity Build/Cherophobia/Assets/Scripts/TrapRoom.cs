using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

    private void OnTriggerEnter(Collider trapCollider)
    {
        if (trapActive) 
        {
            trapActive = false; // Deactivate Trap
            isPlayerTrapped = true;
            FakeItem.SetActive(false);

            DoorBlock.SetActive(true);
            HappinessTrap.SetActive(true);

            _light.color = new Color32(255, 112, 112, 255);
            m_previousRange = _light.range;
            _light.range = 20f;

            TakeThisText.text = "HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN  HAPPINESS IS A SIN";
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
            DoorBlock.SetActive(false);
        }

        if (_timeSincePlayerTrapped >= rangeDuration)
        {
            _light.range = m_previousRange;
            isPlayerTrapped = false;
        }
        
    }
}
