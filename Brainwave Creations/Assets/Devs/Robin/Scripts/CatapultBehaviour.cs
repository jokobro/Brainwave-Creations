using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CatapultBehaviour : MonoBehaviour
{
    private HingeJoint2D joint;
    private JointMotor2D motor;
    private Camera mainCamera;
    private float defaultCameraSize;
    private Rigidbody2D playerRb;
    private GameObject UiDocumentObj;
    

    [Header("Motor properties")]
    [SerializeField] private float motorSpeed;
    [SerializeField] private float motorForce;
    [SerializeField] private float resetTimer;
    [SerializeField] private bool playerAimInput = false;
    [SerializeField] Transform location;

    [Header("zoom properties")]
    [SerializeField] private float waitForZoomOut;
    [SerializeField] float zoomOutAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        UiDocumentObj = GameObject.Find("UIDocument");
        UiDocumentObj.SetActive(false);
        joint = GetComponent<HingeJoint2D>();
        motor = joint.motor;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        defaultCameraSize = mainCamera.orthographicSize;
    }
    // turns the motor of the hingeJoint off and applies all the multiplier variables, and then turns it on to apply all of it at once.
    private IEnumerator LaunchCatapult()
    {
        yield return new WaitUntil(() => playerAimInput == true);
        joint.useMotor = false;
        joint.useLimits = false;
        motor.motorSpeed = motorSpeed;
        motor.maxMotorTorque = motorForce;
        joint.motor = motor;      
        joint.useMotor = true;
        playerRb.GetComponent<PlayerController>().enabled = false;
        playerRb.AddForce(location.transform.position - playerRb.transform.position.normalized * motorForce);
        StartCoroutine(ResetCatapult());    
    }
    // waits a set amount of seconds and then sets the limit in degrees back to the starting position
    private IEnumerator ResetCatapult()
    {
        WaitForSeconds wait = new WaitForSeconds(resetTimer);
        yield return wait;
        joint.useLimits = true;
        UiDocumentObj.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // for the player to use during platforming
        if (gameObject.name == "Player catapult")
        {
            StartCoroutine(CameraZoomOut());
            StartCoroutine(LaunchCatapult());
            UiDocumentObj.SetActive(true);
        }
        // to slingshot bombs at the enemy structure
        else if(gameObject.name == "Bomb catapult" && collision.gameObject.tag == "Bomb")
        {
            UiDocumentObj.SetActive(true);
            StartCoroutine(CameraZoomOut());
            StartCoroutine(LaunchCatapult());
        }
    }

    private IEnumerator CameraZoomOut()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(waitForZoomOut);
        mainCamera.orthographicSize = defaultCameraSize +zoomOutAmount;
        yield return waitForSeconds;
        mainCamera.orthographicSize = defaultCameraSize;
    }
}
