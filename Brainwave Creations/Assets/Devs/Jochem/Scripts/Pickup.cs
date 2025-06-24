using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int id;
    private PlayerController playerController;
    private Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        playerController = FindAnyObjectByType<PlayerController>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           playerController.PickupThrowableObjects(id, this.gameObject);
           id = 2;
        }
    }
}
