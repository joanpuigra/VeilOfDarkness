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
        SetShadowed(Physics.Raycast(
            transform.position,
            lightDirection,
            out RaycastHit hit,
            Mathf.Infinity,
            obstructionMask
        ));
    }

    private void SetShadowed(bool shadowed)
    {
        Color color = characterRenderer.material.color;
        color.a = shadowed ? 0.5f : 1f;
        characterRenderer.material.color = color;
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