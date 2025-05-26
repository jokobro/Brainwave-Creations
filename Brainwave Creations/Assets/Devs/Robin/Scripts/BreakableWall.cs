using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;
public class BreakableWall : MonoBehaviour
{
    BoxCollider2D myCollider;
    PlayerController playerController;
    private void Awake()
    {
      myCollider=GetComponent<BoxCollider2D>();   
      playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {    
        if(playerController.slinging)
        {
           TriggerSet();
        }
    }
    public void TriggerSet()
    {
        switch (myCollider.isTrigger)
        {
            case false:
                myCollider.isTrigger = true;
            break;

            case true:
                myCollider.isTrigger = false;
            break;
        }
    }
} 
