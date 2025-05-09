using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStructure : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Void"))
        {
            SceneManager.LoadScene(3);
        }
    }
}
