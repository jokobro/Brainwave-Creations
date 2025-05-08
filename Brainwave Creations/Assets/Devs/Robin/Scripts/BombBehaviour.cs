using UnityEngine;
using UnityEngine.Rendering;

public class BombBehaviour : MonoBehaviour
{
    [SerializeField] float explosionForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy structure"))
        {
            Rigidbody2D rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
            rb2D.AddForce(transform.forward * explosionForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
}
