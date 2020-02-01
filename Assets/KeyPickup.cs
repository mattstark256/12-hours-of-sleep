using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField]
    private InputKey keyPrefab;
    public InputKey GetKeyPrefab()    { return keyPrefab;  }
}
