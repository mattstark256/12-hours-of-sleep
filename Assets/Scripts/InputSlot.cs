using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSlot : MonoBehaviour
{
    [SerializeField]
    private Action action;
    public Action GetAction() { return action; }

    private InputKey inputKey;
    public InputKey GetInputKey() { return inputKey; }
    public void SetInputKey(InputKey _inputKey) { inputKey = _inputKey; }
}
