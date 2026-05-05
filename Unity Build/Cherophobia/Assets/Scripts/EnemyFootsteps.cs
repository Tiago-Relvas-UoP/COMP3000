using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the playing of Enemy Footsteps SFX (The Mimic), and also holds a method to play attack sfx.
public class EnemyFootsteps : MonoBehaviour
{
    private EnemyController enemy;
    private AudioManager audioManager;

    [Header("Time between footsteps")]
    [SerializeField] public float crouchDelay = 1.0f;
    [SerializeField] public float walkDelay = 1.0f;
    [SerializeField] public float sprintDelay = 1.0f;

    [Header("Properties")]
    [SerializeField] public AudioClip footStepSFX;
    [SerializeField] public AudioClip attackSFX;
    [SerializeField] public float volume = 0.5f;
    [SerializeField] public float spatialBlend = 1f;
    [SerializeField] public float minRange = 5f;
    [SerializeField] public float maxRange = 5f;

    private float _footstepDelay;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyController>();

        _footstepDelay = walkDelay;
        audioManager = GetComponentInChildren<AudioManager>();

        StartCoroutine(PlayFootSteps());
    }

    // Calls upon PlaySFX() method in AudioManager.cs to play Attack SFX
    public void PlayAttackSFX() 
    { 
        audioManager.PlaySFX(attackSFX, volume, spatialBlend, minRange, maxRange);
    }

    // Runs each frame. Plays Footsteps SFX with delay between each playthrough based on current enemy behaviour. Wont play if its currently waiting on top of a patrol point, or is attacking the player.
    IEnumerator PlayFootSteps()
    {
        while (true)
        {
            if (!enemy._isWaiting && !enemy._isAttacking)
            {
                _footstepDelay = walkDelay;
                audioManager.PlaySFX(footStepSFX, volume, spatialBlend, minRange, maxRange);
            }

            yield return new WaitForSeconds(_footstepDelay);
        }
    }
}
