using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerable : MonoBehaviour
{
    protected bool powered = false;

    public virtual void SetPowered(bool _powered)
    {
        Debug.Log("setting powered state to " + _powered);

        powered = _powered;
    }
}
