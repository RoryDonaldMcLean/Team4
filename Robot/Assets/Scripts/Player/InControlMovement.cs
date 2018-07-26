using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class InControlMovement : MonoBehaviour
{
    Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

    //is there 2 players in the game. if so use different controls for player 1 and 2 
    //but allows it all to be in 1 script
	public bool isBlue = false;

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
			//AkSoundEngine.SetRTPCValue ("LimbNumber", 3.0f);
		} 
		else if (GetLegQuantity () == 1)
		{
			playerSpeed = 3.0f;
			//AkSoundEngine.SetRTPCValue ("LimbNumber", 1.0f);

		} else
		{
			playerSpeed = 1.5f;
			//AkSoundEngine.SetRTPCValue ("LimbNumber", 0.0f);

		}

		//left and right movement
		velocity.x = inputDevice.LeftStickX;
		//forward and backwards movement
		velocity.z = inputDevice.LeftStickY;


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


	//keyboard control
	void ProcessInput()
	{
		//as movement speed is based on how many limbs you have, check this during process input

		if (GetLegQuantity () >= 2)
		{
			playerSpeed = 6.0f;
			//AkSoundEngine.SetRTPCValue ("LimbNumber", 3.0f);

		} 
		else if (GetLegQuantity () == 1)
		{
			playerSpeed = 3.0f;
			//AkSoundEngine.SetRTPCValue ("LimbNumber", 1.0f);

		} else
		{
			playerSpeed = 1.5f;
			//AkSoundEngine.SetRTPCValue ("LimbNumber", 0.0f);

		}

		//player 1
		//move forward
		if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
		{

			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[0]))
			{
				velocity.z += 1.0f;
			}
			//move backward
			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[2]))

			{
				velocity.z -= 1.0f;
			}


			//move left

			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[1]))

			{
				velocity.x -= 1.0f;
			}

			//move right

			if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[3]))

			{
				velocity.x += 1.0f;
			}

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
			}


		}
		else
		{
            //////////////////////////////////////////////////////////
            /// //player 2
            /// //move forward
            if(Input.GetKey(GameManager.Instance.playerSetting.currentButton[12]))
            {
                velocity.z += 1.0f;
            }
            //move backward
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[14]))
            {
                velocity.z -= 1.0f;
            }

            //move left
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[13]))
            {
                velocity.x -= 1.0f;
            }

            //move right
            if (Input.GetKey(GameManager.Instance.playerSetting.currentButton[15]))
            {
                velocity.x += 1.0f;
            }


            //jumping
            if ((grounded == true || doubleJump == true) && Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[20]) && this.GetLegQuantity() >= 1)
            {
                if (grounded && this.GetLegQuantity() >= 2)
                    doubleJump = true;
                else
                    doubleJump = false;
                grounded = false;
                velocity.y = jumpSpeed;
            }
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
        //Vector2 ve2 = new Vector2(vel.x, vel.z);
        //vel.x = ve2.normalized.x;
        //vel.z = ve2.normalized.y;

        rb1.velocity = vel * playerSpeed;
        Vector3 lookAt = new Vector3(rb1.velocity.x, 0, rb1.velocity.z);

        //will rotate the player to face the direction they are moving
        if (lookAt != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt), 0.1f);
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
