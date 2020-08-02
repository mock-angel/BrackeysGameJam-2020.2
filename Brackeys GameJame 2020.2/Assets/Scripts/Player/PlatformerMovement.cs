using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformerMovement : MonoBehaviour
{
    #region VARIABLES

    public static PlatformerMovement Instance;

    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private SpriteRenderer spriterenderer;

    //Movement Variables
    public float moveSpeed = 6f;
    public float jumpForce = 16f;
    public float moveInput;

    //Jump
    public int maxExtraJumpAmount = 1;
    public int currentExtraJumpAmount;

    public bool isGrounded;
    public float checkRadius = 0.1f;
    public Transform feetPosition;
    public LayerMask groundLayerMask;

    public bool isJumping;
    public float jumpTimeCounter;
    public float maxJumpTime = 0.2f;

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

        #endregion

        //rigidbody2d.gravityScale = 8f;
        #region ASSIGN VARIABLES

        Instance = this;
        currentExtraJumpAmount = maxExtraJumpAmount;

        #endregion

        //Set Gravity
        rigidbody2d.gravityScale = 12f;
    }

    // Update is called once per frame
    void Update()
    {
        //Player Input
        PlayerInput();


        //Animation
        FlipPlayerSprite();
    }

    void FixedUpdate()
    {
        //Player Movement
        PlayerMovement();
    }

    public void PlayerInput()
    {
        //Player Input

        #region MOVING

        //Player Move Input
        moveInput = Input.GetAxis("Horizontal");

        #endregion

        #region JUMPING

        //Jump Once
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;

            //Reset JumpTimeCounter
            jumpTimeCounter = maxJumpTime;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * jumpForce;
        }

        //Jump Longer
        else if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                //Actually Jump
                rigidbody2d.velocity = Vector2.up * jumpForce;

                //Decrease jumptime
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        //Jump Multiple Times
        if(Input.GetKeyDown(KeyCode.Space) && currentExtraJumpAmount > 0)
        {
            isJumping = true;

            //Reset JumpTimeCounter
            jumpTimeCounter = maxJumpTime;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * jumpForce;

            //Decrease Jump Amount
            currentExtraJumpAmount--;
        }

        if(Input.GetKey(KeyCode.Space) && !isGrounded && jumpTimeCounter > 0)
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
        if (Input.GetKeyUp(KeyCode.Space) && currentExtraJumpAmount <= 0)
        {
            isJumping = false;
        }

        #endregion
    }

    public void PlayerMovement()
    {
        #region PLAYER MOVEMENT

        //Player Movement
        rigidbody2d.velocity = new Vector2(moveInput * moveSpeed, rigidbody2d.velocity.y);

        #endregion

        #region PLAYER JUMP

        //Check if is Grounded
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkRadius, groundLayerMask);

        //Fall, Land Animation
        animator.SetBool("isGrounded", isGrounded);

        //When Player is Grounded
        if (isGrounded)
        {
            currentExtraJumpAmount = maxExtraJumpAmount;

            //Land Animation
            animator.SetBool("isJumping", false);
        }

        #endregion
    }

    public void FlipPlayerSprite()
    {
        if (moveInput == 0)
        {
            //Idle Animation
            animator.SetFloat("movementSpeed", Mathf.Abs(moveInput));
        }
        else if(moveInput > 0)
        {
            //Walk Animation
            animator.SetFloat("movementSpeed", Mathf.Abs(moveInput));

            //Look to the right
            spriterenderer.flipX = false;
        }
        else if(moveInput < 0)
        {
            //Walk Animation
            animator.SetFloat("movementSpeed", Mathf.Abs(moveInput));

            //Look to the left
            spriterenderer.flipX = true;
        }
    }
}
