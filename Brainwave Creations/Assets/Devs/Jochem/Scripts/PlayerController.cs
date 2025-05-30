using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.FilePathAttribute;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public CatapultBehaviour catapultBehaviour;

    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;  

    Rigidbody2D rigidBody;
    Transform catapultBombSpawn;

    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 4; // tweaken kijken wat goede waarde is 
    [SerializeField] int interactionRange;
    [SerializeField] LayerMask interactableLayer;

    private float timer = 0;
    private bool timerIsActive = false;
    private bool isGrounded = true;
    private float moveSpeed = 6f;
    Vector2 inputMovement;

    [SerializeField] private List<GameObject> PickedUpObjects = new List<GameObject>();

    private InputActionMap moveActionMap;


    private void Awake()
    {
        catapultBombSpawn = GameObject.FindGameObjectWithTag("Bomb spawn point").gameObject.transform;
        rigidBody = GetComponent<Rigidbody2D>();
        moveActionMap = inputActions.FindActionMap("Move");
        moveActionMap.Enable();
    }

    private void Update()
    {
        Movement();

        if (timerIsActive)
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

        if (inputMovement.x == -1)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (inputMovement.x == 1)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void HandeleJumping(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                moveActionMap.Enable();
                rigidBody.GetComponent<PlayerController>().enabled = true;
            break;

            case "Side wall":
                moveActionMap.Disable();
            break;

            case "Void":
            case "Enemy":
                SceneManager.LoadScene(2);
                Debug.Log("Void geraakt Game Over");
            break;

            case "Catapult collider":
                catapultBehaviour = collision.gameObject.GetComponent<CatapultBehaviour>();
                moveActionMap.Disable();
                catapultBehaviour.CatapultBehaviourStart();
            break;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, forward, interactionRange, interactableLayer);

            if (PickedUpObjects.Count != 0 && hit.collider != null && hit.collider.gameObject.name == "Bomb catapult")
            {
                catapultBehaviour = hit.collider.GetComponent<CatapultBehaviour>();
                catapultBehaviour.CatapultBehaviourStart();
                var bomb = Instantiate(PickedUpObjects[0], catapultBombSpawn.position, Quaternion.identity);
                bomb.gameObject.SetActive(true);
                PickedUpObjects.RemoveAt(0);
                timerIsActive = false;
                timer = 6;
            }
        }
    }
}
