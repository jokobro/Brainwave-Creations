using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private int id;
    private Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                myCollider.isTrigger = false;
                playerController.PickupThrowableObjects(id, this.gameObject);
                Debug.Log("ja");
            }
        }
    }
}
