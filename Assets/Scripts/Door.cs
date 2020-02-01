using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    enum DOORSTATE
    {
        OPENING,
        CLOSING,
        STATIONARY
    };

    DOORSTATE doorCond;
    Vector3 start, end;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;
    float startX, startY, closeY, currentPosX, speed;

    float openAmount = 0;
    float openDuration = 2;

    // Start is called before the first frame update
    void Start()
    {
        doorCond = DOORSTATE.CLOSING;
        start.x = transform.position.x;
        start.y = transform.position.y;
        end.x = start.x;
        end.y = startY + 1.5f;
        currentPosX = startX;
        speed = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorCond == DOORSTATE.OPENING)
        {
            openAmount += Time.deltaTime / openDuration;
        }
        else
        {
            openAmount -= Time.deltaTime / openDuration;
        }
        openAmount = Mathf.Clamp01(openAmount);

        transform.position = Vector3.Lerp(start, end, openAmount);
    }


    public void Open()
    {
        doorCond = DOORSTATE.OPENING;
    }


    public void Close()
    {
        doorCond = DOORSTATE.CLOSING;
    }
}
