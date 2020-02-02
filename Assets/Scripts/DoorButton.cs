using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField]
    GameObject on;

    [SerializeField]
    GameObject off;

    [SerializeField]
    GameObject door;


    float timeActive = 3f;
    bool doorOpen = false;
    bool interacted = false;
    //Door open;

    public void activate()
    {
        //gameObject.SetActive(false);
        //open.Open();
        doorOpen = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = on.GetComponent<SpriteRenderer>().sprite;
        Debug.Log("changed");
        //change animation
    }


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = off.GetComponent<SpriteRenderer>().sprite;
        doorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact") && interacted)
        {
            Debug.Log("pressed");
            //door.GetComponent<Door>().Open();
            doorOpen = true;
        }

        if (doorOpen)
        {
            timeActive -= Time.deltaTime;
            if (timeActive < 0)
            {
                //call close door function
                //change animation
                //door.GetComponent<Door>().Close();
                gameObject.GetComponent<SpriteRenderer>().sprite = off.GetComponent<SpriteRenderer>().sprite;
                doorOpen = false;
                timeActive = 3f;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interacted = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interacted = false;
        }
    }
}
