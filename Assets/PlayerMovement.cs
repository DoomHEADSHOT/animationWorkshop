using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Player ID")]
    [SerializeField] private string playerName;
    [SerializeField]
    [TextArea]
    private string Description = "this the description of our NPC";

    [Header("Movement Variables")]
    [Tooltip("this is the variable that tells our code the direction of the movement")]
    [SerializeField] float horizontalInput;
    [SerializeField] [Delayed] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [HideInInspector] public bool isGrounded = false;

    [Space(20)]

    [Header("Health Variables")]
    [SerializeField] int maxHealth = 100;
    [Range(0, 100)]
    [SerializeField] int currentHealth;
    [Tooltip("the default healing points in our game")]
    [SerializeField] [Min(1)] int healPoints;

    [ColorUsage(true,false)] public Color color = Color.white;

    Rigidbody2D rb;
    Animator animator;

    [ContextMenu("resetVariablesToDefault")]
    public void resetDefaultVariables()
    {
        moveSpeed = 5f;
        jumpPower = 5f;
        maxHealth = 100;
        currentHealth =  80;
        healPoints = 10;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    //i love this function
    void Update()
    {
      
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip character for animations
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            
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
