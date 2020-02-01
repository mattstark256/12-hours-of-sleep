using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    // editor feilds
    [SerializeField]
    private float playerSpeed = 10;
    [SerializeField]
    private float groundMovementSmoothing = 10; 
    [SerializeField]
    private float airMovementSmoothing = 10;

    [SerializeField]
    private GameObject debugPrefab;
    [SerializeField]
    LayerMask environment;

    [SerializeField]
    private float jumpVelocity = 20;
    [SerializeField]
    private float jumpCooldown = 0.1f;
    private float timeWhenLastJumped = 0;
    [SerializeField]
    private float coyoteTime = 0.1f;
   // private float timeWhenCoyoteKickedIn = 0;
    [SerializeField]
    private float timeSinceOnFloor = 0;

    [SerializeField]
    private float assumedTerminalVelocity = 100;

    // other untiy bullshit
    private GameObject player;
    private Rigidbody2D playerRB;

    // member variables
    private Vector2 movementDirection;
    private Bounds spriteBounds;
    float fallingVelocity;

   // flags for fixed update
   bool canJump = false;
    bool onFloor = false;
    bool onFloorLastFrame = false;
    bool beginJump = false;


    public Vector3 GetVelocity()
    {
        return playerRB.velocity;
    }

    public bool IsOnFloor()
    {
        return onFloor;
    }


    // Start is called before the first frame update
    void Start()
    {
      
        player = gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();

      

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        

    }
  
    void handleIfCanJump()
    {
        onFloorLastFrame = onFloor;
        bool inAirLastFrame = !onFloor;
        
       
            
        onFloor = Physics2D.Raycast(transform.position + Vector3.right*0.25f, Vector3.down, 0.1f, environment)
               || Physics2D.Raycast(transform.position + Vector3.left*0.25f, Vector3.down, 0.1f, environment);


        if (onFloor)
        {
            timeSinceOnFloor = coyoteTime;
        }
        else
        {
            timeSinceOnFloor = Mathf.Clamp(timeSinceOnFloor - Time.deltaTime, 0, coyoteTime);
        }




        //if(onFloorLastFrame &&!onFloor) // coyote time kick in
        //{
        //    timeWhenCoyoteKickedIn = Time.time;
            
        //}


      //  Instantiate(debugPrefab, transform.position + Vector3.right * 0.25f + Vector3.down*0.1f, Quaternion.identity);

        

        if (inAirLastFrame && onFloor)
        {
            if(fallingVelocity / assumedTerminalVelocity > 0.13f)
            {
                Debug.Log(fallingVelocity / assumedTerminalVelocity);
                CameraEffects.Instance.AddScreenShakeAndChromaticAberration(fallingVelocity/assumedTerminalVelocity);
            }
            //fallingVelocity = 0;
        }

        bool pretendWeAreOnFloor = timeSinceOnFloor > 0;


        bool notInJumpCooldownn = (Time.time - timeWhenLastJumped > jumpCooldown);

        canJump = (onFloor || pretendWeAreOnFloor) && notInJumpCooldownn;

        if (!onFloor && playerRB.velocity.y < 0)
        {
            fallingVelocity = -playerRB.velocity.y;
        }
    }

    void HandleInput() 
    {
        movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        bool jumpInput = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W); // replace this line with matt's call
        if (jumpInput && canJump)
        {
            beginJump = true;
            //canJump = false;
        }
    }

    private void FixedUpdate()
    {
        handleIfCanJump();

        Vector2 newVelocity = playerRB.velocity;

        if (beginJump)
        {
            beginJump = false;
            onFloor = false;
            timeWhenLastJumped = Time.time;
            newVelocity.y = jumpVelocity;
        }

        if (onFloor)
        {
            if (Mathf.Abs(movementDirection.x) > 0) // moving
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, groundMovementSmoothing * Time.fixedDeltaTime);
            }
            else
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed,2* groundMovementSmoothing * Time.fixedDeltaTime);

            }
        }
        else // jumping or falling
        {

            if (movementDirection.x != 0)
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, airMovementSmoothing * Time.fixedDeltaTime); 
            }

           // Instantiate(debugPrefab, transform.position, Quaternion.identity);
        }

        playerRB.velocity = newVelocity;
    }


}
