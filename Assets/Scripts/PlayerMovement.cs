using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;

    // states for animator
    private enum MovementState 
    {
        idle,
        running,
        jumping,
        falling
    }

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;



    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded()) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        UpdateAnimation();
    }


    private void FixedUpdate() 
    {
        // Horizontal Movement Update
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }


    private void UpdateAnimation() 
    {
        MovementState state;

        // if there is horizontal movement, the state is set to running
        if (horizontal > 0f) 
        {
            state = MovementState.running;
            sr.flipX = false;
            coll.offset = new Vector2(0.5f, 0f);
        }
        else if (horizontal < 0f) 
        {
            state = MovementState.running;
            sr.flipX = true;
            coll.offset = new Vector2(-0.5f, 0f);
        }
        else 
        {
            state = MovementState.idle;
        }

        // if there is positive vertical movement, you are jumping or falling
        if (rb.velocity.y > 0.1f) 
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f) 
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }


    private bool IsGrounded() 
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }
}
