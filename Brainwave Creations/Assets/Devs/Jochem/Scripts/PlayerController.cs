using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    
    Rigidbody2D rigidBody;
    Transform catapultBombSpawn;

    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 4f; // tweaken kijken wat goede waarde is 
    [SerializeField] int interactionRange;
    [SerializeField] LayerMask interactableLayer;

    private float timer = 0;
    private bool timerIsActive = false;
    private bool isGrounded = true;
    private float moveSpeed = 6f;
    Vector2 inputMovement;

    [SerializeField] private List <GameObject> PickedUpObjects = new List<GameObject>();
    
    private InputActionMap moveActionMap;

    private void Awake()
    {
        catapultBombSpawn = GameObject.FindGameObjectWithTag("Catapult spawn").gameObject.transform;
        rigidBody = GetComponent<Rigidbody2D>();
        moveActionMap = inputActions.FindActionMap("Move");
        moveActionMap.Enable();
    }

    private void Update()
    {
        Movement();

       if(timerIsActive)
       {
            timer -= Time.deltaTime;
       }
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
        else if (collision.gameObject.CompareTag("Side wall"))
        {
            moveActionMap.Disable();
        }
        if (collision.gameObject.CompareTag("Void"))
        {
            SceneManager.LoadScene(2);
            Debug.Log("Void geraakt Game Over");
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

    public void PlaceBombOnCatapult()
    {
        timerIsActive = true;
        if (timer <= 0)
        {
           Vector2 forward = transform.TransformDirection(Vector2.right);
           RaycastHit2D hit = Physics2D.Raycast(transform.position, forward, interactionRange,interactableLayer);     
           GameObject hitCollider = hit.collider.gameObject;
   
            if (hitCollider.tag == "Catapult")
            {
                var bomb = Instantiate(PickedUpObjects[0].gameObject, catapultBombSpawn.transform.position, Quaternion.identity, catapultBombSpawn);
                bomb.gameObject.SetActive(true);
                PickedUpObjects.RemoveAt(0);
                timerIsActive = false;
                timer = 1;      
             }
        }
    }
}
