using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private static readonly int isWalking = Animator.StringToHash("isWalking");

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
    
    private CharacterController _controller;
    private Animator _animator;

    private Vector3 _velocity;
    private Vector2 _moveInput;
    
    private float _verticalRotation;
    private bool _isPressed;
    private bool _isWalking;
    private bool _previousWalking;
    
    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        turnShadowAction.action.Enable();
        
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
        turnShadowAction.action.performed += OnShadowToggle;
    }
    
    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        turnShadowAction.action.Disable();
        
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        turnShadowAction.action.performed -= OnShadowToggle;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        Debug.Log($"MoveInput: {_moveInput}");
        UpdateWalkingState();
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
        UpdateWalkingState();
    }

    private void UpdateWalkingState()
    {
        // if (_isWalking == _previousWalking) return;
        // _animator.SetBool(isWalking, _isWalking);
        // _previousWalking = _isWalking;
        
        _isWalking = _moveInput.sqrMagnitude > 0.1f;
        if (_isWalking == _previousWalking) return;
        
        _animator.SetBool(isWalking, _isWalking);
        _previousWalking = _isWalking;
    }
    
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("The Player collided with an enemy.");
        }
        else if (other.gameObject.CompareTag("ShadowArea"))
        {
            Debug.Log("The Player entered a shadow area.");
        }
    }

    private void HandleLook()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        if (lookInput.sqrMagnitude < 0.01f) return;
        
        transform.Rotate(Vector3.up, lookInput.x * cameraSensitivity);

        _verticalRotation -= lookInput.y * cameraSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -20f, 20f);
        cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        Vector3 move = (transform.right * _moveInput.x + transform.forward * _moveInput.y).normalized * speed;
        
        // Apply gravity
        if (_controller.isGrounded)
        {
            _velocity.y = -2f;
            if (jumpAction.action.triggered) _velocity.y = jumpSpeed;
        }
        else
        {
            _velocity.y -= gravity * Time.deltaTime;
        }
        
        // Move character
        Vector3 finalMove = new(move.x, _velocity.y, move.z);
        _controller.Move(finalMove * Time.deltaTime);
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
