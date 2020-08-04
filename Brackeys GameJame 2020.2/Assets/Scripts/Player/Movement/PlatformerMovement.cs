using System.Collections;
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

    //Attack
    [Header("Attack")]
    public GameObject AttackContainer;

    //Character Stages
    [Header("Character Stages")]
    public int characterStage;

    //Movement Variables
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 16f;
    public float moveInput;

    //Jump
    [Header("Jump")]
    public int maxExtraJumpAmount = 2;
    public int currentExtraJumpAmount;

    public bool isGrounded;
    public float checkRadius = 0.1f;
    public Transform feetPosition;
    public LayerMask groundLayerMask;

    public bool isJumping;
    public float jumpTimeCounter;
    public float maxJumpTime = 0.2f;

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
        currentExtraJumpAmount = maxExtraJumpAmount;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //Change Character Stage
        ChangeCharacterStage();

        //Player Input
        PlayerInput();

        #region FLIP PLAYER

        if (!spriterenderer.flipX && moveInput < 0)
        {
            //Flip Player
            FlipPlayerSprite();
        }
        else if (spriterenderer.flipX && moveInput > 0)
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


        if (characterStage == 0)
        {

        }
        else if (characterStage == 1)
        {
            //Assign new Animation
            animatoroverrider.SetAnimationToValueInList(0);
        }
        else if (characterStage == 2)
        {
            //Assign new Animation
            animatoroverrider.SetAnimationToValueInList(1);
        }
        else if (characterStage == 3)
        {
            //Assign new Animation
            animatoroverrider.SetAnimationToValueInList(2);
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
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isJumping && currentExtraJumpAmount == maxExtraJumpAmount)
        {
            isJumping = true;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * jumpForce;

            //Decrease Jump Amount
            currentExtraJumpAmount = 0;
        }

        //Jump Once
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * jumpForce;

            //Decrease Jump Amount
            currentExtraJumpAmount--;
        }

        //Jump Longer
        else if (Input.GetKey(KeyCode.Space) && isJumping && !isGrounded)
        {
            if (jumpTimeCounter > 0 && currentExtraJumpAmount == maxExtraJumpAmount - 1)
            {
                //Decrease jumptime
                jumpTimeCounter -= Time.deltaTime;

                //Actually Jump
                rigidbody2d.velocity = Vector2.up * jumpForce;
            }
            else
            {
                rigidbody2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        //Jump Multiple Times
        if(Input.GetKeyDown(KeyCode.Space) && currentExtraJumpAmount > 0 && currentExtraJumpAmount < maxExtraJumpAmount && !isGrounded)
        {
            isJumping = true;

            //Reset JumpTimeCounter
            jumpTimeCounter = maxJumpTime;

            //Actually Jump
            rigidbody2d.velocity = Vector2.up * jumpForce;

            //Decrease Jump Amount
            currentExtraJumpAmount--;
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
        if (Input.GetKeyUp(KeyCode.Space) && currentExtraJumpAmount < maxExtraJumpAmount)
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
        rigidbody2d.velocity = new Vector2(moveInput * moveSpeed, rigidbody2d.velocity.y);


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
            jumpTimeCounter = maxJumpTime;

            //Reset Jump Amount
            currentExtraJumpAmount = maxExtraJumpAmount;

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

        if(isGrounded)
        {
            //Turn Animation
            animator.SetTrigger("turn");
        }

        //Stand still
        if (moveInput == 0)
        {

        }
        //Look to the right
        else if (moveInput > 0)
        {
            //Flip Attack Container
            AttackContainer.transform.localScale = new Vector3(1, 1, 1);

            //Flip Player Sprite
            spriterenderer.flipX = false;
        }
        //Look to the left
        else if (moveInput < 0)
        {
            //Flip Attack Container
            AttackContainer.transform.localScale = new Vector3(-1, 1, 1);

            //Flip Player Sprite
            spriterenderer.flipX = true;
        }

        #endregion
    }

    public void OnDamageTaken(int damage){
        GetComponent<Health>().ChangeHealth(-damage);
    }

}
