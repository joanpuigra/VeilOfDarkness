using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private GameObject missionCompleted;
    [SerializeField] private Transform waypointUI;
    [SerializeField] private Transform player;
    
    private Transform target;

    private int _currentWaypointIndex = 0;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Start()
    {
        FirstMission();
    }
    
    private void Update()
    {
        target = GetCurrentWaypoint();
        if (target) UpdateWaypointUI();
    }

    public Transform GetCurrentWaypoint()
    {
        return _currentWaypointIndex < waypoints.Length ? waypoints[_currentWaypointIndex] : null;
    }

    public void UpdateWaypoint()
    {
        if (_currentWaypointIndex >= waypoints.Length) return;
        _currentWaypointIndex++;
        if (_currentWaypointIndex >= waypoints.Length) MissionCompleted();
    }


    private void FirstMission()
    {
       
    }

    private void MissionCompleted()
    {
        if (missionCompleted) missionCompleted.SetActive(true);
        Debug.Log("Mission Completed!");
    }
    
    private void UpdateWaypointUI()
    {
        waypointUI.gameObject.SetActive(true);
        Vector3 direction = target.position - player.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Set the rotation of the UI element
        waypointUI.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}

