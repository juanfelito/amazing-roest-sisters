using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nikki : MonoBehaviour {
    public float speed = 5;
    public float jumpSpeed = 5;
    public bool facingRight = true;

    bool isJumping = false;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float rayCastLength = 0.005f;
    private float width;
    private float height;
    private float jumpButtonPressTime;
    private float maxJumpTime = 0.2f;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;
    }

    void FixedUpdate() {
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        Vector2 currentVelocity = rigidBody.velocity;
        rigidBody.velocity = new Vector2(horizontalMove * speed, currentVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if ((horizontalMove > 0 && !facingRight) || (horizontalMove < 0 && facingRight)) {
            FlipNikki();
        }

        float verticalMove = Input.GetAxis("Jump");

        if (IsOnGround() && !isJumping && verticalMove > 0) {
            isJumping = true;
        }

        if (jumpButtonPressTime > maxJumpTime) {
            verticalMove = 0f;
        }

        if (isJumping && (jumpButtonPressTime < maxJumpTime)) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }

        if (verticalMove >= 1f) {
            jumpButtonPressTime += Time.deltaTime;
        } else {
            isJumping = false;
            jumpButtonPressTime = 0f;
        }

        animator.SetBool("Jumping", isJumping);
    }

    void FlipNikki() {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public bool IsOnGround() {
        bool groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height), Vector2.down, rayCastLength);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height), Vector2.down, rayCastLength);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height), Vector2.down, rayCastLength);

        return groundCheck1 || groundCheck2 || groundCheck3;
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
