using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public CatapultBehaviour catapultBehaviour;
    [SerializeField] private InputActionAsset inputActions;
    Rigidbody2D rigidBody;
    Transform catapultBombSpawn;
    PlayerController playerController;
    Camera mainCamera;
    Animator animator;
    //variables
    float defaultCameraSize;

    [Header("Player Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] int interactionRange;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] float enemyZoomOutAmount;

    public bool slinging = false;
    protected bool isGrounded = false;
    private float moveSpeed = 6f;
    Vector2 inputMovement;

    [SerializeField] private List<GameObject> PickedUpObjects = new List<GameObject>();

    private InputActionMap moveActionMap;

    private void Awake()
    {  
       //setting Camera references
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        defaultCameraSize = mainCamera.orthographicSize;      
       //setting Input references
        moveActionMap = inputActions.FindActionMap("Move");
        moveActionMap.Enable();
        //GetComponent refs
        playerController = GetComponent<PlayerController>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        /* catapultBombSpawn = GameObject.FindGameObjectWithTag("Bomb spawn point").gameObject.transform;*/
        //setting animator variables
       
    }

    private void Update()
    {
        Movement();
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Movement()
    {
        rigidBody.linearVelocity = new Vector2(inputMovement.x * moveSpeed, rigidBody.linearVelocity.y);
    }

    public void HandleMoving(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        animator.SetFloat("InputMovement", inputMovement.x);

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
                slinging = false;
                moveActionMap.Enable();
                BreakableWall.isTriggerBox= false;
                playerController.enabled = true;
            break;

            case "Side wall":
                moveActionMap.Disable();
            break;

            case "Void":
                GameOver();
            break;

            case "Enemy":
                if (slinging)
                {
                    StartCoroutine(EnemyCollisionZoomOut());
                    Vector2 direction = collision.transform.position - transform.position;
                    collision.rigidbody.AddForce(rigidBody.linearVelocityX * direction.normalized, ForceMode2D.Impulse);
                    rigidBody.linearVelocityX = 0;
                }
                else
                {
                    GameOver();
                }
            break;

            case "Catapult collider":
                catapultBehaviour = collision.gameObject.GetComponentInParent<CatapultBehaviour>();
                catapultBehaviour.CatapultBehaviourStart();
            break;
        }
    }
    public void GameOver()
    {
        SceneManager.LoadScene(2);
        gameObject.SetActive(false);
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

    
    public void PlaceBombBehaviourCheck(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 forward = transform.TransformDirection(Vector2.right);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, forward, interactionRange, interactableLayer);
            if (hit.collider != null && PickedUpObjects.Count != 0)
            {
                // Interaction when placing bomb on the catapult
                if (hit.collider.gameObject.name == "Bomb catapult")
                {
                    catapultBehaviour = hit.collider.GetComponent<CatapultBehaviour>();
                    catapultBehaviour.CatapultBehaviourStart();
                    PlaceBomb(catapultBombSpawn.transform);

                }
                else if (hit.collider.gameObject.CompareTag("Breakable wall"))
                {
                    StartCoroutine(PickedUpObjects[0].GetComponent<BombBehaviour>().ExplodeBomb());
                    PlaceBomb(transform);
                }
            }
        }
    }

    private void PlaceBomb(Transform spawnPos)
    {
        PickedUpObjects[0].transform.position = spawnPos.position;
        PickedUpObjects[0].gameObject.SetActive(true);
        PickedUpObjects.RemoveAt(0);
    }

    private IEnumerator EnemyCollisionZoomOut()
    {
        mainCamera.orthographicSize = defaultCameraSize + enemyZoomOutAmount;
        yield return new WaitForSeconds(3);
        mainCamera.orthographicSize = defaultCameraSize;
    }
}
