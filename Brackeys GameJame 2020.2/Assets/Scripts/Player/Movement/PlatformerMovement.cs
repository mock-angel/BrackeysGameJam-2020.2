using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformerMovement : MonoBehaviour
{
    #region VARIABLES

    public static PlatformerMovement Instance { get; private set; }

    [Header("ParticleSystem")]
    public ParticleSystem CheckpointParticles;
    public ParticleSystem LoseLifeParticles;
    public ParticleSystem GainLifeParticles;
    public ParticleSystem LoseHealthParticles;
    public ParticleSystem GainHealthParticles;

    public ParticleSystem MovingParticles;
    public ParticleSystem JumpParticles;

    //Scripts & Components
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer spriterenderer;
    private AnimatorOverrider animatoroverrider;
    private Health healthscript;
    private PlayerAimingAndFire playeramingandfirescript;

    //Character Stages
    [Header("Character Stages")]
    public int characterStage;

    //Movement Variables
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 18f;

    public float currentMoveSpeed_Stage;
    public float currentJumpForce_Stage;
    public float moveInput;

    //Jump
    [Header("Jump")]
    public int jumpAmount = 2;
    public int currentJumpsAvailable;

    public int currentJumpAmount_Stage;

    public bool isGrounded;
    public float checkRadius = 0.1f;
    public Transform feetPosition;
    public LayerMask groundLayerMask;

    public bool isJumping;
    public float jumpTimeCounter;
    public float jumpTime = 0.2f;

    public float currentJumpTime_Stage;

    //Optimised Jump
    [Header("Optimized Jump")]
    public float fallMultiplier = 8f;
    public float lowJumpMultiplier = 7f;

    public float hangTime = 0.2f;
    public float currentHangTime;
    public bool isFalling;

    //Rewind
    public bool isRewinding;

    #endregion

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region PARTICLE SYSTEM

        CheckpointParticles.Stop();
        LoseLifeParticles.Stop();
        GainLifeParticles.Stop();

        LoseHealthParticles.Stop();
        GainHealthParticles.Stop();
        MovingParticles.Stop();
        JumpParticles.Stop();

        #endregion

        #region ASSIGN COMPONENTS

        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriterenderer = gameObject.GetComponent<SpriteRenderer>();
        animatoroverrider = gameObject.GetComponent<AnimatorOverrider>();
        healthscript = gameObject.GetComponent<Health>();
        playeramingandfirescript = gameObject.GetComponent<PlayerAimingAndFire>();

        #endregion

        #region ASSIGN VARIABLES

        Instance = this;

        currentJumpsAvailable = jumpAmount;

        currentMoveSpeed_Stage = moveSpeed;
        currentJumpForce_Stage = jumpForce;
        currentJumpAmount_Stage = jumpAmount;
        currentJumpTime_Stage = jumpTime;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0 && !isRewinding)
        {
            //Player Input
            PlayerInput();

            #region FLIP PLAYER

            //if (transform.localScale.x > 0 && moveInput < 0)
            //{
            //    //Flip Player
            if(moveInput != 0) FlipPlayerSprite();
            //}
            //else if (transform.localScale.x < 0 && moveInput > 0)
            //{
                //Flip Player
            //    FlipPlayerSprite();
            //}

            #endregion
        }


        //Animation
        animator.SetFloat("movementSpeed", Mathf.Abs(moveInput));
    }

    void FixedUpdate()
    {
        //Player Movement
        PlayerMovement();
    }

    public void ChangeCharacterStage()
    {
        if (isGrounded)
        {
            #region CHARACTER STAGES

            //Death
            if (characterStage == 0)
            {
                if (healthscript.isInvincibleCounter > 0.05f)
                {
                    healthscript.isInvincibleCounter = 0.04f;
                }
                healthscript.ChangeHealth(-1);
            }
            //Caveman
            else if (characterStage == 1)
            {
                //Assign new Animation
                animatoroverrider.SetAnimationToValueInList(0);

                //Assign Special skills
                currentJumpForce_Stage = jumpForce * 1.2f;
                currentMoveSpeed_Stage = moveSpeed * 1.2f;
                currentJumpTime_Stage = jumpTime * 1f;
                currentJumpAmount_Stage = jumpAmount + 0;

                //Shooting
                playeramingandfirescript.canShoot = false;
            }
            //Teenager
            else if (characterStage == 2)
            {
                //Assign new Animation
                animatoroverrider.SetAnimationToValueInList(1);

                //Assign Special skills
                currentJumpForce_Stage = jumpForce * 1f;
                currentMoveSpeed_Stage = moveSpeed * 1f;
                currentJumpTime_Stage = jumpTime * 1f;
                currentJumpAmount_Stage = jumpAmount + 0;

                //Shooting
                playeramingandfirescript.canShoot = true;
            }
            //Cyborg
            else if (characterStage == 3)
            {
                //Assign new Animation
                animatoroverrider.SetAnimationToValueInList(2);

                //Assign Special skills
                currentJumpForce_Stage = jumpForce * 0.9f;
                currentMoveSpeed_Stage = moveSpeed * 0.8f;
                currentJumpTime_Stage = jumpTime * 0.9f;
                currentJumpAmount_Stage = jumpAmount + 1;

                //Shooting
                playeramingandfirescript.canShoot = true;
            }

            #endregion#
        }
    }

    public void PlayerInput()
    {
        //Player Input

        #region MOVING

        //Player Move Input
        moveInput = Input.GetAxis("Horizontal");

        if(moveInput != 0) {
            if(!MovingParticles.isPlaying) MovingParticles.Play();
        }
        else if(MovingParticles.isPlaying) MovingParticles.Stop();

        #endregion

        #region JUMPING

        //Jump Once
        if (Input.GetButtonDown("Jump") && currentHangTime > 0 && currentJumpsAvailable == currentJumpAmount_Stage)
        {
            isJumping = true;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;

            //Decrease Jump Amount
            currentJumpsAvailable--;

            JumpParticles.Play();
        }

        //Jump Longer
        else if (Input.GetButton("Jump") && isJumping && !isGrounded)
        {
            if (jumpTimeCounter > 0 && currentJumpsAvailable == currentJumpAmount_Stage - 1)
            {
                //Decrease jumptime
                jumpTimeCounter -= Time.deltaTime;

                //Actually Jump
                rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;
            }
            else
            {
                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        //Jump Multiple Times
        else if (Input.GetButtonDown("Jump") && currentJumpsAvailable > 0 && currentJumpsAvailable < currentJumpAmount_Stage && !isGrounded && !isFalling)
        {
            Debug.Log("test");
            isJumping = true;

            //Reset JumpTimeCounter
            jumpTimeCounter = currentJumpTime_Stage;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;

            //Decrease Jump Amount
            currentJumpsAvailable--;
        }

        //Jump When Falling
        else if (Input.GetButtonDown("Jump") && currentHangTime < 0 && !isJumping && currentJumpsAvailable > 1)
        {
            //Is Falling
            isFalling = true;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;

            //Decrease Jump Amount
            currentJumpsAvailable--;
        }

        //Animation
        if (Input.GetButton("Jump") && !isGrounded && isJumping)
        {
            //Jump Animation
            animator.SetBool("isJumping", true);
        }
        else
        {
            //Jump Animation
            animator.SetBool("isJumping", false);
        }

        //Stop Jumping
        if (Input.GetButtonUp("Jump") && currentJumpsAvailable < currentJumpAmount_Stage)
        {
            isJumping = false;
        }

        #endregion

        #region OPTIMISED JUMPING

        //When Player is Falling
        if (rigidbody2d.velocity.y < 0)
        {
            rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        //When Player is Rising
        else if (rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        #endregion
    }

    public void PlayerMovement()
    {
        #region PLAYER MOVEMENT

        //Player Movement
        rigidbody2d.velocity = new Vector2(moveInput * currentMoveSpeed_Stage, rigidbody2d.velocity.y);


        #endregion

        #region PLAYER JUMP

        //Check if is Grounded
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkRadius, groundLayerMask);

        //Fall, Land Animation
        animator.SetBool("isGrounded", isGrounded);

        //When Player is Grounded
        if (isGrounded && !isJumping)
        {
            //Reset JumpTimeCounter
            jumpTimeCounter = currentJumpTime_Stage;

            //Reset Jump Amount
            currentJumpsAvailable = currentJumpAmount_Stage;

            //Land Animation
            animator.SetBool("isJumping", false);
        }

        //Hang Time
        if (isGrounded)
        {
            currentHangTime = hangTime;
            isFalling = false;
        }
        else
        {
            currentHangTime -= Time.deltaTime;
        }

        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(feetPosition.position, checkRadius);
    }

    public bool isFacingRight = true;

    public void FlipPlayerSprite()
    {
        #region FLIP PLAYER & IDLE, WALK ANIMATION

        if (isGrounded)
        {
            //Turn Animation
            //animator.SetTrigger("turn");
        }

        //if (moveInput == 0) MovingParticles.Stop();
        if (moveInput > 0)
        {
            //Look to the right
            //transform.localScale = new Vector3(1, 1, 1);
            if(!isFacingRight){
                transform.Rotate(0, 180, 0);

                if(isGrounded) animator.SetTrigger("turn");
                isFacingRight = true;
            }
           //MovingParticles.Stop();
            //MovingParticles.Play();
        }
        if (moveInput < 0)
        {
            //Look to the left
            //transform.localScale = new Vector3(-1, 1, 1);

            if(isFacingRight){
                transform.Rotate(0, 180, 0);

                if(isGrounded) animator.SetTrigger("turn");
                isFacingRight = false;
            }
            //MovingParticles.Stop();
            //MovingParticles.Play();
        }

        #endregion
    }

    public void OnDamageTaken(int damage)
    {
        GetComponent<Health>().ChangeHealth(-damage);
    }

}
