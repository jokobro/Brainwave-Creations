using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy properties")]
    [SerializeField] float speed;
    [SerializeField] int collateralForce;
    public bool collided;
    // Start is called before the first frame update

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

        if (collided && collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = collision.transform.position - transform.position;
            collision.rigidbody.AddForce( collateralForce * direction.normalized, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<EnemyController>().collided = true;
            Destroy(gameObject);
        }
        else if (collided && collision.gameObject.layer == 18)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // rotates the enemy at the edge of a platform so it doesnt fall off and can keep patrolling
        if (collision.gameObject.CompareTag("Side wall"))
        {
            if (transform.rotation.x == 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
