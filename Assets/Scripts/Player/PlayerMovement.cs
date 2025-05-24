using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float jumpSpeed = 5f;

    private CharacterController controller;
    private Vector3 velocity;

    public void Move(Vector2 moveInput, bool jump, bool grounded)
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move *= speed;
    
        if (grounded)
        {
            velocity.y = -gravity * Time.deltaTime;
            if (jump) velocity.y = jumpSpeed;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    
        move += velocity;
        controller.Move(move * Time.deltaTime);
    }
}