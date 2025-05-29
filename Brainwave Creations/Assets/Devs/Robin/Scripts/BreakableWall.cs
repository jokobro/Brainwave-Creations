using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public  class BreakableWall: MonoBehaviour
{
    BoxCollider2D myCollider;
    public static bool isTriggerBox = false;

    private void Awake()
    {
        myCollider= GetComponent<BoxCollider2D>();
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
        Destroy(gameObject);
        SceneManager.LoadScene(3);
    }
} 
