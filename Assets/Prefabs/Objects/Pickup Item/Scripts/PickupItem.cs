using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour
{
    private AudioSource pickupSound;

    private void Start()
    {
        pickupSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        pickupSound.Play();

        if (GetComponent<Renderer>()) GetComponent<Renderer>().enabled = false;
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;

        StartCoroutine(DestroyAfterSound());
    }

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(pickupSound.clip.length); // Wait for the length of the sound
        Destroy(gameObject);
    }
}
