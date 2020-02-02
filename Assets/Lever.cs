using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    [SerializeField]
    private Powerable powerable;

    [SerializeField]
    private Sprite on;

    [SerializeField]
    private Sprite off;

    [SerializeField]
    private float timeToReset = 3f;
    [SerializeField]
    private bool doReset;



    private float resetCountdown;

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

        SetSwitchedState(!switched);
    }

    private void Update()
    {
        if (doReset)
        {
            resetCountdown -= Time.deltaTime;

            if (resetCountdown < 0)
            {
                SetSwitchedState(false);
            }
        }
    }

    private void SetSwitchedState(bool _switched)
    {
        Debug.Log("SWITCHED");
        switched = _switched;
        powerable.SetPowered(switched);

        if (switched)
        {
            resetCountdown = timeToReset;
        }



        if (switched)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = on;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = off;
        }
    }
}
