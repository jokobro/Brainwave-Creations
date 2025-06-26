
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public  class BreakableWall: MonoBehaviour
{
    BoxCollider2D myCollider;
    public static bool isTriggerBox = false;
    private PlayerController playerController;

    private void Awake()
    {
        myCollider= GetComponent<BoxCollider2D>();
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
                myCollider.isTrigger = isTriggerBox;
            break;
            case false:
                myCollider.isTrigger = isTriggerBox;
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
