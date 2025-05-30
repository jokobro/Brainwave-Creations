using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CatapultBehaviour : MonoBehaviour
{
    private HingeJoint2D joint;
    private JointMotor2D motor;
    private Camera mainCamera;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private float defaultCameraSize;

    [Header("Motor properties")]
    [HideInInspector]public float motorForce;
    public float maxMotorForce;
    public bool playerAimInput;
    [SerializeField] private GameObject SliderUI;
    [SerializeField] private float motorSpeed;
    [SerializeField] private float resetTimer;
    [SerializeField] Transform location;
    

    [Header("zoom properties")]
    [SerializeField] private float waitForZoomOut;
    [SerializeField] float zoomOutAmount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerController = FindAnyObjectByType<PlayerController>();
        joint = GetComponent<HingeJoint2D>();
        motor = joint.motor;
        defaultCameraSize = mainCamera.orthographicSize;
    }
    // turns the motor of the hingeJoint off and applies all the multiplier variables, and then turns it on to apply all of it at once.
    private IEnumerator LaunchCatapult()
    {
        SliderUI.SetActive(true);
        playerController.enabled = false;
      
        yield return new WaitUntil(() => playerAimInput == true);
        joint.useMotor = false;
        joint.useLimits = false;
        motor.motorSpeed = motorSpeed;
        motor.maxMotorTorque = motorForce;
        joint.motor = motor;      
        joint.useMotor = true;
        playerRb.AddForce(location.transform.position - playerRb.transform.position.normalized * motorForce);
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
        WaitForSeconds waitForSeconds = new WaitForSeconds(waitForZoomOut);
        mainCamera.orthographicSize = defaultCameraSize +zoomOutAmount;
        yield return waitForSeconds;
        mainCamera.orthographicSize = defaultCameraSize;
    }

    public void CatapultBehaviourStart()
    {
        StartCoroutine(LaunchCatapult());
        StartCoroutine(CameraZoomOut());
    }
   
}
