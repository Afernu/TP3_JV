using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float range = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);

            foreach(Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable obj))
                {
                    obj.Interact();
                }
            }
        }
    }
    
    public IInteractable GetInteractableObject()
    {
        float range = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable obj))
            {
                return obj;
            }
        }
        return null;
    }
}
