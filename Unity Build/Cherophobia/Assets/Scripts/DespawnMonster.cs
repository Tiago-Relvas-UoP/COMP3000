using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnMonster : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float minimumDistance = 20f;
    [SerializeField] public Collider playerCollider;

    public EnemyController _enemyController;
    public GameObject _enemyObject;
    private float _distance;
    private bool _enemyIsDisabled;

    // Start is called before the first frame update
    private void Start()
    {
        _enemyIsDisabled = false;
        _enemyController = GameObject.FindGameObjectWithTag("MainEnemy").GetComponent<EnemyController>();
        _enemyObject = GameObject.FindGameObjectWithTag("MainEnemy");
    }

    // Update is called once per frame
    private void Update()
    {
        _distance = _enemyController._dstToPlayer; // Stores distance. Uses variable inside EnemyController script.

        // Disables enemy based on flag state
        if (_enemyIsDisabled) _enemyObject.SetActive(false);
        else if (!_enemyIsDisabled) _enemyObject.SetActive(true);
    }

    // Disables Enemy Game Object after entering trigger zone if there is enough distance between the player AND it is not following/chasing them.
    private void OnTriggerEnter(Collider playerCollider)
    {
        if (_distance >= minimumDistance && _enemyController._state == EnemyState.Patrolling) _enemyIsDisabled = true;
        else if (_distance <= minimumDistance || _enemyController._state == EnemyState.Following) _enemyIsDisabled = false;
    }

    // Re-enables Enemy if it was disabled from OnTriggerEnter
    private void OnTriggerExit(Collider playerCollider)
    {
        if (_enemyIsDisabled) _enemyIsDisabled = false;
    }
}
