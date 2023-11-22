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
    private Animator animator;
    private bool isInteracting = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
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
                    animator.SetBool("IsPunching", true);
                    isInteracting = true;
                }
            }
        }
        else if (isInteracting)
        {
            animator.SetBool("IsPunching", false);
            isInteracting = false;
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
