using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;
public class BreakableWall : PlayerController
{
    BoxCollider2D myCollider;
    private void Awake()
    {
      myCollider=GetComponent<BoxCollider2D>();   
    }

    private void Update()
    {    
        if(isGrounded)
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
