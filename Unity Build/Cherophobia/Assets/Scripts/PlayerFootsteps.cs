using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip footStepSFX;
    public AudioClip flashlightSFX;

    [Header("Footstep Delay")]
    [SerializeField] public float walkDelay = 1.0f;
    [SerializeField] public float sprintDelay = 1.0f;

    private PlayerMovement movement;
    private float _footstepDelay;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        StartCoroutine(PlayFootSteps());
    }

    public void PlayFlashlightSFX() 
    {
        AudioManager.instance.PlaySFX(flashlightSFX);
    }

    // Update is called once per frame
    IEnumerator PlayFootSteps() 
    { 
        while (true) 
        { 
            if (movement.moveDirection.magnitude > 0.1f & movement.grounded) 
            {
                AudioManager.instance.PlaySFX(footStepSFX);
            }

            if (movement.state.Equals(PlayerMovement.MovementState.sprinting)) _footstepDelay = sprintDelay;
            else if (movement.state.Equals(PlayerMovement.MovementState.walking)) _footstepDelay = walkDelay;

            yield return new WaitForSeconds(_footstepDelay);
        }
    }
}
