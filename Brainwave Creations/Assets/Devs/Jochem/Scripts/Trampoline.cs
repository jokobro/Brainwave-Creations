using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trampoline : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float jumpforce;
    [SerializeField] Transform direction;
    [SerializeField] private float launchForce;
    [Header("References")]
    Rigidbody2D playerRigidbody;
    PlayerController playerController;
    PlayerInput input;
    Animator playerAnimator;
    private void Awake()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerAnimator = playerRigidbody.GetComponent<Animator>();
        playerController = FindAnyObjectByType<PlayerController>();
        input = playerController.GetComponent<PlayerInput>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //direction
        Vector2 vectorDirection = direction.position - transform.position;
        //player settings
        playerAnimator.SetBool("IsGrounded", false);
        playerController.slinging = true;
        playerController.enabled = true;
        input.enabled = true;
        StartCoroutine(playerController.HandleSlingAnim());
        switch (gameObject.name)
        {
            case "DirectionalLaunchpad":
                if (collision.gameObject.CompareTag("Player"))
                {   
                    if (vectorDirection.x != 0 || vectorDirection.y < 0) playerController.enabled = false; playerController.isGrounded = false; BreakableWall.isTriggerBox = true;
                    playerRigidbody.AddForce(jumpforce * vectorDirection.normalized, ForceMode2D.Impulse);
                }
                break;
            case "HeightDependingLaunchpad":
                if (collision.gameObject.CompareTag("Player"))
                {
                    float impactSpeed = MathF.Abs(collision.relativeVelocity.y);
                    float dynamicForce = impactSpeed * launchForce;
                    playerRigidbody.AddForce(dynamicForce * vectorDirection, ForceMode2D.Impulse);
                }
                break;
        }
    }
}
