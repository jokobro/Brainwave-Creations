using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BombBehaviour : MonoBehaviour
{ 
    [Header("Exploding bomb variables")]
    public float explosionTime;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask hitLayer;
    private Animator explosionEffect;
    private void Awake()
    {
        explosionEffect = GetComponent<Animator>();
    }

    public IEnumerator ExplodeBomb()
    {
        yield return new WaitForSeconds(explosionTime);
        //for the explosion animation
        explosionEffect.SetBool("boom", true);
        gameObject.transform.localScale = new Vector3(1,1,1);
        //checks if the overlap circle hits the player or the wall and destroys it
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(transform.position, explosionRadius, hitLayer);
        for(int i = 0; i < hitCollider.Length; i++)
        {
            if (hitCollider[i].gameObject.CompareTag("Player"))
            {
                hitCollider[i].GetComponent<PlayerController>().GameOver();
            }
            else if (hitCollider[i].gameObject.CompareTag("Breakable wall"))
            {
                hitCollider[i].gameObject.SetActive(false);
            }        
        }    
        //for the animation to be done playing
        yield return new WaitForSeconds(.6f);
        gameObject.SetActive(false);     
    }
}
