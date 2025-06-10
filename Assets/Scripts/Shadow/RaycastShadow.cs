using System;
using System.Linq;
using UnityEngine;

public class RaycastShadow : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Transform directionalLight;
    [SerializeField] private LayerMask shadowLayer;
    [SerializeField] private GameObject characterRenderer;
    [SerializeField] private GameObject shadowRenderer;
    
    [Header("Shadow Settings")]
    [SerializeField] private Vector3 floorScale = new(1f, 0.01f, 1f);
    [SerializeField] private Vector3 wallScale = new (0.01f, 1f, 1f);
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private float shadowTransition = 50f;
    
    // [Header("Material Settings")]
    // [SerializeField] private Renderer targetRenderer;
    // [SerializeField] private Material[] newMaterial;
    // private int _materialIndex;
    
    private bool isShadowed;
    private RaycastHit lastHit;
    private Vector3 cachedLightDirection;

    private void Awake()
    {
        UpdateLightDirection();
    }

    private void Update()
    {
        RayCastShadowUpdate();
    }

    // private void SwapMaterial()
    // {
    //     if (!targetRenderer) return;
    //     
    //     Material[] materials = targetRenderer.materials;
    //     if (_materialIndex < 0 || _materialIndex >= materials.Length)
    //     {
    //         Debug.LogWarning("Material index is out of range.");
    //         return;
    //     }
    //     materials[_materialIndex] = newMaterial[_materialIndex];
    //     targetRenderer.materials = materials;
    // }
    
    private void RayCastShadowUpdate()
    {
        if (!directionalLight) return;
        
        UpdateLightDirection();
        
        bool targetHit = Physics.Raycast(
            transform.position,
            cachedLightDirection,
            out RaycastHit hit,
            100f,
            shadowLayer
        );
        
        isShadowed = targetHit;
        lastHit = hit;
        
        SetShadowed(isShadowed);
    }

    private void UpdateLightDirection()
    {
        cachedLightDirection = -directionalLight.forward.normalized;
    }

    private void SetShadowed(bool shadowed)
    {
        if (!shadowRenderer) return;
        
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, shadowLayer);
        bool isOnWall = hits.Any(hit => hit.transform.position.x < transform.position.x);
        
        UpdateCharacterScale(shadowed, isOnWall ? wallScale : floorScale);
    }
    
    private void UpdateCharacterScale(bool shadowed, Vector3 shadowScale)
    {
        if (!shadowRenderer) return;
        
        Vector3 targetScale = shadowed ? shadowScale : Vector3.zero;
        
        shadowRenderer.transform.localScale = Vector3.Lerp(
            shadowRenderer.transform.localScale,
            targetScale,
            Time.deltaTime * shadowTransition
        );
        
        characterRenderer.transform.localScale = Vector3.Lerp(
            characterRenderer.transform.localScale,
            shadowed ? Vector3.zero : Vector3.one,
            Time.deltaTime * shadowTransition
        );
    }

    private void OnDrawGizmos()
    {
        // Draw the ray from the character to the light source
        if (!directionalLight) return;
        
        Gizmos.color = isShadowed ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, cachedLightDirection * 5f);
        
        if (isShadowed)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHit.point, 0.1f);
        }
        
        // Draw the detection wall
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, shadowLayer);
        bool leftDetected = hits.Any(hit => hit.transform.position.x < transform.position.x);

        Gizmos.color = leftDetected ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}