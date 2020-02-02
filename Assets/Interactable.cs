using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual bool CanInteract()
    {
        return true;
    }


    public virtual void Interact()
    {
        Debug.Log("interacting");
    }
}
