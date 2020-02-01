using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentPickup : MonoBehaviour
{
    [SerializeField]
    private InputFragment fragmentPrefab;
    public InputFragment GetFragmentPrefab() { return fragmentPrefab; }
}
