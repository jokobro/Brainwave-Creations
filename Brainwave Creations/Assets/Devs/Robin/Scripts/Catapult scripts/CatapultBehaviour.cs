using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CatapultBehaviour : MonoBehaviour
{
    // component references
    private HingeJoint2D joint;
    private JointMotor2D motor;
    private Camera mainCamera;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private FollowPlayer followPlayer;
    private Animator playerAnimator;
    //variables
    private float defaultCameraSize;

    [Header("Motor properties")]
    [HideInInspector] public bool playerAimInput;
    [HideInInspector] public float slingPower;
    public Transform spawnPos;
    public float maxSlingPower;

    [SerializeField] private float motorForce;
    [SerializeField] private GameObject SliderUI;
    [SerializeField] private float motorSpeed;
    [SerializeField] private float resetTimer;
    [SerializeField] Transform direction;

    [Header("zoom properties")]
    [SerializeField] float zoomOutAmount;
    [SerializeField] float waitUntilZoomIn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // tag finding references
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // get component references
        playerController = FindAnyObjectByType<PlayerController>();
        playerAnimator = playerRb.GetComponent<Animator>();
        joint = GetComponent<HingeJoint2D>();
        followPlayer = mainCamera.GetComponent<FollowPlayer>();
        // variable set
        motor = joint.motor;
        defaultCameraSize = mainCamera.orthographicSize;
    }
    // turns the motor of the hingeJoint off and applies all the multiplier variables, and then turns it on to apply all of it at once.
    private IEnumerator LaunchCatapult()
    {
        BreakableWall.isTriggerBox= true;
        SliderUI.SetActive(true);    
        yield return new WaitUntil(() => playerAimInput == true);
        //setting player variables
        playerController.DisablePlayer();
        playerController.enabled = false;
        playerController.slinging = true;
        StartCoroutine(playerController.HandleSlingAnim());
        // setting motor properties
        joint.useMotor = false;
        joint.useLimits = false;
        motor.motorSpeed = motorSpeed;
        motor.maxMotorTorque = motorForce;
        joint.motor = motor;      
        joint.useMotor = true;
        // adding player force and reset catapult
        Vector2 vectorDirection = direction.position - playerRb.transform.position;
        playerRb.AddForce(slingPower * vectorDirection.normalized,ForceMode2D.Impulse);
        StartCoroutine(ResetCatapult());    
    }
    // waits a set amount of seconds and then sets the limit in degrees back to the starting position
    private IEnumerator ResetCatapult()
    {
        WaitForSeconds wait = new WaitForSeconds(resetTimer);
        yield return wait;
        followPlayer.playerTarget = playerRb.transform;
        joint.useLimits = true;
        playerAimInput = false;
        SliderUI.SetActive(false);
    }

    private IEnumerator CameraZoomOut()
    {
        mainCamera.orthographicSize = defaultCameraSize + zoomOutAmount;
        followPlayer.playerTarget = direction;
        mainCamera.farClipPlane = 1000000000000000000000f;
        yield return new WaitUntil(() => playerAimInput == true);
        yield return new WaitForSeconds(waitUntilZoomIn);
        mainCamera.orthographicSize = defaultCameraSize;
        mainCamera.farClipPlane = 1000;
    }

    public void CatapultBehaviourStart()
    {
        StartCoroutine(LaunchCatapult());
        StartCoroutine(CameraZoomOut());
    }
}
