using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Pool;
using System.Collections;

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

    [Header("Shooting Variables")]
    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private float fireRateDelay = 0.5f;
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private float projectileLifetime = 10f;
    private float fireRateTimer = 0f;

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
                // Search for the object with the tag "Pool" and assign it to projectilePool
        GameObject poolObject = GameObject.FindGameObjectWithTag("Pool");
        if (poolObject != null)
        {
            projectilePool = poolObject.GetComponent<ProjectilePool>();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Pool' found. Please assign the projectile pool.");
        }
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
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            
            animator.SetBool("isJumping", !isGrounded);
        }

        // Shooting logic with fire rate timer
        fireRateTimer += Time.deltaTime;
        if (Input.GetAxis("Fire1") > 0 && fireRateTimer > fireRateDelay)
        {
            FireProjectile();
            fireRateTimer = 0f;
        }

        if (Input.GetAxis("Fire1")>0){
            animator.SetBool("Shoot",true);
        } else {
            animator.SetBool("Shoot", false);
        }
    }

    private void FireProjectile()
    {
        // Get projectile from pool instead of instantiating
        GameObject projectile = projectilePool.GetProjectile();

        // Set projectile position in front of the player
        Vector3 projectileSpawnPosition = transform.position + new Vector3(transform.localScale.x, 0, 0);
        projectile.transform.position = projectileSpawnPosition;

        // Projectile direction based on player facing direction
        Vector2 direction = new Vector2(transform.localScale.x, 0);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Reset velocity
        projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);

        // Set up timer to return projectile to pool
        StartCoroutine(ReturnProjectileAfterDelay(projectile, projectileLifetime));
    }

    private System.Collections.IEnumerator ReturnProjectileAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (projectile.activeInHierarchy) // Check if it hasn't already been returned to pool
        {
            projectilePool.ReturnProjectile(projectile);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }
}
