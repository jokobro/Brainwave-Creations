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

    private bool timerStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        joint = GetComponent<HingeJoint2D>();
        motor = joint.motor;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStart)
        {
          joint.useMotor = false;
          motor.motorSpeed = motorSpeed;
          motor.maxMotorTorque = motorForce;
          joint.motor = motor;
          joint.useMotor = true;
          StartCoroutine(ResetCatapult());
        }
    }

    private IEnumerator ResetCatapult()
    {
        timerStart = false;
        WaitForSeconds wait = new WaitForSeconds(resetTimer);
        yield return wait;
        Debug.Log("reset");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        timerStart = true;
    }
}
