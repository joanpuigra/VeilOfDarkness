using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private int _currentWaypoint;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
    }

    private void Update()
    {
        PatrolWaypoints();
    }
    
    private void PatrolWaypoints()
    {
        // Check agent remaining distance
        if (!(navMeshAgent.remainingDistance < 0.5f)) return;
     
        // Move to next waypoint
        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[_currentWaypoint].position);
        
        // Walking
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            //! Walking animation
        }
        else
        {
            //! Idle animation
        }
    }
}
