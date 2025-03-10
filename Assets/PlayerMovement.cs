using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    float horizontalInput;
    float moveSpeed = 5f;
    public float moslem;
    float jumpPower = 5f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    //i love this function
    void Update()
    {
        if (!IsOwner) return;
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip character for animations
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
                isGrounded = false;

                animator.SetBool("isJumping", !isGrounded);
            }
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            isGrounded = false;

            animator.SetBool("isJumping", !isGrounded);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            isGrounded = false;
            
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
}
