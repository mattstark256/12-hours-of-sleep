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
    private float timeWhenCoyoteKickedIn = 0;

    // other untiy bullshit
    private GameObject player;
    private Rigidbody2D playerRB;

    // member variables
    private Vector2 movementDirection;
    private Bounds spriteBounds;

    // flags for fixed update
    bool canJump = false;
    bool onFloor = false;
    bool onFloorLastFrame = false;
    bool beginJump = false;
    

    // Start is called before the first frame update
    void Start()
    {
      
        player = gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();

        spriteBounds = player.GetComponent<SpriteRenderer>().sprite.bounds;

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();



    }
  
    void handleIfCanJump()
    {
        
        onFloorLastFrame = onFloor;
        onFloor = Physics2D.Raycast(player.transform.position + spriteBounds.center - Vector3.right * spriteBounds.extents.x * 0.95f + Vector3.down * (spriteBounds.extents.y * 1.1f), Vector2.down, 0, environment)
               || Physics2D.Raycast(player.transform.position + spriteBounds.center + Vector3.right * spriteBounds.extents.x * 0.95f + Vector3.down * (spriteBounds.extents.y * 1.1f), Vector2.down, 0, environment);

        bool notInJumpCooldownn = (Time.time - timeWhenLastJumped > jumpCooldown);

        if(onFloorLastFrame &&!onFloor) // coyote time kick in
        {
            timeWhenCoyoteKickedIn = Time.time;
        }

        bool pretendWeAreOnFloor = (Time.time - timeWhenCoyoteKickedIn < coyoteTime);

        canJump = (onFloor || pretendWeAreOnFloor) && notInJumpCooldownn;


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

        if (onFloor)
        {
            //Debug.Log("on floor");
            if (Mathf.Abs(movementDirection.x) > 0) // moving
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, groundMovementSmoothing * Time.fixedDeltaTime);
            }
            else
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed,2* groundMovementSmoothing * Time.fixedDeltaTime);

            }
            if (beginJump)
            {
                beginJump = false;
                onFloor = false;
                timeWhenLastJumped = Time.time;
                newVelocity.y += jumpVelocity;
            }
        }
        else // jumping or falling
        {

            if (movementDirection.x != 0)
            {
                //Debug.Log("movement direction not 0");
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, airMovementSmoothing * Time.fixedDeltaTime); 
            }

            Instantiate(debugPrefab, transform.position, Quaternion.identity);
        }

        playerRB.velocity = newVelocity;
    }


}
