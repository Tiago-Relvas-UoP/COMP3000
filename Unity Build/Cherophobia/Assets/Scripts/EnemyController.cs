using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// Enemy States
public enum EnemyState
{
    Patrolling,
    Following,
    Attacking,
    Investigating, // Not being used atm. It will be used when the AI hears a sound in the proximity to go investigate it.
}

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player; // Reference to player.
    [SerializeField] private Transform[] patrolPoints; // Stores all patrol points.
    [SerializeField] public HealthController healthController; // Reference to Health Bar Component.
    [SerializeField] public GameManager gameManager;

    [Header("Settings")]
    [SerializeField] private float patrolWaitTime = 2f; // Time to wait before resuming patrol.
    [SerializeField] private float stopAtDistance = 0.5f; // Stop-distance offset when Enemy reaches patrol point.
    [SerializeField] private float detectionRange = 5f; // How far the enemy can see.
    [SerializeField] private float viewAngle = 90f; // How wide the enemy can see.
    [SerializeField] private float losePlayerTime = 3f; // Time before enemy loses interest once player leaves LoS.
    [SerializeField] private float attackRange = 1.2f; // How far Enemy can attack.

    private NavMeshAgent _agent; // NavMeshAgent Component.
    private Animator _animator; // Animator Component.
    public EnemyState _state = EnemyState.Patrolling; // Set to Patrolling by default.
    private int _currentPatrolIndex; // Stores the current patrol point.
    public bool _isWaiting; // Determines if Enemy is waiting or not.
    private float _timeSinceLostPlayer; // Stores in a float (that is incremented with delta Time) how much time has passed since Enemy has lost player in their LoS.
    public bool _isAttacking; // Determines if Enemy is currently mid-attack or not.
    private EnemyState m_storestate;
    public bool isChasing;

    public float _dstToPlayer; // Used to calculate if Enemy should despawn if player reaches a "safe" zone;

    // Called before Start()
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GoToNextPatrolPoint(); // On Game Start, calculate and set next patrol point.
    }

    // Update is called once per frame
    private void Update()
    {
        var dstToPlayer = Vector3.Distance(player.position, transform.position); // Stores how much distance there is between Enemy and player.
        _dstToPlayer = dstToPlayer;


        if (m_storestate != _state) Debug.Log("New state: " + _state);
        m_storestate = _state;

        // Switch statement that controls the Enemies behaviour depending on current state
        switch (_state)
        {
            // When Enemy state is set to Patrolling
            case EnemyState.Patrolling:
                Patrol();

                if(dstToPlayer < detectionRange && CanSeePlayer())
                {
                    _state = EnemyState.Following;
                }

                break;

            // When Enemy state is set to Following (the player)
            case EnemyState.Following:
                FollowPlayer(); // Sets destination to player

                // If player is within range, attack them
                if (dstToPlayer <= attackRange) 
                {
                    _state = EnemyState.Attacking;
                    StartAttack(); // Triggers attack animation and
                }

                // If cannot see Player, start "cooldown" and once its done, move to closest patrol point and resume patrol
                if (!CanSeePlayer())
                {
                    _timeSinceLostPlayer += Time.deltaTime; // Increment with delta-time
                    if (_timeSinceLostPlayer >= losePlayerTime) // If lost player for enough time
                    {
                        _state = EnemyState.Patrolling; // State change to Patrolling
                        GoToClosestPatrolPoint(); // Change destination to closest patrol point

                    }
                }
                else
                {
                    _timeSinceLostPlayer = 0f; // Set back to 0
                }

                break;

            // When Enemy state is set to Attacking (The player)
            case EnemyState.Attacking:

                Attack();
                if(!_isAttacking) // Note to self: Maybe this is the issue with Enemy not attacking player in range after attacking once?
                {
                    _state = EnemyState.Following; // State change to Following
                    _agent.isStopped = false; // Enemy (Agent) is no longer stopped
                }

                break;
        }
        // Patrol();
        UpdateAnimations();
    }

    // Method that Stops enemy, sets Attacking bool to true and activates the "Attack" Trigger for the animation to start
    private void StartAttack()
    {
        _agent.isStopped = true;
        _isAttacking = true;
        _animator.SetTrigger("Attack");
    }

    private void Attack()
    {
        _agent.isStopped = true; // On attack, agent must stop
        var direction = (player.position - transform.position).normalized; // Direction calculation between Enemy (agent) and player
        direction.y = 0f; // Disregards y value since its irrelevant.
        if (direction != Vector3.zero) // If Enemy is not looking in Players direction
        {
            transform.rotation = Quaternion.LookRotation(direction); // Rotate them to player.
        }
    }

    // Used by animation event. Sets Attacking parameter to false, and deals damage to player.
    private void OnAttackAnimationEnd()
    {
        _isAttacking = false;
    }

    private void DamagePlayer() 
    {
        healthController.ReceiveDamage(20);
    }

    // Method called to set agent destination to current players position
    private void FollowPlayer() 
    {
        _agent.SetDestination(player.position);
    }

    // First part of Patrol behaviour. First it will check if its waiting, and if not then it will check if there are no pending paths and if it has reached a patrol point, then start a couroutine to wait and set next patrol point. 
    private void Patrol() 
    {
        if (_isWaiting) return; // Return nothing if Enemy is Waiting.
        if (!_agent.pathPending && _agent.remainingDistance <= stopAtDistance) 
        {
            StartCoroutine(WaitAtPatrolPoint()); // Call Coroutine 
        }
    }

    // Coroutine when Enemy reaches a Patrol point. First, enemy is set to waiting and is stopped, then it waits a few seconds (set in inspector) and once the duration is over, then next patrol point is set and enemy starts moving there.
    private IEnumerator WaitAtPatrolPoint()
    {
        Debug.Log("Waiting at Patrol");
        // Sets enemy to Wait/Stop on reaching current patrol point
        _isWaiting = true;
        _agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime); // Wait X seconds before resuming

        // Enemy is no longer set to Wait/Stop, and next Patrol Point is calculated.
        _agent.isStopped = false;
        GoToNextPatrolPoint(); // Set next patrol point
        _isWaiting = false;
    }

    // Forces enemy to retrieve to the closest patrol point. This method is called once it loses interest in a player.
    private void GoToClosestPatrolPoint()
    {
        Debug.Log("Going to closest Patrol Point");

        if (patrolPoints.Length == 0) return; // Returns nothing if there are no set patrol points.

        var closestIndex = 0; // Set to 0 by default, but will store the closest patrol point index to then be set as the next interest point for enemy to walk to
        var closestDistance = float.MaxValue; // Sets to maximum possible value on start

        // Go through each patrol point to find the closest
        for (var i = 0; i < patrolPoints.Length; i++)
        {
            var distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < closestDistance) // if current calculated distance is shorter than current closest distance
            {
                closestDistance = distance; // Set closest distance to new distance
                closestIndex = i; // Change patrol point index
            }
        }

        _currentPatrolIndex = closestIndex; // New current patrol point is set to determined patrol index
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position); // Change Enemy (Agent) Destination to that Patrol Index
    }

    // Method that sets the next patrol point for enemy to go to.
    private void GoToNextPatrolPoint() 
    {
        Debug.Log("Going to next patrol point");

        if (patrolPoints.Length == 0) return; // Returns nothing if there are no set patrol points.

        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position); // Sets destination to current set index patrol point
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length; // Changes current patrol index to next one so next time method is called, it will go to the new one.
      
    }

    // Method is constantly called in Update. Manages Enemy animations
    private void UpdateAnimations()
    {
        var isWalking = _agent.velocity.sqrMagnitude > 0.01f; // Set to one (true) if the agent has any velocity applied to it
        _animator.SetBool("IsWalking", isWalking); // Bool is set to True/False depending on current isWalking status.
    }

    // Method that returns true if two sub-methods also return true.
    
    private bool CanSeePlayer()
    {
        //Debug.Log("Facing player: " + IsFacingPlayer());
        //Debug.Log("ClearPath: " + HasClearPathToPlayer());
        return IsFacingPlayer() && HasClearPathToPlayer(); // Returns a true value is both methods also return the same
    }

    // Method that checks if Enemy is currently facing the player
    private bool IsFacingPlayer()
    {
        if (gameManager.hidingInLocker == true) 
        {
            _timeSinceLostPlayer = 99999999999999;
            return false;
        }
        

        var dirToPlayer = (player.position - transform.position).normalized; // Direction calculation between Enemy (agent) and player
        var angle = Vector3.Angle(transform.forward, dirToPlayer); // Calculates angle between Enemy (agent) and player
        return angle <= viewAngle / 2f; // Returns a true bool if angle is less or equal than the set view angle divided by 2
    }

    // Method that checks if Enemy has a Clear view towards the player
    private bool HasClearPathToPlayer() 
    {
        var dirToPlayer = player.position - transform.position; // Distance calculation between Enemy (agent) and player
        if(Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, dirToPlayer.magnitude)) // If Raycast that originates from enemy hits the player
        {
            return hit.transform == player; // Returns true if player has been hit by raycast
        }

        return true;
    }

    private void OnEnable()
    {
        //Debug.Log("Enemy was enabled!");

        _state = EnemyState.Patrolling;
        _isWaiting = false;
        _isAttacking = false;
        _timeSinceLostPlayer = 0f;

        _agent.isStopped = false;
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
    }
}
