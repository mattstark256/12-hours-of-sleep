using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField]
    private Powerable powerable;

    [SerializeField]
    GameObject on;

    [SerializeField]
    GameObject off;

    // Levers can only be interacted with once (for now).
    private bool switched = false;


    //public override bool CanInteract()
    //{
    //    return !switched;
    //}

    public override void Interact()
    {
        AudioManager.Instance.Play("lever");
        base.Interact();

        switched = !switched;
        powerable.SetPowered(switched);
    }

    private void Update()
    {
        if (switched)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = on.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = off.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
