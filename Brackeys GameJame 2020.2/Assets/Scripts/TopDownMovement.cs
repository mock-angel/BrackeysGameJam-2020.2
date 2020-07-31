//Code: Fynn Frings, 31.07.2020 (7:15pm GMT+1)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    #region VARIABLES

    private Rigidbody2D rigidbody2d;

    //Movement Variables
    public float moveSpeed = 1f;
    public Vector2 movement;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        #region ASSIGN COMPONENTS

        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();

        #endregion

        rigidbody2d.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerInput
        PlayerInput();
    }


    void FixedUpdate()
    {
        //Player Movement
        PlayerMovement();
    }

    private void PlayerInput()
    {
        #region PLAYER INPUT

        //Player Input
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        #endregion
    }
    private void PlayerMovement()
    {
        #region PLAYER MOVEMENT

        //Player Movement
        rigidbody2d.MovePosition(rigidbody2d.position + movement * moveSpeed * Time.fixedDeltaTime);

        #endregion
    }
}
