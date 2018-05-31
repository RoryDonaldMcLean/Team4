using System.Collections;
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

	Vector3 velocity = new Vector3 (0.0f, 0.0f, 0.0f);

	//is there 2 players in the game. if so use different controls for player 1 and 2 
	//but allows it all to be in 1 script
	public bool player2 = false;


	public float playerSpeed = 4.0f;

	public Rigidbody rb1;
	public float jumpForce = 10;

	public bool grounded = true;


	// Use this for initialization
	void Start () 
	{
		rb1 = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ProcessInput ();
	}

	void ProcessInput()
	{
		//update the game controller
		prevState =state;
		state = GamePad.GetState (PlayerIndex.One);

		player2PrevState = player2State;
		player2State = GamePad.GetState (PlayerIndex.Two);

		//player 1
		//move forward
		if (player2)
		{
			if (Input.GetKey (KeyCode.W) || (prevState.ThumbSticks.Left.Y > 0.1))
			{
				velocity.z += 1.0f;

			}
			//move backward
			if (Input.GetKey (KeyCode.S) || (prevState.ThumbSticks.Left.Y < -0.1))
			{
				velocity.z -= 1.0f;
			}

			//move left
			if (Input.GetKey (KeyCode.A) || (prevState.ThumbSticks.Left.X < -0.1))
			{
				velocity.x -= 1.0f;
			}

			//move right
			if (Input.GetKey (KeyCode.D) || (prevState.ThumbSticks.Left.X > 0.1))
			{
				velocity.x += 1.0f;
			}

			//jumping
			if (grounded == true && prevState.Buttons.A == ButtonState.Released &&
			    state.Buttons.A == ButtonState.Pressed)
			{
				grounded = false;
				rb1.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
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
			if (Input.GetKey (KeyCode.DownArrow) || (player2PrevState.ThumbSticks.Left.Y < -0.1))
			{
				velocity.z -= 1.0f;
			}

			//move left
			if (Input.GetKey (KeyCode.LeftArrow) || (player2PrevState.ThumbSticks.Left.X < -0.1))
			{
				velocity.x -= 1.0f;
			}

			//move right
			if (Input.GetKey (KeyCode.RightArrow) || (player2PrevState.ThumbSticks.Left.X > 0.1))
			{
				velocity.x += 1.0f;
			}

			//jumping
			if (grounded == true && player2PrevState.Buttons.A == ButtonState.Released &&
				player2State.Buttons.A == ButtonState.Pressed || grounded == true && Input.GetKey(KeyCode.G))
			{
				grounded = false;
				//regardless of the jumpforce it only does a tiny hop
				rb1.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
			}

		}


		updateMovement (velocity);
		velocity.z = 0.0f;
		velocity.x = 0.0f;
	}

	//updates movement using the passed velocity vector
	void updateMovement(Vector3 vel)
	{
		//Rigidbody rb1 = GetComponent<Rigidbody> ();
		rb1.velocity = vel * playerSpeed;

		//will rotate the player to face the direction they are moving
		transform.LookAt (transform.position + rb1.velocity);
	}



	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Ground")
		{
			grounded = true;
		}
	}


}
