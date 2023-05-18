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
    private bool isDead;
    private bool isFacingRight;
    private float jumpTimeCounter;

    [SerializeField] private float jumpTime;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;

    // states for animator
    private enum AnimationState 
    {
        Player_Idle,
        Player_Run,
        Player_Jump,
        Player_Fall,
        Player_Landing,
        Player_Duck,
        Player_Death_1
    }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;


    void Start() 
    {
        currentState = AnimationState.Player_Idle;
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
            CameraController.Instance.DuckCamera(isDucking);
        }

        // determine what animation to play when on the ground
        if (IsGrounded() && !isJumping) 
        {
            if (isLanding) 
            {
                ChangeAnimationState(AnimationState.Player_Landing);
                EffectAnimator.Instance.setPosition(transform.position);
                EffectAnimator.Instance.Land(isFacingRight);
            }
            else if (horizontal > 0f || horizontal < 0f) 
            {
                ChangeAnimationState(AnimationState.Player_Run);
            }
            else if (vertical < 0f) 
            {
                ChangeAnimationState(AnimationState.Player_Duck);
                isDucking = true;
                CameraController.Instance.DuckCamera(isDucking);
            }
            else 
            {
                ChangeAnimationState(AnimationState.Player_Idle);
            }
        }

        // update animation for jumping based on flags, NOT on vertical movement
        // otherwise walking up slopes becomes jumping
        if (isJumpPressed || isJumping) 
        {
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
        if (Input.GetKey(KeyCode.Space) && isJumping) 
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

        if (rb.velocity.y < -0.5f) 
        {
            ChangeAnimationState(AnimationState.Player_Fall);
            isLanding = true;
        }

    }


    private void ChangeAnimationState(AnimationState newState) 
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;

        // play the new animation
        anim.Play(newState.ToString());

        // reassign the current state
        currentState = newState;
    }


    private bool IsGrounded() 
    {
        return Physics2D.BoxCast(coll.bounds.center, new Vector2(0.5f, 1f), 0f, Vector2.down, 0.1f, jumpableGround);
    }


    // called by landing animation at the end of its cycle
    private void LandingComplete() 
    {
        isLanding = false;
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
