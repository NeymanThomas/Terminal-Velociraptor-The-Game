using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private AnimationState currentState;
    private float horizontal;
    private float vertical;
    private bool isJumpPressed;
    private bool isJumping;
    private bool isDucking;
    private bool isLanding;
    private bool isScratchPressed;
    private bool isScratching;
    private bool isNotMoving;
    private bool isDead;
    private bool isFacingRight;
    private float jumpTimeCounter;
    private float originalSpeed;

    [SerializeField] private float jumpTime;
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;
    [SerializeField] private float crouchingSpeed;

    // states for animator
    private enum AnimationState 
    {
        Player_Idle,
        Player_Run,
        Player_Jump,
        Player_Fall,
        Player_Landing,
        Player_Duck,
        Player_Duck_Walk,
        Player_StandingScratch,
        Player_RunningScratch,
        Player_Death_1
    }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D circleColl;
    [SerializeField] private BoxCollider2D boxColl;
    [SerializeField] private LayerMask jumpableGround;


    void Start() 
    {
        currentState = AnimationState.Player_Idle;
        originalSpeed = speed;
    }


    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && IsGrounded()) 
        {
            isJumpPressed = true;
        }
        if (Input.GetButtonUp("Jump")) 
        {
            isJumping = false;
        }

        if (Input.GetButtonDown("Fire1") && IsGrounded() && !isDucking) 
        {
            isScratchPressed = true;
        }

        if (vertical < 0) 
        {
            isDucking = true;
        }
        else 
        {
            isDucking = false;
        }
    }


    private void FixedUpdate() 
    {
        // No movement is allowed if you're dead
        if (isDead) return;

        // Horizontal Movement Update
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // simply flip the sprite depending on horizontal movement
        if (horizontal > 0f) 
        {
            sr.flipX = false;
            isFacingRight = true;
        }
        else if (horizontal < 0f)
        {
            sr.flipX = true;
            isFacingRight = false;
        }

        // reset the camera if the player was previously ducking
        if (!isDucking && CameraController.Instance.IsPlayerDucking) 
        {
            GrowHitbox();
            CameraController.Instance.RaiseCamera();
        }

        // determine what animation to play when on the ground
        if (IsGrounded() && !isJumping) 
        {
            // landing overrides all other grounded animations because it is the first
            // thing you must do after falling, duh
            if (isLanding) 
            {
                ChangeAnimationState(AnimationState.Player_Landing);
                EffectAnimator.Instance.setPosition(transform.position);
                EffectAnimator.Instance.Land(isFacingRight);
            }

            // see if the player is moving or not
            else if (horizontal > 0f || horizontal < 0f) 
            {
                // check to see if the player is ducking. Adjust the collider hitbox as well
                if (isDucking) 
                {
                    isScratching = false;
                    isScratchPressed = false;
                    ShrinkHitbox();
                    ChangeAnimationState(AnimationState.Player_Duck_Walk);
                    CameraController.Instance.DuckCamera();
                }

                // Did the player press the attack button?
                else if (isScratchPressed) 
                {
                    // check to see if the player is already in the process of the scratching animation.
                    // this is important because if the player goes from moving and stopping then the
                    // separate standing and running attack animations override each other.
                    if (!isScratching) 
                    {
                        isScratching = true;
                        ChangeAnimationState(AnimationState.Player_RunningScratch);
                    }
                }
                // otherwise, just run
                else 
                {
                    ChangeAnimationState(AnimationState.Player_Run);
                }
            }

            // otherwise, the player is standing still
            else 
            {
                // is the player ducking while not moving?
                if (isDucking) 
                {
                    isScratching = false;
                    isScratchPressed = false;
                    ShrinkHitbox();
                    ChangeAnimationState(AnimationState.Player_Duck);
                    CameraController.Instance.DuckCamera();
                }

                // is the player attacking while standing still?
                else if (isScratchPressed) 
                {
                    if (!isScratching) 
                    {
                        isScratching = true;
                        ChangeAnimationState(AnimationState.Player_StandingScratch);
                    }
                }
                // otherwise, the player is standing idly
                else 
                {
                    ChangeAnimationState(AnimationState.Player_Idle);
                }
            }
        }

        // update animation for jumping based on flags, NOT on vertical movement
        // otherwise walking up slopes becomes jumping
        if (isJumpPressed || isJumping) 
        {
            isScratchPressed = false;
            isScratching = false;
            ChangeAnimationState(AnimationState.Player_Jump);
        }

        // Jumping movement update
        if (isJumpPressed && IsGrounded()) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            isJumpPressed = false;
            isJumping = true;
            jumpTimeCounter = jumpTime;
        }
        // logic to allow the player to jump higher the longer jump is held
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && isJumping) 
        {
            if (jumpTimeCounter > 0) 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jumpTimeCounter -= Time.deltaTime;
            }
            else 
            {
                isJumping = false;
            }
        }

        // if the player is not on the ground and moving negatively, they're falling
        if (!IsGrounded() && rb.velocity.y < -0.5f && !isScratching) 
        {
            ChangeAnimationState(AnimationState.Player_Fall);
            isLanding = true;
        }
    }


    // All updates to the player's current animation are made here
    private void ChangeAnimationState(AnimationState newState) 
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;

        // play the new animation
        anim.Play(newState.ToString());

        // reassign the current state
        currentState = newState;
    }


    // simple check to see if the player is touching the ground layer or not
    private bool IsGrounded() 
    {
        return Physics2D.BoxCast(circleColl.bounds.center, new Vector2(0.4f, 0.4f), 0f, Vector2.down, 0.4f, jumpableGround);
    }


    // when crouching, the speed and hitbox sizes need to be adjusted
    private void ShrinkHitbox() 
    {
        speed = crouchingSpeed;
        boxColl.size = new Vector2(0.1f, 0.1f);
        boxColl.offset = new Vector2(0f, -0.1f);
        circleColl.radius = 0.3f;
        circleColl.offset = new Vector2(0f, -0.2f);
    }


    // after coming out of ducking, hitbox and speed need to be adjusted
    private void GrowHitbox() 
    {
        speed = originalSpeed;
        boxColl.size = new Vector2(1f, 0.5f);
        boxColl.offset = new Vector2(0f, 0.1f);
        circleColl.radius = 0.4f;
        circleColl.offset = new Vector2(0f, -0.1f);
    }


    // called by landing animation at the end of its cycle
    private void LandingComplete() 
    {
        isLanding = false;
    }


    // called by scratching animations at the end of their cycles
    private void ScratchComplete() 
    {
        isScratchPressed = false;
        isScratching = false;
        ChangeAnimationState(AnimationState.Player_Idle);
    }


    // For detecting when running into objects with collision detection
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Hazard")) 
        {
            isDead = true;
            ChangeAnimationState(AnimationState.Player_Death_1);
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void RestartLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isDead = false;
    }
}
