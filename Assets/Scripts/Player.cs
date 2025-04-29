using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Input actions added to inspector
    [SerializeField] private InputActionReference
        moveAction,
        lookAction,
        turnShadowAction;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float cameraSensitivity = 1f;

    private Vector3 _velocity;
    private float _verticalRotation;

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        turnShadowAction.action.Enable();
        turnShadowAction.action.performed += ctx => TurnShadow();
    }
    
    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        turnShadowAction.action.Disable();
        turnShadowAction.action.performed -= ctx => TurnShadow();
    }

    private void Update()
    {
        MoveInput();
        LookInput();
        Gravity();
    }

    private void Gravity()
    {
        // Controls velocity based on gravity
        if (controller.isGrounded)
        {
            _velocity.y = -gravity * Time.deltaTime;
        }
        else
        {
            _velocity.y -= gravity * Time.deltaTime;
        }
        controller.Move(_velocity * Time.deltaTime);
    }

    private void MoveInput()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new(moveInput.x, 0, moveInput.y);
        controller.Move(move * (speed * Time.deltaTime));
    }

    private void LookInput()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        if ((lookInput.sqrMagnitude < 0.01f)) return;
        
        // Horizontal rotation
        float horizontalRotation = lookInput.x * cameraSensitivity;
        transform.Rotate(Vector3.up, horizontalRotation);

        // Vertical rotation
        _verticalRotation -= lookInput.y * cameraSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }
    
    private void TurnShadow()
    {
        // Implement shadow turning logic here
        Debug.Log("Shadow turned!");
    }
}
