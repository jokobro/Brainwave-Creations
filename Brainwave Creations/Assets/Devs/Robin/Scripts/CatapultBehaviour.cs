using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CatapultBehaviour : MonoBehaviour
{
    private HingeJoint2D joint;
    private JointMotor2D motor;
    private Camera mainCamera;
    private float defaultCameraSize;

    [Header("Motor properties")]
    [SerializeField] private float motorSpeed;
    [SerializeField] private float motorForce;
    [SerializeField] private float resetTimer;

    [Header("zoom out properties")]
    [SerializeField] private float waitForZoomOut;
    [SerializeField] float zoomOutAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        joint = GetComponent<HingeJoint2D>();
        motor = joint.motor;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        defaultCameraSize = mainCamera.orthographicSize;
    }
    // turns the motor of the hingeJoint off and applies all the multiplier variables, and then turns it on to apply all of it at once.
    private void LaunchCatapult()
    {
       joint.useMotor = false;
       joint.useLimits = false;
       motor.motorSpeed = motorSpeed;
       motor.maxMotorTorque = motorForce;
       joint.motor = motor;
       joint.useMotor = true;
       StartCoroutine(ResetCatapult());    
    }
    
    // waits a set amount of seconds and then sets the limit in degrees back to the starting position
    private IEnumerator ResetCatapult()
    {
        WaitForSeconds wait = new WaitForSeconds(resetTimer);
        yield return wait;
        joint.useLimits = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // for the player to use during platforming
        if (gameObject.name == "Player catapult")
        {
            StartCoroutine(CameraZoomOut());
            LaunchCatapult();
        }
        // to slingshot bombs at the enemy structure
        else if(gameObject.name == "Bomb catapult" && collision.gameObject.tag == "Bomb")
        {
            StartCoroutine(CameraZoomOut());
            LaunchCatapult();
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
