using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Powerable
{
    [SerializeField]
    private float openDistance = 3;
    [SerializeField]
    private float openDuration = 2;

    float openFraction = 0;

    private Vector3 startPos;

    //enum DOORSTATE
    //{
    //    OPENING,
    //    CLOSING,
    //    STATIONARY
    //};

    //DOORSTATE doorCond;
    //Vector3 start, end;
    //SpriteRenderer spriteRenderer;
    //Rigidbody2D body;
    //float startX, startY, closeY, currentPosX, speed;

    //float openDuration = 2;

    // Start is called before the first frame update
    void Start()
    {
        //doorCond = DOORSTATE.CLOSING;
        //start.x = transform.position.x;
        //start.y = transform.position.y;
        //end.x = start.x;
        //end.y = startY + 1.5f;
        //currentPosX = startX;
        //speed = .2f;

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (doorCond == DOORSTATE.OPENING)
        //{
        //    openAmount += Time.deltaTime / openDuration;
        //}
        //else
        //{
        //    openAmount -= Time.deltaTime / openDuration;
        //}
        //openAmount = Mathf.Clamp01(openAmount);
        if (powered)
        {
            openFraction += Time.deltaTime / openDuration;
        }
        else
        {
            openFraction -= Time.deltaTime / openDuration;
        }
        openFraction = Mathf.Clamp01(openFraction);

        transform.position = startPos + openFraction * Vector3.up * openDistance;
    }



    //public void Open()
    //{
    //    doorCond = DOORSTATE.OPENING;
    //}


    //public void Close()
    //{
    //    doorCond = DOORSTATE.CLOSING;
    //}
}
