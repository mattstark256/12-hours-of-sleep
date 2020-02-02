using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // editor feilds
    [SerializeField]
    private float playerSpeed = 10;
    [SerializeField]
    private float crouchSpeed = 5;
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
    private float minShakeVelocity = 30;
    [SerializeField]
    private float maxShakeVelocity = 60;
    [SerializeField]
    private CapsuleCollider2D tallCollider;
    [SerializeField]
    private CapsuleCollider2D crouchedCollider;

    // other untiy bullshit
    private GameObject player;
    private Rigidbody2D playerRB;
    public Animator animator;
    public SpriteRenderer playerSprite;

    // member variables
    private Vector2 movementDirection;
    float fallingVelocity;
    float tallHeight ;
    float colliderWidth ;
    float crouchedHeight ;

    float tallOffset ;
    float crouchedOffset ;
    float colliderXOffset;

    bool crouched = false;

    // flags for fixed update
    bool canJump = false;
    bool onFloor = false;
    bool beginJump = false;
    bool crouchInput = false;


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
        // tallCollider = GetComponent<CapsuleCollider2D>();
        // crouchedCollider = GetComponentInChildren<CapsuleCollider2D>();
        //tallHeight = GetComponent<Collider>().size.y;
        //colliderWidth = GetComponent<Collider>().size.x;
        //crouchedHeight = tallHeight / 2f;
        //tallOffset = GetComponent<Collider>().offset.y;
        //crouchedOffset = tallOffset / 2f;
        //colliderXOffset = GetComponent<Collider>().offset.x;
        //crouchedCollider.enabled = false;
        //tallCollider.enabled = true;
        tallCollider.gameObject.SetActive(true);
        crouchedCollider.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void handleIfCanJump()
    {
        bool onFloorLastFrame = onFloor;

        onFloor = Physics2D.Raycast(transform.position + Vector3.right * 0.2f, Vector3.down, 0.1f, environment)
               || Physics2D.Raycast(transform.position + Vector3.left * 0.2f, Vector3.down, 0.1f, environment);

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
            // ~sf normal landing
            float trauma = Mathf.Clamp01(Mathf.InverseLerp(minShakeVelocity, maxShakeVelocity, fallingVelocity));
            Debug.Log("impact velocity: " + fallingVelocity + " max & min: " + maxShakeVelocity + ", " + minShakeVelocity + " trauma value: " + trauma);
            CameraEffects.Instance.AddScreenShake(trauma);

            if (trauma > 0.1f) // slightly above velocity for same height jump
            {
                AudioManager.Instance.Play("heavy_landing");
            }
            else
            {
                AudioManager.Instance.Play("normal_landing");
            }

        }

        // Create object to vizualize trajectory
        //Instantiate(debugPrefab, transform.position + Vector3.right * 0.25f + Vector3.down * 0.1f, Quaternion.identity);
    }


    void HandleInput()
    {
        movementDirection = Vector2.zero;
        if (InputMapper.Instance.GetButton(Action.MoveRight)) movementDirection.x += 1;
        if (InputMapper.Instance.GetButton(Action.MoveLeft)) movementDirection.x -= 1;
        //movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0); // replace line with matts code
        bool jumpInput = InputMapper.Instance.GetButtonDown(Action.Jump);
        //bool jumpInput = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W); // replace this line with matt's call
        //crouchInput = Input.GetKey(KeyCode.S);
        crouchInput = InputMapper.Instance.GetButton(Action.Crouch); // replace this line with matt's call

        if (crouched)
        {

            if (Mathf.Abs(movementDirection.x) > 0.1 && onFloor)
            {
                AudioManager.Instance.SetLoopingAndPlay("crouch_walk");
            }
            else if (AudioManager.Instance.IsPlaying("crouch_walk"))
            {
                AudioManager.Instance.StopLooping("crouch_walk");
            }
        }
        else {

            if (Mathf.Abs(movementDirection.x) > 0.1 && onFloor)
            {
                AudioManager.Instance.SetLoopingAndPlay("walking");
            }
            else if (AudioManager.Instance.IsPlaying("walking"))
            {
                AudioManager.Instance.StopLooping("walking");
            }
        }




        if (jumpInput && canJump)
        {
            AudioManager.Instance.Play("jump");
            beginJump = true;
        }
        
    }

    private void FixedUpdate()
    {

        // crouching

        if (crouchInput )
        {
            if (tallCollider.gameObject.activeSelf == true)
            {
                AudioManager.Instance.Play("crouch");
                crouched = true;
                animator.SetBool("Crouching", true);
                tallCollider.gameObject.SetActive(false);
                crouchedCollider.gameObject.SetActive(true);
            }

        }
        else if(crouchedCollider.gameObject.activeSelf == true && !Physics2D.Raycast(transform.position + Vector3.up,Vector3.up,0.25f, environment))
        {
            AudioManager.Instance.Play("uncrouch");
            crouched = false;
            animator.SetBool("Crouching", false);
            tallCollider.gameObject.SetActive(true);
            crouchedCollider.gameObject.SetActive(false);
        }




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

            animator.SetFloat("Velocity", Mathf.Abs(movementDirection.x));

        if (movementDirection.x > 0.1f) playerSprite.flipX = false;
        if (movementDirection.x < -0.1f) playerSprite.flipX = true;

        if (onFloor)
        {
            animator.SetBool("Jumping", false);
            //Instantiate(debugPrefab, transform.position + Vector3.right * 0.25f + Vector3.down * 0.1f, Quaternion.identity);
            float speed = crouched ? crouchSpeed : playerSpeed;
            if (Mathf.Abs(movementDirection.x) > 0) // moving
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * speed, groundMovementSmoothing * Time.fixedDeltaTime);
            }
            else
            {
                newVelocity.x = Mathf.Lerp(playerRB.velocity.x, movementDirection.x * speed, 2 * groundMovementSmoothing * Time.fixedDeltaTime);
            }
        }
        else // jumping or falling
        {
            animator.SetBool("Jumping", true);

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
