using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    #region VARIABLES

    private Rigidbody2D rigidbody2d;

    //Movement Variables
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float moveInput;

    //Jump
    public bool isGrounded;
    public float checkRadius = 0.1f;
    public Transform feetPosition;
    public LayerMask groundLayerMask;

    public bool isJumping;
    public float jumpTimeCounter;
    public float maxJumpTime = 0.25f;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        #region ASSIGN COMPONENTS

        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();

        #endregion

        //rigidbody2d.gravityScale = 8f;
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
        #region PLAYER INPUT

        //Player Input
        moveInput = Input.GetAxis("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rigidbody2d.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                rigidbody2d.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
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

        isGrounded = Physics2D.OverlapCircle(feetPosition.position, checkRadius, groundLayerMask);

        #endregion
    }
}
