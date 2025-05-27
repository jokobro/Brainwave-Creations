using UnityEngine;

public class ObjectAddScorTEsst : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AddScore(300);
        }
    }
}
