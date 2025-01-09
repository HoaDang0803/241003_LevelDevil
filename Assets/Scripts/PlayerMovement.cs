using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask JumpableGround;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private float dirX = 0f;
    private bool moveLeft;
    private bool moveRight;
    private Animator anim;
    private bool isFlipped = false;
    private enum MovementState { Idle, Running, Jumping, Falling }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        moveLeft = false;
        moveRight = false;
    }

    private void Update()
    {
        MovementState state;
        if (moveRight)
        {
            dirX = GameController.instance.speed;
            state = MovementState.Running;
            sprite.flipX = false;
        }
        else if (moveLeft)
        {
            dirX = -GameController.instance.speed;
            state = MovementState.Running;
            sprite.flipX = true;
        }
        else
        {
            dirX = 0;
            state = MovementState.Idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }

        anim.SetInteger("state", (int)state);

        //if (GameController.instance.currentLevel == 8)
        //{
        //    dirX = -Input.GetAxisRaw("Horizontal");
        //}
        //else
        //{
        //    dirX = Input.GetAxisRaw("Horizontal");
        //}

        //rb.velocity = new Vector2(dirX * GameController.instance.speed, rb.velocity.y);

        //if (Input.GetButtonDown("Jump") && IsGrounded())
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, GameController.instance.jumpForce);
        //}
    }

    public void Move()
    {
        if (!GameController.instance.canMove)
            return;
        rb.velocity = new Vector2(dirX, rb.velocity.y);
    }

    public void PointerDownLeft()
    {
        if (GameController.instance.currentLevel == 8)
        {
            moveRight = true;
        }
        else
        {
            moveLeft = true;
        }
    }

    public void PoiterUpLeft()
    {
        if (GameController.instance.currentLevel == 8)
        {
            moveRight = false;
        }
        else
        {
            moveLeft = false;
        }
    }

    public void PointerDownRight()
    {
        if (GameController.instance.currentLevel == 8)
        {
            moveLeft = true;
        }
        else
        {
            moveRight = true;
        }
    }

    public void PointerUpRight()
    {
        if (GameController.instance.currentLevel == 8)
        {
            moveLeft = false;
        }
        else
        {
            moveRight = false;
        }
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, GameController.instance.jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, JumpableGround);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}
