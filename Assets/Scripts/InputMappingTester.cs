using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMappingTester : MonoBehaviour
{
    InputMapper inputMapper;


    // Start is called before the first frame update
    void Start()
    {
        inputMapper = GetComponent<InputMapper>();

        //inputMapper.AddMapping(Action.Test, KeyCode.A);
        //Debug.Log("Added mapping for Test");
        //inputMapper.AddMapping(Action.MoveLeft, KeyCode.S);
        //Debug.Log("Added mapping for MoveLeft");

        //inputMapper.RemoveMapping(Action.Test);
        //Debug.Log("Removed mapping for Test");
    }


    // Update is called once per frame
    void Update()
    {
        if (inputMapper.GetButtonDown(Action.Interact))
        {
            Debug.Log("Interact button pressed");
        }   
        if (inputMapper.GetButtonDown(Action.MoveLeft))
        {
            Debug.Log("Move Left button pressed");
        }
    }
}
