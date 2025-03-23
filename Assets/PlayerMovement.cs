using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player ID")]
    [SerializeField] private string playerName;

    [Header("Movement Variables")]
    [Tooltip("this is the variable that tells our code the direction of the movement")]
    [SerializeField] float horizontalInput;
    [SerializeField] [Delayed] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 5f;
    [HideInInspector] public bool isGrounded = false;

    [Header("Double Jump")]
    [SerializeField] private int maxJumps = 1;
    private int remainingJumps;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;

    [Space(20)]

    [SerializeField] private ProjectilePool projectilePool;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRateDelay = 0.5f;
    [SerializeField] private float projectileLifeTime = 10f;
    private float fireRateTimer = 0f;


    Rigidbody2D rb;
    Animator animator;

    [ContextMenu("resetVariablesToDefault")]
    public void resetDefaultVariables()
    {
        moveSpeed = 5f;
        jumpPower = 5f;

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        var Camera = FindFirstObjectByType <Unity.Cinemachine.CinemachineCamera>();
        DontDestroyOnLoad(Camera);
        if (Camera != null) {
            Camera.Follow = transform;
        }else
        {
            print("I cannot find the camera");
        }

        GameObject poolObject = GameObject.FindGameObjectWithTag("Pool");
        if (poolObject != null) { 
            projectilePool = poolObject.GetComponent<ProjectilePool>();
        }else
        {
            Debug.Log("we didn't found the pool object");
        }
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
        
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || remainingJumps > 0 ))
        {
            if (!isGrounded)
            {
                remainingJumps--;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            isGrounded = false;
            
            animator.SetBool("isJumping", !isGrounded);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


        fireRateTimer += Time.deltaTime;
        if(Input.GetAxis("Fire1") > 0  && fireRateTimer > fireRateDelay)
        {
            GameObject projectile = projectilePool.GetProjectile();

            Vector3 projectibleSpawnPoint = transform.position + new Vector3(transform.localScale.x,0,0);
            projectile.transform.position = projectibleSpawnPoint;

            Vector2 direction = new Vector2 (transform.localScale.x,0);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            projectile.GetComponent<Rigidbody2D>().AddForce(direction* 30f,ForceMode2D.Impulse);

            StartCoroutine(ReturnProjectileAfterDelay(projectile,projectileLifeTime));
        }




        if (Input.GetAxis("Fire1") > 0) {
            animator.SetBool("Shoot", true);
        } else
        {
            animator.SetBool("Shoot", false);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false; // set this to false to prevent dashing spam
        isDashing = true; // we set the dashing to true ,so we can disable the movement later

        // variable used for direction ,we used Local scale X to know the direction of the player
        Vector2 dashDirection = new Vector2(transform.localScale.x, 0);
        rb.AddForce(dashDirection * dashSpeed ,ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator ReturnProjectileAfterDelay(GameObject projectile, float time)
    {
        yield return new WaitForSeconds(time);
        if (projectile.activeInHierarchy)
        {
            projectilePool.ReturnProjectile(projectile);
        }
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if (!isDashing) // we disable this line when he is dashing to not interreupt the dash
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocityY);// we use this line for movement
        }
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
        remainingJumps = maxJumps;
    }
}
