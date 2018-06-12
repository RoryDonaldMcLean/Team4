﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class Movement_ : MonoBehaviour
{
    //Used for the Xinput Plug in. Tracks the state of the controllers for player 1 and 2
    GamePadState state;
    GamePadState prevState;
    GamePadState player2State;
    GamePadState player2PrevState;

    Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    public bool player2 = false;

    public float playerSpeed;

    public Rigidbody rb1;
    float jumpSpeed = 5.0f;
    float dropdownSpeed = 5.0f;

    public bool grounded = true;
    public bool doubleJump = false;

	int quantity;

    // Use this for initialization
    void Start()
    {
        rb1 = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        //update the game controller
        prevState = state;
        state = GamePad.GetState(PlayerIndex.One);

        player2PrevState = player2State;
        player2State = GamePad.GetState(PlayerIndex.Two);

		//as movement speed is based on how many limbs you have, check this during process input
		GetLegQuantity ();
		if (quantity >= 2)
		{
			playerSpeed = 6.0f;
		} else if (quantity == 1)
		{
			playerSpeed = 3.0f;
		} else
		{
			playerSpeed = 1.5f;
		}

        //player 1
        //move forward
        if (player2)
        {
			if (Input.GetKey(KeyCode.W) || (prevState.ThumbSticks.Left.Y > 0.1))
            {
                velocity.z += 1.0f;
            }

            //move backward
			if (Input.GetKey(KeyCode.S) || (prevState.ThumbSticks.Left.Y < -0.1))
            {
                velocity.z -= 1.0f;
            }


            //move left
			if (Input.GetKey(KeyCode.A) || (prevState.ThumbSticks.Left.X < -0.1))
            {
                velocity.x -= 1.0f;
            }

            //move right
			if (Input.GetKey(KeyCode.D) || (prevState.ThumbSticks.Left.X > 0.1))
            {
                velocity.x += 1.0f;
            }

			//if player 1 presses the A button or the left ctrl button AND they are on the ground AND! have at least 1 leg
			//JUMP!!!
			if ((grounded ==true || doubleJump == true) && Input.GetKeyDown(KeyCode.LeftControl) && this.GetLegQuantity() >= 1 || 
				(grounded ==true || doubleJump == true) && prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed 
				&& this.GetLegQuantity() >= 1)
            {
                if (grounded && this.GetLegQuantity() >= 2)
                    doubleJump = true;
                else
                    doubleJump = false;
                grounded = false;
                velocity.y = jumpSpeed;
            }


        }
        else
        {
            //////////////////////////////////////////////////////////
            /// //player 2
            /// //move forward
            if (Input.GetKey(KeyCode.UpArrow) || (player2PrevState.ThumbSticks.Left.Y > 0.1))
            {
                velocity.z += 1.0f;
            }
            //move backward
            if (Input.GetKey(KeyCode.DownArrow) || (player2PrevState.ThumbSticks.Left.Y < -0.1))
            {
                velocity.z -= 1.0f;
            }


            //move left
            if (Input.GetKey(KeyCode.LeftArrow) || (player2PrevState.ThumbSticks.Left.X < -0.1))
            {
                velocity.x -= 1.0f;
            }

            //move right
            if (Input.GetKey(KeyCode.RightArrow) || (player2PrevState.ThumbSticks.Left.X > 0.1))
            {
                velocity.x += 1.0f;
            }
            

            //jumping
			if ((grounded ==true || doubleJump == true) && Input.GetKeyDown(KeyCode.RightControl) && this.GetLegQuantity() >= 1 || 
				(grounded == true || doubleJump == true) && player2PrevState.Buttons.A == ButtonState.Released && player2State.Buttons.A == ButtonState.Pressed
				&& this.GetLegQuantity() >= 1)
            {
				Debug.Log (quantity);
                if (grounded && this.GetLegQuantity() >= 2)
                    doubleJump = true;
                else
                    doubleJump = false;
                grounded = false;
                velocity.y = jumpSpeed;
            }

        }

		//if you are in the air then apply a downward force
		if (!grounded)
		{
			if (velocity.y >= -1)
			{
				velocity.y += Physics.gravity.y * Time.deltaTime * dropdownSpeed;
			} 
			else
			{
				velocity.y = -1.01f;
			}
		} 
		else
		{
			velocity.y = 0;
		}

		updateMovement(velocity);
		velocity.z = 0.0f;
		velocity.x = 0.0f;
    }

    //updates movement using the passed velocity vector
    void updateMovement(Vector3 vel)
    {
        rb1.velocity = vel * playerSpeed;

        //will rotate the player to face the direction they are moving
        transform.LookAt(transform.position + new Vector3(rb1.velocity.x, 0, rb1.velocity.z));
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    private int GetLegQuantity()
    {
        quantity = 0;
        //loop through all the child objects attached to player
        for (int i = 0; i < this.transform.childCount; i++)
        {
            //find the object that has the "area" in it's name
            if (this.transform.GetChild(i).name.Contains("area"))
            {
                //loop through all of that objects children, they should all be the hinges OR Limbs
                for (int u = 0; u < this.transform.GetChild(i).childCount; u++)
                {
                    //find the object that has the "Leg" in it's name
                    if (this.transform.GetChild(i).transform.GetChild(u).name.Contains("Leg"))
                    {
                        quantity++;
                    }
                }
            }

        }
        return quantity;
    }

}
