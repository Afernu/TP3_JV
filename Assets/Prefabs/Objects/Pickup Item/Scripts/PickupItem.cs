using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        // Add logic to handle the pickup
        // For example, add the item to the player's inventory, increase score, etc.

        Destroy(gameObject);
    }
}
