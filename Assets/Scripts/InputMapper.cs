using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Action { MoveRight, MoveLeft, Jump, Crouch, Interact, Whip }


public class InputMapper : MonoBehaviour
{
    // Singleton
    private static InputMapper instance;
    public static InputMapper Instance { get { return instance; } }


    Dictionary<Action, KeyCode> actionKeyMapping = new Dictionary<Action, KeyCode>();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    public bool GetButtonDown(Action action)
    {
        return actionKeyMapping.ContainsKey(action) && Input.GetKeyDown(actionKeyMapping[action]);
    }


    public bool GetButton(Action action)
    {
        return actionKeyMapping.ContainsKey(action) && Input.GetKey(actionKeyMapping[action]);
    }


    public bool GetButtonUp(Action action)
    {
        return actionKeyMapping.ContainsKey(action) && Input.GetKeyUp(actionKeyMapping[action]);
    }


    public void AddMapping(Action action, KeyCode keyCode)
    {
        actionKeyMapping[action] = keyCode;
    }


    public void RemoveMapping(Action action)
    {
        actionKeyMapping.Remove(action);
    }
}
