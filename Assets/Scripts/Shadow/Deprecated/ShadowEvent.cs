using UnityEngine;
using UnityEngine.Events;

namespace Shadow
{
    public class ShadowEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onShadowTrigger;
        [SerializeField] private UnityEvent onShadowExit;

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
            onShadowExit?.Invoke();
            _isTriggered = false;
        }
    }
}
