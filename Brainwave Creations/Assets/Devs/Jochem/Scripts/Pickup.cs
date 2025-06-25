using UnityEngine;

public class Pickup : MonoBehaviour
{
    private int id = 0;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           playerController.PickupThrowableObjects(id, this.gameObject);
           id = 1;
        }
    }
}
