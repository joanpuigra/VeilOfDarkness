using UnityEngine;

public class ShadowDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask shadowLayer;
    
    private void Update()
    {
        DetectShadows();
    }
    
    private void DetectShadows()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, shadowLayer);
        // Debug.Log(hits.Length > 0 ? "Player on Shadow" : "Player on light");
    }
}
