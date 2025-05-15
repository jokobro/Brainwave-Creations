using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CatapultBehaviourBomb : MonoBehaviour
{
    private HingeJoint2D joint;
    private JointMotor2D motor;
    private Camera mainCamera;
    private Rigidbody2D playerRb;
    public CatapultBehaviour cataPultBehaviour;
    private float defaultCameraSize;

    [Header("Motor properties")]
    public float motorForce;
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
        cataPultBehaviour = GetComponent<CatapultBehaviour>();
        joint = GetComponent<HingeJoint2D>();
        motor = joint.motor;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        defaultCameraSize = mainCamera.orthographicSize;
    }
    // turns the motor of the hingeJoint off and applies all the multiplier variables, and then turns it on to apply all of it at once.
    private IEnumerator LaunchCatapult()
    {
        SliderUI.SetActive(true);
      
        yield return new WaitUntil(() => playerAimInput == true);
        joint.useMotor = false;
        joint.useLimits = false;
        motor.motorSpeed = motorSpeed;
        motor.maxMotorTorque = motorForce;
        joint.motor = motor;      
        joint.useMotor = true;
        playerRb.AddForce(location.transform.position - transform.position.normalized * motorForce);
        StartCoroutine(ResetCatapult());    
    }
    // waits a set amount of seconds and then sets the limit in degrees back to the starting position
    private IEnumerator ResetCatapult()
    {
        WaitForSeconds wait = new WaitForSeconds(resetTimer);
        yield return wait;
        joint.useLimits = true;
        SliderUI.SetActive(false);
    }

    private IEnumerator CameraZoomOut()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(waitForZoomOut);
        mainCamera.orthographicSize = defaultCameraSize +zoomOutAmount;
        yield return waitForSeconds;
        mainCamera.orthographicSize = defaultCameraSize;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerRb.GetComponent<PlayerController>().enabled = false;
        playerAimInput=false;
        StartCoroutine(CameraZoomOut());
        StartCoroutine(LaunchCatapult());
    }    
}
