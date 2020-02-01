using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool lit = false;
    public void SetLit(bool _lit) { lit = _lit; }
    public bool IsLit() { return lit; }
}
