using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CatapultBehaviour : MonoBehaviour
{
    // component references
    private HingeJoint2D joint;
    private JointMotor2D motor;
    private Camera mainCamera;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private GameObject[] breakableWalls;
    //variables
    private float defaultCameraSize;

    [Header("Motor properties")]
    public float motorForce;
    [HideInInspector] public bool playerAimInput;
    [HideInInspector] public float slingPower;
    public float maxSlingPower;
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
        breakableWalls = GameObject.FindGameObjectsWithTag("Breakable wall");
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // get component references
        playerController = FindAnyObjectByType<PlayerController>();
        joint = GetComponent<HingeJoint2D>();
        // variable set
        motor = joint.motor;
        defaultCameraSize = mainCamera.orthographicSize;
    }
    // turns the motor of the hingeJoint off and applies all the multiplier variables, and then turns it on to apply all of it at once.
    private IEnumerator LaunchCatapult()
    {
        foreach (GameObject walls in breakableWalls)walls.GetComponent<BreakableWall>().TriggerSet();
        SliderUI.SetActive(true);
      
        yield return new WaitUntil(() => playerAimInput == true);
        //setting player variables
        playerController.slinging = true;
        playerController.enabled = false;
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
        joint.useLimits = true;
        playerAimInput = false;
        SliderUI.SetActive(false);
    }

    private IEnumerator CameraZoomOut()
    {
        mainCamera.orthographicSize = defaultCameraSize + zoomOutAmount;
        yield return new WaitUntil(() => playerAimInput == true);
        yield return new WaitForSeconds(waitUntilZoomIn);
        mainCamera.orthographicSize = defaultCameraSize;
    }

    public void CatapultBehaviourStart()
    {
        StartCoroutine(LaunchCatapult());
        StartCoroutine(CameraZoomOut());
    }
}
