using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int id;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.PickupThrowableObjects(id, this.gameObject);
            }
        }
    }
}
