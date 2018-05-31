using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class SCR_Melody : MonoBehaviour 
{
	public GameObject myDoor;

	//Used for the Xinput Plug in. Tracks the state of the controllers for player 1 and 2
	GamePadState state;
	GamePadState prevState;
	GamePadState player2State;
	GamePadState player2PrevState;

	//is there 2 players in the game. if so use different controls for player 1 and 2 
	//but allows it all to be in 1 script
	public bool player2 = false;

	public AudioClip chirp1;
	public AudioClip chirp2;
	public AudioClip chirp3;
	public AudioClip chirp4;
	private AudioSource source;

	//The code the robots use to compare to the door code
	public List<int> Robotcode = new List<int> ();

	//this is the bool check for if the robotCode is the same as the door code
	bool test = true;
	public bool correctCode = false;

	//if the player is inside the door collider, will allow melodies to be played
	//bool Player1touchingDoor = false;
	//bool Player2touchingDoor = false;


	public GameObject Canvas;
	public GameObject CanvasNoteSheet;

	//counter used to track which note you are inputting i.e.
	//the first input, the second etc
	int noteCounter = 0;

	//array of arrow textures that will be applied to the rawimages when you input a note
	public Texture[] Arrows;
	//array of rawimages that are being used in the UI for the melodies
	public RawImage[] Notes;
	//Blank array which is used to take all the textures off the rawimages when a code is wrong or finished with
	RawImage[] blankNotes;


	// Use this for initialization
	void Start () 
	{
		source = GetComponent<AudioSource> ();
		blankNotes = Notes;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ProcessInput ();
		//player1 touching the door, if the correct code is inputted then no longer show the UI
		if (myDoor.GetComponent<SCR_Door>().Player1enteredBounds == true && correctCode == false || 
			myDoor.GetComponent<SCR_Door>().Player2enteredBounds == true && correctCode == false)
		{
			//touchingDoor = true;
			CanvasNoteSheet.SetActive (true);
		} 
		else
		{
			//touchingDoor = false;
			CanvasNoteSheet.SetActive (false);
		}




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

			//can only play notes when player is in proxcimity to box
		if ((myDoor.GetComponent<SCR_Door>().Player1enteredBounds == true && Robotcode.Count <= 3))
			{
				//beeps Y
				if (prevState.DPad.Up == ButtonState.Released &&
					state.DPad.Up == ButtonState.Pressed)
				{
					source.PlayOneShot (chirp1);
					Robotcode.Add (1);
					Notes [noteCounter].texture = Arrows [0]; 

					//whenever a note is played
					noteCounter += 1;
				}
				else if (prevState.DPad.Left == ButtonState.Released &&
					state.DPad.Left == ButtonState.Pressed)
				{
					source.PlayOneShot (chirp2);
					Robotcode.Add (2);

					//whenever a note is played
					Notes [noteCounter].texture = Arrows [1]; 

					//whenever a note is played
					noteCounter += 1;

				}
				else if (prevState.DPad.Right == ButtonState.Released &&
					state.DPad.Right == ButtonState.Pressed)
				{
					source.PlayOneShot (chirp3);
					Robotcode.Add (3);

					//whenever a note is played
					Notes [noteCounter].texture = Arrows [2]; 

					//whenever a note is played
					noteCounter += 1;

				}
				else if (prevState.DPad.Down == ButtonState.Released &&
					state.DPad.Down == ButtonState.Pressed)
				{
					source.PlayOneShot (chirp4);
					Robotcode.Add (4);

					//whenever a note is played
					Notes [noteCounter].texture = Arrows [3]; 

					//whenever a note is played
					noteCounter += 1;

				}

				//once 4 notes have been played check to see if the code is correct or false
				if (noteCounter == 4)
				{
					Invoke("CheckCode", 1.0f);
				}
			} 

			//////////////////////////////////////////////////////////
			/// //player 2
			/// //move forward

			//Player 2 melody notes
			//can only play notes when player is in proxcimity to box
			if ((myDoor.GetComponent<SCR_Door>().Player2enteredBounds == true && Robotcode.Count <= 3))
			{
				//beeps Y
				if (player2PrevState.DPad.Up == ButtonState.Released &&
					player2State.DPad.Up == ButtonState.Pressed || Input.GetKey(KeyCode.I))
				{
					//source.PlayOneShot (chirp1);
					Robotcode.Add (5);
					Notes [noteCounter].texture = Arrows [4]; 

					//whenever a note is played
					noteCounter += 1;
				}
				else if (player2PrevState.DPad.Left == ButtonState.Released &&
					player2State.DPad.Left == ButtonState.Pressed || Input.GetKey(KeyCode.J))
				{
					//source.PlayOneShot (chirp2);
					Robotcode.Add (6);

					//whenever a note is played
					Notes [noteCounter].texture = Arrows [5]; 

					//whenever a note is played
					noteCounter += 1;

				}
				else if (player2PrevState.DPad.Right == ButtonState.Released &&
					player2State.DPad.Right == ButtonState.Pressed || Input.GetKey(KeyCode.L))
				{
					//source.PlayOneShot (chirp3);
					Robotcode.Add (7);

					//whenever a note is played
					Notes [noteCounter].texture = Arrows [6]; 

					//whenever a note is played
					noteCounter += 1;

				}
				else if (player2PrevState.DPad.Down == ButtonState.Released &&
					player2State.DPad.Down == ButtonState.Pressed || Input.GetKey(KeyCode.K))
				{
					//source.PlayOneShot (chirp4);
					Robotcode.Add (8);

					//whenever a note is played
					Notes [noteCounter].texture = Arrows [7]; 

					//whenever a note is played
					noteCounter += 1;

				}

				//once 4 notes have been played check to see if the code is correct or false
				if (noteCounter == 4)
				{
					Invoke("CheckCode", 1.0f);
				}
			} 

	}




	void CheckCode()
	{
		//check each element of the doorcode and compare it to the robot code
		for( int i = 0; i < Robotcode.Count; i++)
		{
			//if doorcode is not the same as robotcode the code is wrong

			if (SCR_Door.Doorcode [i] != Robotcode [i])
			{
				test = false;
				Debug.Log ("Wrong code");
			} 
		}

		if (test == true)
		{
			//if the robot code is the same as the doorcode then display UI message
			Debug.Log ("code correct");
			//Canvas.SetActive (true);
			CancelInvoke ("CheckCode");
			correctCode = true;

		} else
		{
			Debug.Log ("Wrong code");
			test = true;
			Robotcode.Clear ();
		}


		//once 4 note inputs have been carried out. 
		//set the counter back to 0
		noteCounter = 0;

		//the 4 RawImages that your using to show the arrows, set their textures to null
		for (int i = 0; i < Notes.Length; i++)
		{
			Notes [i].texture = null;
		}

	}





}
