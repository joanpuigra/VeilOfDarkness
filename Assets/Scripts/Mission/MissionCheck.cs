using UnityEngine;

public class MissionCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Transform current = MissionManager.Instance.GetCurrentWaypoint();

        if (transform != current) return;
        
        MissionManager.Instance.UpdateWaypoint();
    }
}