using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public CatapultBehaviour catapultBehaviour;
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask groundLayer;
    Transform checkPoint;
    Rigidbody2D rigidBody;
    PlayerController playerController;
    Camera mainCamera;
    Animator animator;
    InputActionMap moveActionMap;
    PlayerInput input;

    //variables
    float defaultCameraSize;
    Vector2 inputMovement;

    [Header("Player Settings")]
    [HideInInspector]public bool slinging = false;
    [HideInInspector]public bool isGrounded = false;
    [SerializeField] float jumpForce;
    [SerializeField] int interactionRange; 
    [SerializeField] float enemyZoomOutAmount;
    [SerializeField] List<GameObject> PickedUpObjects = new List<GameObject>(); 
    private float moveSpeed = 6f;
   
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
        input = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        Movement();
        GroundCheck();
    }
    private void Movement()
    {
        rigidBody.linearVelocity = new Vector2(inputMovement.x * moveSpeed, rigidBody.linearVelocity.y);
    }
    public void HandleMoving(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
        animator.SetFloat("IsMoving", inputMovement.x);

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
            StartCoroutine(HandleJumpAnim());
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            rigidBody.AddForce(rigidBody.linearVelocity);
        }
    }
    public void GroundCheck()
    {
        // shoots a raycast to the ground, if it isnt touching the ground the isground bool is false
        animator.SetBool("IsGrounded", isGrounded);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        if (hit) 
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false; 
        }    
    }
    private IEnumerator HandleJumpAnim()
    {
        //waits until the jumping anim is finished and then switches it off, it will afterwards switch into the falling anim
        animator.SetBool("IsJumping", true);
        yield return new WaitForSeconds(.4f);
        animator.SetBool("IsJumping", false);
    }

    public IEnumerator HandleSlingAnim()
    {
        animator.SetBool("IsSlinging", true);
        yield return new WaitForSeconds(.4f);
        animator.SetBool("IsSlinging", false);
    }

    public void DisablePlayer()
    {
        inputMovement.x = 0;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        input.enabled = false;
        animator.SetBool("IsGrounded", false);
        animator.SetFloat("IsMoving", 0);
    }
    private void PlayerGrounded()
    {
        slinging = false;
        input.enabled = true;
        playerController.enabled = true;
        BreakableWall.isTriggerBox = false;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {     
        switch (collision.gameObject.tag)
        {
            case "Ground":
                moveActionMap.Enable();
                PlayerGrounded();
            break;

            case "Void":
                GameOver();
            break;

            case "Enemy":
                if (slinging)
                {
                    StartCoroutine(EnemyCollisionZoomOut());
                }
                else
                {
                    GameOver();
                }
            break;

            case "Catapult collider":
                DisablePlayer();
                catapultBehaviour = collision.gameObject.GetComponentInParent<CatapultBehaviour>();
                transform.position = catapultBehaviour.spawnPos.position;
                catapultBehaviour.playerAimInput = false;
                catapultBehaviour.CatapultBehaviourStart();
                playerController.enabled = false;
            break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Side wall":
                if (!isGrounded)
                {
                    DisablePlayer();
                    playerController.enabled = false;
                    Debug.Log("side");
                }
            break;
            case "Check point":
                checkPoint = collision.gameObject.transform;
            break;
        }
    }
    public void GameOver()
    {
        if (checkPoint == null)
        {
            SceneManager.LoadScene("GameOver");
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.transform.position = checkPoint.transform.position;
            DisablePlayer();
        }
    }
    public void PickupThrowableObjects(int id, GameObject pickUp)
    {
        switch (id)
        {
            case 0:
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
                if (hit.collider.gameObject.CompareTag("Breakable wall"))
                {
                    StartCoroutine(PickedUpObjects[0].GetComponent<BombBehaviour>().ExplodeBomb());
                    PlaceBomb(hit.collider.transform);
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
