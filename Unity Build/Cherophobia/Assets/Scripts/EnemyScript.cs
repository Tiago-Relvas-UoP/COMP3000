using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    // OLD ATTEMPT AT ENEMY AI FOR THE MIMIC. NOT USED IN THE FINAL PRODUCT.
    // SCRIPT KEPT INSIDE PROGRAM FOR STORING/HISTORY PURPOSES.

    public NavMeshAgent navMeshAgent; // References Navigation Mesh Agent for Enemy

    // Enemy variables
    public float startWaitTime = 3; // How much Enemy should wait before resuming patrol
    public float timeToRotate = 1; // How long it takes enemy to Rotate
    public float walkSpeed = 5; // Walk speed. When on patrol / Not in chase
    public float chaseSpeed = 9; // Chase speed. When chasing player
    public float viewRadius = 15; // How far Enemy can see
    public float viewAngle = 90; // How wide the Enemies POV is.

    public LayerMask playerMask; // Reference to Players Layer Mask
    public LayerMask obstacleMask; // !! WARNING !! THIS MIGHT NOT BE NEEDED // Reference to Obstacles Layer Mask

    public float meshResolution = 1f; // Idk
    public int edgeInterations = 4; // Idk. I think this references how many points to leave when detecting an edge? So if there is a pillar, then it will form a non-walkable layer that scales with edgeDistance that is composed of 4 vertices.
    public float edgeDistance = 0.5f; // How much space to leave between edges of objects

    // Patrol variables
    public Transform[] waypoints; // Patrol points
    private int m_CurrentWaypointIndex; // Stores the amount of waypoints in an Index.

    // Player position references
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    // Member references to Enemy variables
    private float m_WaitTime;
    private float m_TimeToRotate;

    // Bools
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing Variables
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true; 
        m_CaughtPlayer = false; 
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get NavMeshAgent component

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyView();

        if (!m_IsPatrol)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    void Chase()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if(!m_CaughtPlayer)
        {
            MoveSpeed(chaseSpeed);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) 
        { 
            if(m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f) 
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                MoveSpeed(walkSpeed);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else 
            { 
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Patrol()
    {
        // Checks if player is nearby
        if(m_PlayerNear)
        {
            // If player is nearby, and time to rotate is below or equal to zero, navigate towards last player position.
            if(m_TimeToRotate <= 0)
            {
                MoveSpeed(walkSpeed);
                PlayerSpotted(playerLastPosition);
            }
            else  // Else, stop in place for a few seconds before returning to patrol
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else // If player is not near
        {
            m_PlayerNear = false; 
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); // Return to patrol to last set waypoint

            // Checks if enemy has reached current waypoint
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) 
            { 
                // If waiting time is over, set next waypoint, and start patrolling there
                if(m_WaitTime <= 0) 
                {
                    NextPoint();
                    MoveSpeed(walkSpeed);
                    m_WaitTime = startWaitTime;
                }
                else // Else, stand still and wait a few seconds.
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    // Called to change Enemy move speed.
    void MoveSpeed(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    // Called to stop the Enemy. So for example, when they reach last known player position and they aren't in Line of Sight.
    void Stop() 
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    // Called when Enemy reaches a set Patrol point so it goes to the next one.
    public void NextPoint() 
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length; // Returns incremented value compared to previous waypoint index unless its the same as length (total waypoints)
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); // Sets destination to new Waypoint
    }

    // Called when player makes contact with enemy collider
    void CaughtPlayer()
    {
        // Modify later so player is dealt damage.
        m_CaughtPlayer = true;
    }

    // Called when player is in LoS (Line of Sight)
    void PlayerSpotted(Vector3 player)
    {
        navMeshAgent.SetDestination(player); // Set Agent Destination to player
        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(m_WaitTime <= 0) // If waited enough time, return to patrol
            {
                m_PlayerNear = false;
                MoveSpeed(walkSpeed);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else // Stand still and continously decrease time to wait until its 0
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    // Handles enemies view. Includes behaviours when player enters within range.
    void EnemyView()
    {
        // Increases in Length when player is in range.
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        // Loop runs until player is out of range (So when Collider length becomes the same as i)
        for(int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform; // Player Transform
            Vector3 dirToPlayer = (player.position - transform.position).normalized; // Direction towards player

            // Checks if player is within the enemies view angle.
            if(Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position); // Distance towards player

                // Casts a Raycast in front of enemy. Checks if player is within distance, and not being an obstacle. 
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask)) 
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else // If not in distance / Behind an obstacle.
                {
                    m_PlayerInRange = false;
                }
            }

            // Checks if distance between Enemy and Player is bigger than set view radius
            if(Vector3.Distance(transform.position, player.position) > viewRadius) 
            {
                m_PlayerInRange = false;
                m_PlayerPosition = Vector3.zero;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }

            // When player is in range, update player position with new new value
            if(m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }   
        }
    }
}
