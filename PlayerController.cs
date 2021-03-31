using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    
    float xInput;
    float zInput;
    Vector3 moveZDir;
    Vector3 moveXDir;
    Vector3 finalDirection;

    Rigidbody rb;

    [Header("Ground Checking")]
    public Transform groundChecker;
    public LayerMask isGround;
    public float groundCheckerRadius = 0.2f;

    bool facingLeft;

    [Header("Jump Handling")]
    public float jumpForce;
    public float coyoteTime;

    [Space]
    public float fallGravityMultiplier;
    public float shortJumpGravityMultiplier;

    [Space]
    [SerializeField]
    bool isGrounded;
    [SerializeField]
    bool coyoteJump = false;

    [Space]
    [SerializeField]
    bool isJumping = false;
    [SerializeField]
    bool jumpInputReset = true;

    [Header("Visual")]
    public Animator anim;
    public ParticleSystem jumpParticle;
    public ParticleSystem airTrailParticle;
    public ParticleSystem landParticle;

    //public ScoreTracker scoreTracker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && jumpInputReset)
        {
            isJumping = true;
            jumpInputReset = false;
        }

        moveXDir = xInput * Camera.main.transform.right;
        moveZDir = zInput * Camera.main.transform.forward;

        Vector3 combinedInput = moveXDir + moveZDir;

        float inputMagnitude = Mathf.Abs(xInput) + Mathf.Abs(zInput);

        finalDirection = new Vector3(combinedInput.normalized.x, 0, combinedInput.normalized.z);

        Quaternion playerRot = Quaternion.LookRotation(finalDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, playerRot, Time.fixedDeltaTime * inputMagnitude * 1.5f);
        transform.rotation = targetRotation;

        FallGravity();

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(finalDirection.x * speed, rb.velocity.y, finalDirection.z * speed);   // horizontal input

        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));           // update animator for Run animation

        bool groundTouch = isGrounded;
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckerRadius, isGround);    // check if the player is on the ground
        //anim.SetBool("isGrounded", isGrounded);                     // reset to Idle animation

        float animVelocityClamp = Mathf.Clamp(rb.velocity.y, -0.5f, 0.5f);
        //anim.SetFloat("verticalVelocity", animVelocityClamp);
        
        //Play particle on the frame of landing
        if (!groundTouch && isGrounded)
        {
            landParticle.Play();
        }

        // the player is grounded and can now jump
        if (isGrounded && !isJumping)
        {
            jumpInputReset = true;
            airTrailParticle.Stop();
        }

        // allows player to still jump after briefly walking off the ground
        if (!isGrounded && jumpInputReset)
        {
            coyoteJump = true;
            StartCoroutine(CoyoteTimer());
        }
        
        // can jump if on the ground or in coyote time
        if ((isGrounded||coyoteJump) && isJumping)
        {
            Jump();
        }

        // handles sprite orientation
        if (xInput > 0 && !facingLeft)
        {
            //Flip();
        }

        else if (xInput < 0 && facingLeft)
        {
            //Flip();
        }


    }

    void Jump()
    {
        jumpParticle.Play();
        airTrailParticle.Play();
        rb.velocity += Vector3.up * jumpForce;
        isJumping = false;
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        Vector2 orientationScale = transform.localScale;
        orientationScale.x *= -1;

        transform.localScale = orientationScale;
    }

    // Gravity modifier when falling/jumping
    void FallGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (shortJumpGravityMultiplier - 1) * Time.deltaTime;
        }
    }

    // Allow Coyote jump until coyote time runs out
    IEnumerator CoyoteTimer()
    {
        yield return new WaitForSeconds(coyoteTime);
        coyoteJump = false;
    }

    // Collect Coin
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Coin"))
        {
            //scoreTracker.updateScore(scoreTracker.getCoinScore());
            //string currentScore = scoreTracker.getScore().ToString();
            //scoreTracker.scoreValue.SetText(currentScore);
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckerRadius);         // Draw Ground Checker
    }
}
