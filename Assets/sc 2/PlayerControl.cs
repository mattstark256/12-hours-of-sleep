using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

   // [SerializeField]
   // GameObject audioManagerObject;
   // AudioManagerScript audioManager;

    [SerializeField]
    private float playerSpeed = 10;
    [SerializeField]
    private float jumpVelocity = 20;
    [SerializeField]
    private float airborneJumpAcceleration = 5;
    [SerializeField]
    private float horizontalSmoothingOnGround = 20;
    [SerializeField]
    private float horizontalSmoothingInAir = 5;
    [SerializeField]
    float initialCoyoteTime = 0.1f;

    private Rigidbody2D playerRB;

    public Vector2 directionOffset { get; private set; }

    bool jumping;
    bool jumpInitialPush;
    bool jumpHeld;

    bool onFloor;
    bool onFloorLastTick;
    [SerializeField]
    float currentCoyoteTime = 0;

    [SerializeField]
    private GameObject player;


    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
        //audioManager = audioManagerObject.GetComponent<AudioManagerScript>();
        onFloorLastTick = true;
    }



    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jumpInitialPush = true;
        }
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
        {
            jumpHeld = true;
        }
        
    }



    private void FixedUpdate()
    {
        Vector2 newVelocity = playerRB.velocity;

        if(currentCoyoteTime > 0)
        {
            currentCoyoteTime -= Time.fixedDeltaTime;
        }else if (currentCoyoteTime < 0)
        {
            currentCoyoteTime = 0;
        }

        if(Physics2D.Raycast(player.transform.position + new Vector3(-0.4f, 0, 0) + Vector3.down, Vector2.down*0.8f, 0) || Physics2D.Raycast(player.transform.position + new Vector3(+0.4f, 0, 0) + Vector3.down*0.8f, Vector2.down, 0)) // update: two rays now
        {
            onFloor = true;
           
            if((currentCoyoteTime < initialCoyoteTime) && (!jumpInitialPush || !jumpHeld))
                jumping = false;

            onFloorLastTick = true;
        }
        else
        {
            onFloor = false;
        }

      

        if (onFloor == false && onFloorLastTick == true)
        {
            onFloorLastTick = false;
            if (!jumping && (!jumpInitialPush||!jumpHeld))
            {
                currentCoyoteTime = initialCoyoteTime;
            }
            else
            {
                currentCoyoteTime = 0;
            }
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        directionOffset = new Vector2(horizontalInput, 0);

        if (onFloor) // jump from floor
        {
            newVelocity.x = Mathf.Lerp(playerRB.velocity.x, horizontalInput * playerSpeed, horizontalSmoothingOnGround * Time.fixedDeltaTime);

            if(Input.GetAxis("Horizontal") ==0)
            {
                newVelocity.x = 0;
            }

            if (jumpInitialPush)
            {
                jumpInitialPush = false;
                //if (Mathf.Abs(playerRB.velocity.y) < 15f)
                //{
                //    audioManager.PlayNextJump();
                //}
                newVelocity.y = jumpVelocity;
                currentCoyoteTime = 0;
                onFloor = false;
                jumping = true;
            }
        }
        else if(currentCoyoteTime > 0 && !onFloor && !jumping) // jump from coyote grace period
        {
            newVelocity.x = Mathf.Lerp(playerRB.velocity.x, horizontalInput * playerSpeed, horizontalSmoothingInAir * Time.fixedDeltaTime);
            if (jumpInitialPush && playerRB.velocity.y <= 0)
            {
                jumpInitialPush = false;
                //if (Mathf.Abs(playerRB.velocity.y) < 15f)
                //{
                //    audioManager.PlayNextJump();
                //}
                newVelocity.y = jumpVelocity;
                currentCoyoteTime = 0;
                onFloor = false;
                jumping = true;
            }
            else if (jumpHeld || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) // checks for both in case one is wrong, this is the best way i could think of                                                                             // although it doesn't acutually really matter if we miss a frame here
            {
                jumpHeld = false;
                newVelocity.y += airborneJumpAcceleration * Time.fixedDeltaTime;
            }
        }
        else // already in air
        {
            newVelocity.x = Mathf.Lerp(playerRB.velocity.x, horizontalInput * playerSpeed, horizontalSmoothingInAir * Time.fixedDeltaTime);

            if (jumpHeld || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) // checks for both in case one is wrong, this is the best way i could think of
                                                                                    // although it doesn't acutually really matter if we miss a frame here
            {
                jumpHeld = false;
                newVelocity.y += airborneJumpAcceleration * Time.fixedDeltaTime;
            }
        }

        playerRB.velocity = newVelocity;

        if (jumpInitialPush) // this should only be true for one frame / tick
        {
            jumpInitialPush = false;

        }
    }
}
