using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 4f; // tweaken kijken wat goede waarde is 
    private bool isGrounded = true;

    Rigidbody2D rigidBody;

    
    

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    public void HandeleJumping(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }   
    }
}
