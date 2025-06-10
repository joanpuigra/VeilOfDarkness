using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShadowGizmoCharacter : MonoBehaviour
{
    [SerializeField] private LayerMask shadowLayer;
    [SerializeField] private Vector3 leftScale = new (0.1f, 1f, 1f);
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private GameObject characterRenderer;
    [SerializeField] private float detectionRadius = 1f;

    private Rigidbody rb;
    private Vector3 targetScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetScale = normalScale;
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, shadowLayer);

        bool leftDetected = hits.Any(hit => hit.transform.position.x < transform.position.x);

        targetScale = leftDetected ? leftScale : normalScale;

        characterRenderer.transform.localScale = Vector3.Lerp(
            characterRenderer.transform.localScale,
            targetScale,
            Time.deltaTime * 5f
        );
    }

    private void OnDrawGizmos()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, shadowLayer);

        bool leftDetected = hits.Any(hit => hit.transform.position.x < transform.position.x);

        Gizmos.color = leftDetected ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}