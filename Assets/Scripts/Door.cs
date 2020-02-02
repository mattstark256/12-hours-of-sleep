using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Powerable
{
    [SerializeField]
    private float openDistance = 3f;
    [SerializeField]
    private float openDuration = 2f;
    [SerializeField]
    private float closeDuration = 1f;

    float openFraction = 0f;

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
            if(openFraction > 0.95f)
            {
                AudioManager.Instance.StopLooping("door_movement");
            }
            else
            {
                AudioManager.Instance.SetLoopingAndPlay("door_movement");
            }

        }
        else
        {
            bool previouslyOpen = openFraction > 0;
            openFraction -= Time.deltaTime / closeDuration;
            if (previouslyOpen)
            {
                if (openFraction <= 0)
                {
                    AudioManager.Instance.StopLooping("door_movement");
                    AudioManager.Instance.Play("door_impact");
                }
                else
                {
                    AudioManager.Instance.SetLoopingAndPlay("door_movement");
                }
            }
        }
        openFraction = Mathf.Clamp01(openFraction);

        transform.position = startPos + openFraction * Vector3.up * openDistance;

        // Door closes after short period of time
        //if (openFraction >= 0.99 && powered)
        //{
        //    timeOpen -= Time.deltaTime;
        //    if (timeOpen <= 0)
        //    {
        //        powered = false;
        //        timeOpen = 3f;
        //    }
        //}
    }

    public override void SetPowered(bool _powered)
    {
        //if (powered && _powered==false)
        //{
        //    AudioManager.Instance.StopLooping("door_movement");
        //}

        base.SetPowered(_powered);
        
        if(_powered == false)
        {
            AudioManager.Instance.StopLooping("door_movement");
        }
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
