using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    
    Rigidbody2D rigidBody;

    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 4f; // tweaken kijken wat goede waarde is 

    private bool isGrounded = true;
    private float moveSpeed = 6f;
    Vector2 inputMovement;

    [SerializeField] private List <GameObject> PickedUpObjects = new List<GameObject>();
    
    private InputActionMap moveActionMap;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        moveActionMap = inputActions.FindActionMap("Move");
        moveActionMap.Enable();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        rigidBody.linearVelocity = new Vector2(inputMovement.x * moveSpeed, rigidBody.linearVelocity.y);
    }

    public void HandleMoving(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void HandeleJumping(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Side wall"))
        {
            moveActionMap.Disable();
        }
    }

    public void PickupThrowableObjects(int id, GameObject pickUp)
    {
        switch (id)
        {
            case 0:
                Debug.Log("object met nummer 1 is opgepakt");
                PickedUpObjects.Add(pickUp);
                pickUp.SetActive(false);
                break;
            case 1:
                Debug.Log("object met nummer 2 is opgepakt");
                PickedUpObjects.Add(pickUp);
                pickUp.SetActive(false);
                break;
        }
    }
}
