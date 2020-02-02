using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Don't teleport the player while they could be in an interactable!


public class Interactor : MonoBehaviour
{
    private Interactable currentInteractable;


    private void Update()
    {
        if (currentInteractable!= null)
        {
            if (InputMapper.Instance.GetButtonDown(Action.Interact))
            {
                currentInteractable.Interact();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        currentInteractable = null;
    }
}
