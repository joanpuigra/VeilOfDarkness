using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Input actions added to inspector
    [SerializeField] private InputActionReference
        moveAction,
        lookAction,
        turnShadowAction,
        jumpAction;

    private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Camera shadowCamera;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float cameraSensitivity = 1f;
    [SerializeField] private Material newMaterial;
    [SerializeField] private Material exitMaterial;
    private Renderer playerRenderer;

    private bool isPressed;
    
    private Vector3 _velocity;
    private float _verticalRotation;

    private void OnEnable()
    {
        playerRenderer = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();
        moveAction.action.Enable();
        lookAction.action.Enable();
        turnShadowAction.action.Enable();
        turnShadowAction.action.performed += ctx => TurnShadow();
        jumpAction.action.Enable();
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
        JumpInput();
        Gravity();
    }

    private void JumpInput()
    {
        if (jumpAction.action.triggered && controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(2f * gravity);
        }
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

    public void OnShadowEnter()
    {
        if (!playerRenderer || !newMaterial) return;
        playerRenderer.material = newMaterial;
    }

    public void OnShadowExit()
    {
        if (!playerRenderer || !exitMaterial) return;
        playerRenderer.material = exitMaterial;
    }
    
    private void TurnShadow()
    {
        float interact = turnShadowAction.action.ReadValue<float>();
        switch (interact)
        {
            case > 0.5f when !isPressed:
                Debug.Log("Shadow on!");
                isPressed = true;
                break;
            case <= 0.5f when isPressed:
                Debug.Log("Shadow off!");
                isPressed = false;
                break;
        }
        shadowCamera.gameObject.SetActive(isPressed);
    }
}
