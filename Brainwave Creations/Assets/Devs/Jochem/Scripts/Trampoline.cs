using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpforce;
    [SerializeField] Transform direction;
    Rigidbody2D playerRigidbody;
    PlayerController playerController;
    private float launchForce = 10f;
    Vector3 launchDirection = Vector3.up;

    private void Awake()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (gameObject.name)
        {
            case "DirectionalLaunchpad":
                if (collision.gameObject.CompareTag("Player"))
                {
                    playerController.slinging = true;
                    Vector2 vectorDirection = direction.position - transform.position;
                    if (vectorDirection.x != 0 || vectorDirection.y < 0) playerController.enabled = false;
                    playerRigidbody.AddForce(jumpforce * vectorDirection.normalized, ForceMode2D.Impulse);
                }
                break;
            case "HeightDependingLaunchpad":
                if (collision.gameObject.CompareTag("Player"))
                {
                    playerRigidbody.AddForce(launchForce * launchDirection);
                }
                break;
        }
    }
}
