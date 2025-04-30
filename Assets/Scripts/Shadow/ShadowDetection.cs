using UnityEngine;
using System.Linq;

public class ShadowDetection : MonoBehaviour
{
    [SerializeField] private Transform shadowTransform;
    [SerializeField] private Transform lightTransform;
    private LightType _lightType;

    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Vector3 shadowExtrusion = Vector3.zero;

    private Vector3[] _shadowVertices;
    private Mesh _shadowMeshCollider;
    private MeshCollider _shadowCollider;

    private bool _canShadowUpdate = true;
    [SerializeField][Range(0.02f, 1f)] private float updateShadowInterval = 0.5f;

    private void Awake()
    {
        InitializeShadowCollider();

        // Gets the light type
        _lightType = lightTransform.GetComponent<Light>().type;

        // Filter the mesh vertices and creates a new mesh
        // Distinct() is used to remove duplicate vertices
        _shadowVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();
        _shadowMeshCollider = new Mesh();
    }

    private void Update()
    {
        // Sets the shadow collider on the object position
        shadowTransform.position = transform.position;
    }

    private void FixedUpdate()
    {
        // Updates the shadow collider only if the transform has changed
        //! NEEDS TO BE CHECKED IF IT WORKS / hasChanged is Working?
        if (!transform.hasChanged || !_canShadowUpdate) return;
        Invoke(nameof(UpdateShadowCollider), updateShadowInterval);
        _canShadowUpdate = false;
    }

    private void InitializeShadowCollider()
    {
        GameObject shadowGameObject = shadowTransform.gameObject;

        // Hides the shadow gameObject in the hierarchy
        shadowGameObject.hideFlags = HideFlags.HideInHierarchy;

        // Creates a new mesh collider for the shadow collider
        _shadowCollider = shadowGameObject.AddComponent<MeshCollider>();
        _shadowCollider.convex = true;
        _shadowCollider.isTrigger = true;
    }

    private void UpdateShadowCollider()
    {
        // Updates the shadow collider mesh vertices
        _shadowMeshCollider.vertices = GetShadowVertices();
        _shadowCollider.sharedMesh = _shadowMeshCollider;
        _canShadowUpdate = true;
    }

    // Calculates shadow vertices
    private Vector3[] GetShadowVertices()
    {
        // Gets the shadow vertices from the mesh filter
        Vector3[] points = new Vector3[2 *_shadowVertices.Length];

        // Gets the raycast direction
        Vector3 raycastDirection = lightTransform.forward;

        int n = _shadowVertices.Length;

        // Iterates through the shadow vertices to create the shadow collider
        for (int i = 0; i < n; i++)
        {
            // Transforms local vertices to world space
            Vector3 point = transform.TransformPoint(_shadowVertices[i]);

            // Change the raycast direction based on the light type
            if (_lightType != LightType.Directional)
            {
                // Point light or spotlight
                raycastDirection = point - lightTransform.position;
            }

            points[i] = GetShadowDirection(point, raycastDirection);
            points[n + i] = GetShadowExtrusion(point, points[i]);
        }

        return points;
    }

    // Calculates raycast direction
    private Vector3 GetShadowDirection(Vector3 fromPosition, Vector3 direction)
    {
        return Physics.Raycast(fromPosition, direction, out var hit, Mathf.Infinity, targetLayerMask)
            ? hit.point - transform.position
            : fromPosition + 100 * direction - transform.position;
    }

    // Calculates shadow extrusion
    private Vector3 GetShadowExtrusion(Vector3 shadowVertexPosition, Vector3 shadowPointPosition)
    {
        return shadowExtrusion.sqrMagnitude == 0
            ? shadowVertexPosition - transform.position
            : shadowPointPosition + shadowExtrusion;
    }
}
