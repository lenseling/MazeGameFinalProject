using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;

enum State
{
    Initializing,
    Investigating,
    Wandering,
}

public class EnemyAI : MonoBehaviour
{
    public float maxInvestiagationTime = 5f;        // max time to investiage the sound
    public float maxInvestigationDistance = 10f;    // max distance to trigger investiagtion
    public float wanderRadius = 10f;                // radius within which the Enemy can wander
    public float wanderInterval = 5f;               // time interval between picking new random destinations
    public float detectionRadius = 2.0f;            // Distance to consider the player "found"
    public float cooldown = 2.0f;                         // cooldown for reducing lives

    private GameObject player;                       // Reference to the player's transform
    private NavMeshAgent agent;                      // reference to the Enemy pathfinding agent
    private float investigationStartTime;           // when the last investigation started
    private float wanderTimer;                      // timer for the current wandering behavior
    private State curState;
    private HealthSystem hs;
    private float lastReduced;

    public void Init()
    {
        // Automatically find and assign the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on the Enemy prefab!");
            return;
        }
        curState = State.Initializing;
        startWandering();
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("GameObject named 'Player' not found in the scene. Make sure it exists and is named correctly.");
        }
        else
        {
            Debug.Log("Player GameObject successfully assigned!");
        }
        Debug.Log("Enemy Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        // check if the Enemy has reached the player
        float distanceToPlayer = Vector3.Distance(agent.transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRadius && Time.time - lastReduced >= cooldown)
        {
            // player found
            FindObjectsOfType<HealthSystem>()[0].reduceLife();
            Debug.Log("Player found!");
            lastReduced = Time.time;
        }
        if (curState == State.Investigating)
        { 
           if(Time.time > investigationStartTime + maxInvestiagationTime)
           {
                // check if the Enemy's investigation time exceeds
                // if exceeds, then stops and starts wandering
                Debug.Log("Enemy stops investiagtion");
                startWandering();
            }
        } else if (curState == State.Wandering)
        {
            wandering();
        }
    }

    private void startWandering()
    {
        wanderTimer = wanderInterval;
        curState = State.Wandering;
        PickRandomDestination();
    }

    private void wandering()
    {
        wanderTimer -= Time.deltaTime;

        // if the Enemy reaches the destination, pick the next one
        if (wanderTimer < 0 || (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) || agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            PickRandomDestination();
            wanderTimer = wanderInterval;
        }
    }

    private void PickRandomDestination()
    {
        // generate a random position within the wander radius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        // sample the position to ensure it's on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            //Debug.Log($"Enemy wanders to new position: {hit.position}");
        }
        else
        {
            Debug.LogWarning("Failed to find a valid NavMesh position for wandering.");
        }
    }

    public void startInvestigating(Transform target)
    {
        // check if the target is within the maximum investigation distance
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > maxInvestigationDistance)
        {
            Debug.Log($"Investigation ignored: Target too far ({distanceToTarget} units).");
            return;
        }

        agent.SetDestination(target.position);
        curState = State.Investigating;
        investigationStartTime = Time.time;
        Debug.Log("Enemy starts investigating.");
    }
}
