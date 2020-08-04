﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformerMovement : MonoBehaviour
{
    #region VARIABLES

    public static PlatformerMovement Instance{get; private set;}

    //Scripts & Components
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer spriterenderer;
    private AnimatorOverrider animatoroverrider;

    //Character Stages
    [Header("Character Stages")]
    public int characterStage;

    //Movement Variables
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 16f;

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

    #endregion

    void Awake(){
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region ASSIGN COMPONENTS

        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriterenderer = gameObject.GetComponent<SpriteRenderer>();
        animatoroverrider = gameObject.GetComponent<AnimatorOverrider>();

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
        //Player Input
        PlayerInput();

        #region FLIP PLAYER

        if (transform.localScale.x > 0 && moveInput < 0)
        {
            //Flip Player
            FlipPlayerSprite();
        }
        else if (transform.localScale.x < 0 && moveInput > 0)
        {
            //Flip Player
            FlipPlayerSprite();
        }

        #endregion

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
        #region CHARACTER STAGES

        //Death
        if (characterStage == 0)
        {

        }
        //Caveman
        else if (characterStage == 1)
        {
            //Assign new Animation
            animatoroverrider.SetAnimationToValueInList(0);

            //Assign Special skills
            currentJumpForce_Stage = jumpForce * 1.2f;
            currentMoveSpeed_Stage = moveSpeed * 1.2f;
            currentJumpTime_Stage = jumpTime * 0.9f;
            currentJumpAmount_Stage = jumpAmount + 0;
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
        }
        //Cyborg
        else if (characterStage == 3)
        {
            //Assign new Animation
            animatoroverrider.SetAnimationToValueInList(2);

            //Assign Special skills
            currentJumpForce_Stage = jumpForce * 0.8f;
            currentMoveSpeed_Stage = moveSpeed * 0.8f;
            currentJumpTime_Stage = jumpTime * 0.9f;
            currentJumpAmount_Stage = jumpAmount + 1;
        }

        #endregion
    }

    public void PlayerInput()
    {
        //Player Input

        #region MOVING

        //Player Move Input
        moveInput = Input.GetAxis("Horizontal");

        #endregion

        #region JUMPING

        //Jump When Falling
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isJumping && currentJumpsAvailable == currentJumpAmount_Stage)
        {
            isJumping = true;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;

            //Decrease Jump Amount
            currentJumpsAvailable = 0;
        }

        //Jump Once
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;

            //Decrease Jump Amount
            currentJumpsAvailable--;
        }

        //Jump Longer
        else if (Input.GetKey(KeyCode.Space) && isJumping && !isGrounded)
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
        if(Input.GetKeyDown(KeyCode.Space) && currentJumpsAvailable > 0 && currentJumpsAvailable < currentJumpAmount_Stage && !isGrounded)
        {
            isJumping = true;

            //Reset JumpTimeCounter
            jumpTimeCounter = currentJumpTime_Stage;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * currentJumpForce_Stage;

            //Decrease Jump Amount
            currentJumpsAvailable--;
        }

        //Animation
        if (Input.GetKey(KeyCode.Space) && !isGrounded && isJumping)
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
        if (Input.GetKeyUp(KeyCode.Space) && currentJumpsAvailable < currentJumpAmount_Stage)
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
        else if (rigidbody2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
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

        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(feetPosition.position, checkRadius);
    }

    public void FlipPlayerSprite()
    {
        #region FLIP PLAYER & IDLE, WALK ANIMATION

        if (isGrounded)
        {
            //Turn Animation
            animator.SetTrigger("turn");
        }

        if (moveInput == 0)
        {

        }
        else if(moveInput > 0)
        {
            //Look to the right
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(moveInput < 0)
        {
            //Look to the left
            transform.localScale = new Vector3(-1, 1, 1);
        }

        #endregion
    }

    public void OnDamageTaken(int damage){
        GetComponent<Health>().ChangeHealth(-damage);
    }

}
