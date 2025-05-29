using UnityEngine;

public class ShadowGizmo : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private LayerMask shadowLayer;

    private void OnDrawGizmos()
    {
        bool shadowDetected = Physics.OverlapSphere(transform.position, detectionRadius, shadowLayer).Length > 0;

        Gizmos.color = shadowDetected ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
