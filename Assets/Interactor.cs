using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private bool interactPressed = false;
    private bool interactPressed2 = false; // interactPressed needs to be reset on FixedUpdate, but FixedUpdate comes before OnTriggerStay2D, so I use this to communicate between them


    private void Update()
    {
        //interactPressed = Input.GetKeyDown(KeyCode.E);
        if (InputMapper.Instance.GetButtonDown(Action.Interact)) interactPressed = true;
    }

    private void FixedUpdate()
    {
        interactPressed2 = interactPressed;
        interactPressed = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            if (interactPressed2)
            {
                interactable.Interact();
            }
        }

    }

}
