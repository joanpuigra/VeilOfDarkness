using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float lookSpeed = 2f;
    
    private float _verticalRotation;

    public void Look(Vector2 lookInput)
    {
        if (lookInput.sqrMagnitude < 0.01f) return;

        transform.Rotate(Vector3.up, lookInput.x * lookSpeed);
        
        _verticalRotation -= lookInput.y * lookSpeed;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }
}