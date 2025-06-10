using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference
        moveAction,
        lookAction,
        jumpAction,
        toggleShadowAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        toggleShadowAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        toggleShadowAction.action.Disable();
    }
}