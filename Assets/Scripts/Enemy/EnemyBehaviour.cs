using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Material patrolMaterial;
    [SerializeField] private Material chasingMaterial;
    [SerializeField] private Material fleeingMaterial;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float visionAngle = 90f;
    [SerializeField] private float fleeRange = 1f;
    [SerializeField] private float lostSightTime = 2f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float chaseSpeed = 1.5f;
    [SerializeField] private float fleeSpeed = 1f;

    private NavMeshAgent agent;
    private NPCState currentState = NPCState.Patrolling;
    private int currentWaypoint;
    private float timeWithoutSeeingPlayer;
    private bool playerVisible;

    private enum NPCState { Patrolling, Chasing, Fleeing }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(NPCState.Patrolling);
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, playerPosition.position);
        playerVisible = IsPlayerInSight();

        switch (currentState)
        {
            case NPCState.Patrolling:
                Patrol(playerDistance, playerVisible);
                break;
            case NPCState.Chasing:
                Chase(playerDistance, playerVisible);
                break;
            case NPCState.Fleeing:
                Flee(playerDistance, playerVisible);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Patrol(float distance, bool playerVisible)
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWaypoint();

        if (playerVisible && distance <= detectionRange)
            ChangeState(NPCState.Chasing);
    }

    private void Chase(float distance, bool playerVisible)
    {
        agent.SetDestination(playerPosition.position);

        if (distance <= fleeRange)
            ChangeState(NPCState.Fleeing);
        else if (!playerVisible || distance > detectionRange)
        {
            timeWithoutSeeingPlayer += Time.deltaTime;
            if (timeWithoutSeeingPlayer >= lostSightTime)
                ChangeState(NPCState.Patrolling);
        }
        else
        {
            timeWithoutSeeingPlayer = 0f;
        }
    }

    private void Flee(float distance, bool playerVisible)
    {
        Vector3 fleeDirection = (transform.position - playerPosition.position).normalized;
        Vector3 fleeDestination = transform.position + fleeDirection * 5f;

        if (NavMesh.SamplePosition(fleeDestination, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);

        if (distance > fleeRange + 2f && playerVisible)
            ChangeState(NPCState.Chasing);
        else if (!playerVisible)
        {
            timeWithoutSeeingPlayer += Time.deltaTime;
            if (timeWithoutSeeingPlayer >= lostSightTime)
                ChangeState(NPCState.Patrolling);
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypoint].position);
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    private void ChangeState(NPCState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case NPCState.Patrolling:
                enemyRenderer.sharedMaterial = patrolMaterial;
                agent.speed = patrolSpeed;
                GoToNextWaypoint();
                break;
            case NPCState.Chasing:
                enemyRenderer.sharedMaterial = chasingMaterial;
                agent.speed = chaseSpeed;
                timeWithoutSeeingPlayer = 0f;
                break;
            case NPCState.Fleeing:
                enemyRenderer.sharedMaterial = fleeingMaterial;
                agent.speed = fleeSpeed;
                timeWithoutSeeingPlayer = 0f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private bool IsPlayerInSight()
    {
        Vector3 playerDirection = playerPosition.position - transform.position;
        float angle = Vector3.Angle(transform.forward, playerDirection);

        if (angle < visionAngle / 2f &&
            Vector3.Distance(transform.position, playerPosition.position) <= detectionRange)
        {
            Ray ray = new Ray(transform.position + Vector3.up, playerDirection.normalized);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, detectionRange))
            {
                if (hit.transform == playerPosition)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        
        // Draw detection range
        Gizmos.color = playerVisible ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw flee range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

        if (!playerPosition) return;
        
        Vector3 leftRay = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward;
        Vector3 rightRay = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;

        // Draw vision rays
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up, leftRay * detectionRange);
        Gizmos.DrawRay(transform.position + Vector3.up, rightRay * detectionRange);
    }
}
