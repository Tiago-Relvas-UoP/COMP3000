using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void PlayAttackSFX() 
    { 
        audioManager.PlaySFX(attackSFX, volume, spatialBlend, minRange, maxRange);
    }

    // Update is called once per frame
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
