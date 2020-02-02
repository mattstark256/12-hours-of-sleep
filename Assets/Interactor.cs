using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private bool interactPressed = false;
    private void Update()
    {
        interactPressed = Input.GetKeyDown(KeyCode.E);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            if (interactPressed)
            {
                interactable.Interact();
                interactPressed = false;
            }
        }

    }
}
