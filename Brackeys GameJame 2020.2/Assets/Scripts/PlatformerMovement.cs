using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    #region VARIABLES

    private Rigidbody2D rigidbody2d;

    //Movement Variables
    public float moveSpeed = 5f;
    public float jumpForce = 15f;
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


    // Start is called before the first frame update
    void Start()
    {
        #region ASSIGN COMPONENTS

        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();

        #endregion

        //rigidbody2d.gravityScale = 8f;
        #region ASSIGN VARIABLES

        currentExtraJumpAmount = maxExtraJumpAmount;

        #endregion

        //Set Gravity
        rigidbody2d.gravityScale = 14f;
    }

    // Update is called once per frame
    void Update()
    {
        //Player Input
        PlayerInput();
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
            if(jumpTimeCounter > 0)
            {
                //Actually Jump
                rigidbody2d.velocity = Vector2.up * jumpForce;

                //Decrease jumptime
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        //Jump Multiple Times
        if(Input.GetKeyDown(KeyCode.Space) && currentExtraJumpAmount > 0 && isJumping)
        {
            //Reset JumpTimeCounter
            jumpTimeCounter = maxJumpTime;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * jumpForce;

            //Decrease Jump Amount
            currentExtraJumpAmount--;
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

        //When Player is Grounded
        if(isGrounded)
        {
            currentExtraJumpAmount = maxExtraJumpAmount;
            //isJumping = false;
        }

        #endregion
    }
}
