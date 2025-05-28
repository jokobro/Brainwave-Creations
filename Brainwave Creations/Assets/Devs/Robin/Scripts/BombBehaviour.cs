using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BombBehaviour : MonoBehaviour
{
    [Header("Catapult variables")]
    [SerializeField] float explosionForce;

    [Header("Exploding bomb variables")]
    public float explosionTime;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask hitLayer;


    private void Awake()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy structure"))
        {
            Rigidbody2D rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
            rb2D.AddForce(transform.forward * explosionForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }

    public IEnumerator ExplodeBomb()
    {
        yield return new WaitForSeconds(explosionTime);
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, explosionRadius, hitLayer);

        for(int i = 0; i < hitCollider.Length; i++)
        {
            if (hitCollider[i].gameObject.CompareTag("Player"))
            {
                hitCollider[i].GetComponent<PlayerController>().GameOver();
            }
            else
            {
                hitCollider[i].gameObject.SetActive(false);
            }        
        }    
        gameObject.SetActive(false);     
    }
}
