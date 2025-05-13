using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float jumpforce = 5f;
    Rigidbody2D playerRigidbody;
    
    private void Awake()
    {
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerRigidbody.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
        }
    }
}
