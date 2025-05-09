using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class CatapultBehaviour : MonoBehaviour
{
    private HingeJoint2D joint;
    private JointMotor2D motor;

    [SerializeField] private float motorSpeed;
    [SerializeField] private float motorForce;
    [SerializeField] private float resetTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        joint = GetComponent<HingeJoint2D>();
        motor = joint.motor;
    }

    // Update is called once per frame
    void Update()
    {
       
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
        // for now is on touch
        LaunchCatapult();
    }
}
