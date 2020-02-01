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
    private float coyoteCountdown = 0;

    [SerializeField]
    private float assumedTerminalVelocity = 100;

    // other untiy bullshit
    private GameObject player;
    private Rigidbody2D playerRB;

    // member variables
    private Vector2 movementDirection;
    float fallingVelocity;

    // flags for fixed update
    bool canJump = false;
    bool onFloor = false;
    bool beginJump = false;


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
        bool onFloorLastFrame = onFloor;

        onFloor = Physics2D.Raycast(transform.position + Vector3.right * 0.25f, Vector3.down, 0.1f, environment)
               || Physics2D.Raycast(transform.position + Vector3.left * 0.25f, Vector3.down, 0.1f, environment);

        coyoteCountdown = (onFloor) ? coyoteTime : coyoteCountdown - Time.deltaTime;

        bool notInJumpCooldown = (Time.time - timeWhenLastJumped > jumpCooldown);
        canJump = (onFloor || coyoteCountdown > 0) && notInJumpCooldown;

        // Store the velocity in case they land on the ground in the next frame
        if (!onFloor && playerRB.velocity.y < 0)
        {
            fallingVelocity = -playerRB.velocity.y;
        }

        // If they've landed on the ground, do a camera shake
        if (!onFloorLastFrame && onFloor)
        {
            CameraEffects.Instance.AddScreenShake(fallingVelocity / assumedTerminalVelocity);
        }

        // Create object to vizualize trajectory
        Instantiate(debugPrefab, transform.position + Vector3.right * 0.25f + Vector3.down * 0.1f, Quaternion.identity);
    }


    void HandleInput()
    {
        movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        bool jumpInput = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W); // replace this line with matt's call
        if (jumpInput && canJump)
        {
            beginJump = true;
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
            coyoteCountdown = 0;
            newVelocity.y = jumpVelocity;
        }

        if (onFloor)
        {
            //Debug.Log("on floor");
            if (Mathf.Abs(movementDirection.x) > 0) // moving
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, groundMovementSmoothing * Time.fixedDeltaTime);
            }
            else
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, 2 * groundMovementSmoothing * Time.fixedDeltaTime);
            }
        }
        else // jumping or falling
        {

            if (movementDirection.x != 0)
            {
                // If you're already moving fast in a direction, don't slow down when you press that direction
                if (Mathf.Sign(movementDirection.x) != Mathf.Sign(newVelocity.x) ||
                    Mathf.Abs(playerSpeed) > Mathf.Abs(newVelocity.x))
                {
                    newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * playerSpeed, airMovementSmoothing * Time.fixedDeltaTime);
                }
            }
        }

        playerRB.velocity = newVelocity;
    }


}
