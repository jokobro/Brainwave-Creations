using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
       var currentSceneIndex = SceneManager.GetActiveScene();
        if (currentSceneIndex.name != "Level 2")
        {
           SceneManager.LoadScene("Level " + currentSceneIndex.buildIndex.ToString());
        }
        else
        {
            SceneManager.LoadScene("VictoryScene");
        }
      
    }
}
