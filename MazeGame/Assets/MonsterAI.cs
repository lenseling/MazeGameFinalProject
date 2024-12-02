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

public class MonsterAI : MonoBehaviour
{
    public NavMeshAgent agent;                      // reference to the Monster pathfinding agent
    public float maxInvestiagationTime = 5f;        // max time to investiage the sound
    public float maxInvestigationDistance = 10f;    // max distance to trigger investiagtion
    public Transform[] waypoints;                   // waypoints for wandering

    private int curWaypointsIdx = 0;                // current position in the waypoints list
    private float investigationStartTime;           // when the last investigation started
    private State curState;

    // Start is called before the first frame update
    void Start()
    {
        curState = State.Initializing;
        startWandering();
    }

    // Update is called once per frame
    void Update()
    {
        // check if the monster has reachedthe player
        if (curState == State.Investigating)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // player found
                // TODO: monster behavior
            }
            else if(Time.time > investigationStartTime + maxInvestiagationTime)
            {
                // check if the monster's investigation time exceeds
                // if exceeds, then stops and starts wandering
                Debug.Log("Monster stops investiagtion");
                startWandering();
            }
        } else
        {
            wandering();
        }
    }

    private void startWandering()
    {
        // pick the next waypoint in the list, make monster start wandering to that point
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[curWaypointsIdx].position);
            curWaypointsIdx = (curWaypointsIdx + 1) % waypoints.Length;
            curState = State.Wandering;
        }
    }

    private void wandering()
    {
        // if the monster reaches the current waypoint, pick the next one
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            startWandering();
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
        Debug.Log("Monster starts investigating.");
    }
}
