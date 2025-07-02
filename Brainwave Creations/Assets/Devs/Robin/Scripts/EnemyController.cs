using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy properties")]
    [SerializeField] float speed;
    [SerializeField] int collateralForce;
    [SerializeField] LayerMask enviornmentLayer;
    private bool collided;
    PlayerController playerController;
    Rigidbody2D rb;
    private Animator smokeEffect;
    // Start is called before the first frame update
    private void Awake()
    {
        smokeEffect = GetComponent<Animator>();
        playerController = FindAnyObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyPathing();
    }

    private void EnemyPathing()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // checks if the enemyy is colliding with the player and if the player is slinging, then makes it be able to collider
        if (collision.gameObject.CompareTag("Player") && playerController.slinging)
        {
            collided = true;
            Vector2 direction = transform.position - collision.transform.position;
            rb.AddForce(collision.rigidbody.linearVelocityX * direction.normalized, ForceMode2D.Impulse);
            collision.rigidbody.linearVelocityX = 0;
        }

        if (collided && collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 collisionDirection = collision.transform.position - transform.position;
            collision.rigidbody.AddForce(collateralForce * collisionDirection.normalized, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == enviornmentLayer)
        {
            smokeEffect.SetTrigger("smoke");
            Destroy(gameObject,.3f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // rotates the enemy at the edge of a platform so it doesnt fall off and can keep patrolling
        if (collision.gameObject.CompareTag("Side wall"))
        {
            if (transform.rotation.y >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (transform.rotation.y < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
