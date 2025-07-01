
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BreakableWall : MonoBehaviour
{
    BoxCollider2D myColliders;
    public static bool isTriggerBox = false;
    private PlayerController playerController;

    private void Awake()
    {
        myColliders = GetComponent<BoxCollider2D>();
        playerController = FindAnyObjectByType<PlayerController>();
    }
    private void Update()
    {
        SetTrigger();
    }

    private void SetTrigger()
    {
        switch (isTriggerBox)
        {
          case true:
              myColliders.isTrigger= true;
          break;
          case false:
              myColliders.isTrigger= false;
          break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.slinging || collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

}
   




