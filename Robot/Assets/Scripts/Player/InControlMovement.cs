﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class InControlMovement : MonoBehaviour
{
    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
    #region public variable
    public bool isBlue = false;
    public int playerNum;
    public float jumpSpeed = 3.3f;
    public float turnSpeed = 1.0f;
    public float gravityAcceleration = -10.0f;
    #endregion

    #region private variable
    private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    private Rigidbody rb1;

    private bool grounded = true;
    private bool doubleJump = false;
    private float playerSpeed = 6;

    private Animator anim;

    private Vector2 input;
    private float currentAngle;
    private Vector3 EulerAngleVelocity;
    private Transform cam;
    private Quaternion targetRotation;
    #endregion
    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb1 = GetComponent<Rigidbody>();
		cam = Camera.main.transform;
        if(this.tag == "Player1")
        {
            playerNum = 0;
        }
        else
        {
            playerNum = 1;
            isBlue = true;
        }
    }


	/*void FixedUpdate()
	{
		GetInput ();

		if (Mathf.Abs (input.x) < 1 && Mathf.Abs (input.y) < 1)
			return;

		CalculateDirection ();
		Rotate ();
		Move ();

	}*/

	void GetInput()
	{
		input.x = Input.GetAxisRaw ("Horizontal");
		input.y = Input.GetAxisRaw ("Vertical");
	}

	void CalculateDirection()
	{
		currentAngle = Mathf.Atan2 (input.x, input.y);
		currentAngle = Mathf.Rad2Deg * currentAngle;
		currentAngle += cam.eulerAngles.y;
	}

	void Rotate()
	{
		targetRotation = Quaternion.Euler (0, currentAngle, 0);
		rb1.rotation = Quaternion.Slerp (transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
	}

	void Move()
	{
		rb1.MovePosition (transform.position + transform.forward * playerSpeed * Time.deltaTime);
	}

    void FootstepP1()
    {
            AkSoundEngine.PostEvent("FootstepP1", gameObject); 
    }

    void FootstepP2()
    {
            AkSoundEngine.PostEvent("FootstepP2", gameObject);
    }

    void PushingRTPC()
    {
        AkSoundEngine.SetRTPCValue("Strain", 100);
    }

    void PushingStopRTPC()
    {
        AkSoundEngine.SetRTPCValue("Strain", 0);
    }

    void Switch()
    {
        AkSoundEngine.PostEvent("Switch", gameObject);
    }

    void Idle()
    {
        AkSoundEngine.PostEvent("Idle", gameObject);
    }


    // Update is called once per frame
    void Update()
    {
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//no controllers plugged in
			//Debug.Log("No controllers plugged in, using keyboard controls");
			ProcessInput();
		} 
		else
		{
			//Debug.Log ("Controllers are plugged in, using plug in");
			ProcessInputInControl (inputDevice);
		}
    }

	//controller control
	void ProcessInputInControl(InputDevice inputDevice)
    {
		
		//as movement speed is based on how many limbs you have, 
		//check this during process input
		if (GetLegQuantity () >= 2)
		{
			playerSpeed = 6.0f;
			AkSoundEngine.SetRTPCValue ("LimbNumber", 3.0f);
		} 
		else if (GetLegQuantity () == 1)
		{
			playerSpeed = 3.0f;
			AkSoundEngine.SetRTPCValue ("LimbNumber", 1.0f);

		} else
		{
			playerSpeed = 1.5f;
			AkSoundEngine.SetRTPCValue ("LimbNumber", 0.0f);

		}

		//left and right movement
		velocity.x = inputDevice.LeftStickX;
		//forward and backwards movement
		velocity.z = inputDevice.LeftStickY;

        if(inputDevice.LeftStickX || inputDevice.LeftStickY)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

		//jumping
		if ((grounded == true || doubleJump == true) && inputDevice.Action1 && this.GetLegQuantity () >= 1)
		{
			if (grounded && this.GetLegQuantity() >= 2)
				doubleJump = true;
			else
				doubleJump = false;
			grounded = false;
			velocity.y = jumpSpeed;
            //anim.SetBool("IsJumping", true);
        }
			
        UpdateMovement(velocity);
        velocity.z = 0.0f;
        velocity.x = 0.0f;
        if (!grounded)
        {
            if (velocity.y >= -jumpSpeed)
            {
                velocity.y += Time.deltaTime * gravityAcceleration;
            }
            else
            {
                velocity.y = -jumpSpeed;
            }
        }
    }


	//keyboard control
	void ProcessInput()
	{
		//as movement speed is based on how many limbs you have, check this during process input

		if (GetLegQuantity () >= 2)
		{
			playerSpeed = 6.0f;
			AkSoundEngine.SetRTPCValue ("LimbNumber", 3.0f);

		} 
		else if (GetLegQuantity () == 1)
		{
			playerSpeed = 3.0f;
			AkSoundEngine.SetRTPCValue ("LimbNumber", 1.0f);

		} else
		{
			playerSpeed = 1.5f;
			AkSoundEngine.SetRTPCValue ("LimbNumber", 0.0f);

		}

		//player 1
		//move forward
		if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
		{

			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[0]))
			{
				velocity.z += 1.0f;
                anim.SetBool("IsMoving", true);
			}

			//move backward
			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[2]))

			{
				velocity.z -= 1.0f;
                anim.SetBool("IsMoving", true);
            }


			//move left
			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[1]))
			{
				velocity.x -= 1.0f;
                anim.SetBool("IsMoving", true);
            }

			//move right
			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[3]))

			{
				velocity.x += 1.0f;
                anim.SetBool("IsMoving", true);
            }

            if(!(Input.GetKey(GameManager.Instance.playerSetting.currentButton[0]) ||
                Input.GetKey(GameManager.Instance.playerSetting.currentButton[2]) ||
                Input.GetKey(GameManager.Instance.playerSetting.currentButton[1]) ||
                Input.GetKey(GameManager.Instance.playerSetting.currentButton[3])))
                anim.SetBool("IsMoving", false);


            //if player 1 presses the A button or the left ctrl button AND they are on the ground AND! have at least 1 leg
            //JUMP!!!
            if ((grounded ==true || doubleJump == true) && Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[8]))

			{
				if (grounded && this.GetLegQuantity() >= 2)
					doubleJump = true;
				else
					doubleJump = false;
				grounded = false;
				velocity.y = jumpSpeed;
                //anim.SetBool("IsJumping", true);
            }
				
		}
		else
		{
            //////////////////////////////////////////////////////////
            /// //player 2
            /// //move forward
            if(Input.GetKey(GameManager.Instance.playerSetting.currentButton[13]))
            {
                velocity.z += 1.0f;
                anim.SetBool("IsMoving", true);
            }
            //move backward
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[15]))
            {
                velocity.z -= 1.0f;
                anim.SetBool("IsMoving", true);
            }

            //move left
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[14]))
            {
                velocity.x -= 1.0f;
                anim.SetBool("IsMoving", true);
            }

            //move right
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[16]))
            {
                velocity.x += 1.0f;
                anim.SetBool("IsMoving", true);
            }

             

            if (!(Input.GetKey(GameManager.Instance.playerSetting.currentButton[13]) ||
                Input.GetKey(GameManager.Instance.playerSetting.currentButton[15]) ||
                Input.GetKey(GameManager.Instance.playerSetting.currentButton[14]) ||
                Input.GetKey(GameManager.Instance.playerSetting.currentButton[16])))
                anim.SetBool("IsMoving", false);


            //jumping
            if ((grounded == true || doubleJump == true) && Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[21]) && this.GetLegQuantity() >= 1)
            {
                if (grounded && this.GetLegQuantity() >= 2)
                    doubleJump = true;
                else
                    doubleJump = false;
                grounded = false;
                velocity.y = jumpSpeed;
                //anim.SetBool("IsJumping", true);
            }
        }

		UpdateMovement(velocity);
		velocity.z = 0.0f;
		velocity.x = 0.0f;
		if (!grounded)
		{
			if (velocity.y >= -jumpSpeed)
			{
				velocity.y += Time.deltaTime * gravityAcceleration;
			}
			else
			{
				velocity.y = -jumpSpeed;
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
            //anim.SetBool("IsJumping", false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if ((col.gameObject.tag == "Ground") && (rb1.velocity.y == 0))
        {
            grounded = false;
            doubleJump = true;
            velocity.y = -jumpSpeed;
        }
    }

    //updates movement using the passed velocity vector
    void UpdateMovement(Vector3 vel)
    {
//		Debug.Log ("velocity.x = " + velocity.x);
        //Vector2 ve2 = new Vector2(vel.x, vel.z);
        //vel.x = ve2.normalized.x;
        //vel.z = ve2.normalized.y;

        rb1.velocity = vel * playerSpeed;
        Vector3 lookAt = new Vector3(rb1.velocity.x, 0, rb1.velocity.z);

        //will rotate the player to face the direction they are moving
        if (lookAt != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), 0.3f);
    
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
                    //find the object that has the "Arm" in it's name
                    Transform limb = this.transform.GetChild(i).transform.GetChild(u);
                    if (limb.name.Contains("Leg") && (limb.gameObject.activeSelf))
                    {
                        quantity++;
                    }
                }
            }
        }
        return quantity;
    }

}
