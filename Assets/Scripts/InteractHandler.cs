using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    GameObject currentObj = null;
  //  public bool active = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.CompareTag ("Interactable"))
        {
            currentObj = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (collision.gameObject == currentObj)
            {
                currentObj = null;
            }
        }
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Interact") && currentObj)
        //{
        //    //open door
        //    Debug.Log("pressed");
        //    door.GetComponent<Door>().Open();
        //    //currentObj.SendMessage("activate");
        //}
    }
}
