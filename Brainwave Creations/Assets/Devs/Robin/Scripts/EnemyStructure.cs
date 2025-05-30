using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStructure : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
       SceneManager.LoadScene(3);
    }
}
