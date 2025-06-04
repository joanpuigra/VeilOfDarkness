using UnityEngine;

public class RaycastShadow : MonoBehaviour
{
    [SerializeField] private Transform directionalLight;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private Renderer characterRenderer;
    
    private bool isShadowed;
    private RaycastHit lastHit;

    private void Update()
    {
        Vector3 lightDirection = -directionalLight.forward;
        
        bool targetHit = Physics.Raycast(
            transform.position,
            lightDirection,
            out RaycastHit hit,
            Mathf.Infinity,
            obstructionMask
        );
        
        isShadowed = targetHit;
        lastHit = hit;
        
        SetShadowed(isShadowed);
    }

    private void SetShadowed(bool shadowed)
    {
        characterRenderer.enabled = !shadowed;
    }

    private void OnDrawGizmos()
    {
        if (!directionalLight) return;
        
        Vector3 lightDirection = -directionalLight.forward;
        Gizmos.color = isShadowed ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, lightDirection * 5f);
        
        if (isShadowed)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHit.point, 0.1f);
        }
    }
}