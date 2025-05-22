using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference turnShadowAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera shadowCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float cameraSensitivity = 1f;
    // [SerializeField] private Material shadowMaterial;
    // [SerializeField] private Material normalMaterial;
    
    private CharacterController _controller;

    private Vector3 _velocity;
    private float _verticalRotation;
    private bool _isPressed;
    
    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        turnShadowAction.action.Enable();
        turnShadowAction.action.performed += OnShadowToggle;
    }
    
    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        turnShadowAction.action.Disable();
        turnShadowAction.action.performed -= OnShadowToggle;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        if (lookInput.sqrMagnitude < 0.01f) return;
        
        transform.Rotate(Vector3.up, lookInput.x * cameraSensitivity);

        _verticalRotation -= lookInput.y * cameraSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move *= speed;

        // Apply gravity
        if (_controller.isGrounded)
        {
            _velocity.y = -gravity * Time.deltaTime;
            if (jumpAction.action.triggered) _velocity.y = jumpSpeed;
        }
        else
        {
            _velocity.y -= gravity * Time.deltaTime;
        }
        
        // Move character
        move += _velocity;
        _controller.Move(move * Time.deltaTime);
    }

    private void OnShadowToggle(InputAction.CallbackContext context)
    {
        float interact = turnShadowAction.action.ReadValue<float>();
        bool newState = interact > 0.5f;
       
        if (newState != _isPressed)
        {
            _isPressed = newState;
            shadowCamera.gameObject.SetActive(_isPressed);
            Debug.Log(_isPressed ? "Shadow on!" : "Shadow off!");
        }
    }
}
