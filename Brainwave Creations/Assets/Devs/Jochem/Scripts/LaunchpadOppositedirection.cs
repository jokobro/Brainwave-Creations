using UnityEngine;

public class LaunchpadOppositedirection : MonoBehaviour
{
    private float launchForce = 10f;
    Rigidbody2D playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 launchDirection = transform.forward;
            Vector3 oppositeDirection = launchDirection * -1;
            playerRigidbody.AddForce(oppositeDirection * launchForce, ForceMode2D.Impulse);


        }

        if (collision.gameObject.CompareTag("Player"))
        {
            playerRigidbody.AddForce(transform.forward);


        }

    }
}
