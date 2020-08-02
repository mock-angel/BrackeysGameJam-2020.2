using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Range(1, 10)]
    public float jumpVelocity = 6f;
    public LayerMask groundLayer;
    public Transform feetPosition;

    void Update()
    {   
        print("reach here");

        if(Input.GetButtonDown("Jump")){
            print("jump initiated");
        }
        if(IsGrounded()){
            print("is grounded");
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
        }
    }
    
    //Copied from Assets/Scripts/PlayerScript.cs as temporary fix.
    private bool IsGrounded()
    {
        Collider2D coll = Physics2D.OverlapCircle(feetPosition.position, .7f, groundLayer);
        if (coll != null)
        {
            return true;
        }
        else
            return false;
    }
}
