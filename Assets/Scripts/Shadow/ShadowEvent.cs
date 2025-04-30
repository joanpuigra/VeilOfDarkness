using UnityEngine;
using UnityEngine.Events;

public class ShadowEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onShadowTrigger;

    private bool _isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _isTriggered) return;
        onShadowTrigger?.Invoke();
        _isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !_isTriggered) return;
        _isTriggered = false;
    }
}
