﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class InControlMovement : MonoBehaviour
{
    Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script

    public float playerSpeed;

    public Rigidbody rb1;
    float jumpSpeed = 5.0f;
    float dropdownSpeed = 5.0f;

    public bool grounded = true;
    public bool doubleJump = false;

	public int playerNum;

    // Use this for initialization
    void Start()
    {
        rb1 = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//Debug.Log ("no controllers plugged in");
		} 
		else
		{
			ProcessInput (inputDevice);
		}
    }

	void ProcessInput(InputDevice inputDevice)
    {
		//as movement speed is based on how many limbs you have, 
		//check this during process input
		if (GetLegQuantity () >= 2)
		{
			playerSpeed = 6.0f;
		} 
		else if (GetLegQuantity () == 1)
		{
			playerSpeed = 3.0f;
		} else
		{
			playerSpeed = 1.5f;
		}


		//left and right movement
		velocity.x = inputDevice.LeftStickX;
		//forward and backwards movement
		velocity.z = inputDevice.LeftStickY;

		/*if (inputDevice.DPadUp.WasPressed)
		{
			Debug.Log ("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
		}*/


		//jumping
		if ((grounded == true || doubleJump == true) && inputDevice.Action1 && this.GetLegQuantity () >= 1)
		{
			if (grounded && this.GetLegQuantity() >= 2)
				doubleJump = true;
			else
				doubleJump = false;
			grounded = false;
			velocity.y = jumpSpeed;
		}
			
        UpdateMovement(velocity);
        velocity.z = 0.0f;
        velocity.x = 0.0f;
        if (!grounded)
        {
            if (velocity.y >= -1.01f)
            {
                velocity.y += Physics.gravity.y * Time.deltaTime * dropdownSpeed;
            }
            else
            {
                velocity.y = -1.01f;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;

            doubleJump = false;
            velocity.y = 0;
        }
    }

    void OnTriggerExit(Collider col)
    {

        if ((col.gameObject.tag == "Ground") &&(rb1.velocity.y < 0))
        {
            grounded = false;
            doubleJump = true;
        }
    }

    //updates movement using the passed velocity vector
    void UpdateMovement(Vector3 vel)
    {
        rb1.velocity = vel * playerSpeed;

        //will rotate the player to face the direction they are moving
        transform.LookAt(transform.position + new Vector3(rb1.velocity.x, 0, rb1.velocity.z));
    }

    private int GetLegQuantity()
    {
        int quantity = 0;

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