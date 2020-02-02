using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField]
    private Powerable powerable;

    // Levers can only be interacted with once (for now).
    private bool switched = false;


    //public override bool CanInteract()
    //{
    //    return !switched;
    //}

    public override void Interact()
    {
        base.Interact();

        switched = !switched;
        powerable.SetPowered(switched);
    }
}
