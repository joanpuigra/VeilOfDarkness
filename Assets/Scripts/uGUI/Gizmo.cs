using UnityEngine;

public class Gizmo : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Color gizmoColor = Color.red;
    [SerializeField] private float gizmoSize = 0.1f;

    private void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 6;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = gizmoColor;
        lineRenderer.endColor = gizmoColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 forward = transform.forward * gizmoSize;
        Vector3 up = Quaternion.Euler(30, 0, 0) * forward;
        Vector3 down = Quaternion.Euler(-30, 0, 0) * forward;
        Vector3 left = Quaternion.Euler(0, -30, 0) * forward;
        Vector3 right = Quaternion.Euler(0, 30, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + up);
        Gizmos.DrawLine(transform.position, transform.position + down);
        Gizmos.DrawLine(transform.position, transform.position + left);
        Gizmos.DrawLine(transform.position, transform.position + right);
        Gizmos.DrawRay(transform.position, forward);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        float coneLength = gizmoSize * 2;
        float angle = 30f;

        Quaternion leftRot = Quaternion.Euler(0, -angle, 0);
        Quaternion rightRot = Quaternion.Euler(0, angle, 0);
        Quaternion upRot = Quaternion.Euler(-angle, 0, 0);
        Quaternion downRot = Quaternion.Euler(angle, 0, 0);

        Vector3 forward = transform.forward * coneLength;

        Gizmos.DrawRay(transform.position, forward);
        Gizmos.DrawRay(transform.position, leftRot * forward);
        Gizmos.DrawRay(transform.position, rightRot * forward);
        Gizmos.DrawRay(transform.position, upRot * forward);
        Gizmos.DrawRay(transform.position, downRot * forward);
    }

    void Update()
    {
        float angle = 30f;
        float length = gizmoSize * 2;

        Vector3 origin = transform.position;
        Vector3 forward = transform.forward * length;
        Vector3 left = Quaternion.Euler(0, -angle, 0) * forward;
        Vector3 right = Quaternion.Euler(0, angle, 0) * forward;
        Vector3 up = Quaternion.Euler(-angle, 0, 0) * forward;
        Vector3 down = Quaternion.Euler(angle, 0, 0) * forward;

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + left);
        lineRenderer.SetPosition(2, origin + up);
        lineRenderer.SetPosition(3, origin + right);
        lineRenderer.SetPosition(4, origin + down);
        lineRenderer.SetPosition(5, origin);
    }
}
