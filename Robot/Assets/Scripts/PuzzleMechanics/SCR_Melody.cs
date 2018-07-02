using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;

public class SCR_Melody : MonoBehaviour 
{
	//public GameObject myDoor;
	//Used for the Xinput Plug in. Tracks the state of the controllers for player 1 and 2
	GamePadState state;
	GamePadState prevState;
	GamePadState player2State;
	GamePadState player2PrevState;

	//is there 2 players in the game. if so use different controls for player 1 and 2 
	//but allows it all to be in 1 script
	public bool player2 = false;

	AudioClip[] Chirps;

	private AudioSource source;

	//The code the robots use to compare to the door code
	public List<int> Robotcode = new List<int> ();

	//this is the bool check for if the robotCode is the same as the door code
	bool test = true;
	public bool correctCode = false;

	//public GameObject Canvas;
	GameObject CanvasNoteSheet;


	//counter used to track which note you are inputting i.e.
	//the first input, the second etc
	int noteCounter = 0;

	//array of arrow textures that will be applied to the rawimages when you input a note
	Sprite[] Arrows;

	//array of rawimages that are being used in the UI for the melodies
	List<GameObject> Notes = new List<GameObject>();

	//Blank array which is used to take all the textures off the rawimages when a code is wrong or finished with
	List<GameObject> blankNotes = new List<GameObject>();


	// Use this for initialization
	void Start () 
	{
		source = GetComponent<AudioSource> ();
		Chirps = new AudioClip[4];
		//get all the audio chirps from resources folder
		for (int i = 0; i < 4; i++)
		{
			Chirps [i] = Resources.Load<AudioClip> ("Audio/Chirps/Chirp " + (i + 1)) as AudioClip;
		}

		//these are the sprites that get added to the notesheet when you play a note (red and blue arrows)
		Arrows = new Sprite[8];
		for(int i = 0; i < 8; i++)
		{
			Arrows[i] = Resources.Load<Sprite> ("Art/PlaceHolder/" + (i + 1)) as Sprite;
		}
	

		CanvasNoteSheet = GameObject.FindGameObjectWithTag ("CanvasNoteSheet");
		for (int i = 0; i < CanvasNoteSheet.transform.childCount; i++)
		{
			Notes.Add(CanvasNoteSheet.transform.GetChild(i).gameObject);
		}
			
		source = GetComponent<AudioSource> ();
		blankNotes = Notes;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ProcessInput ();
		//player1 touching the door, if the correct code is inputted then no longer show the UI
		if (this.GetComponent<SCR_Door>().Player1enteredBounds == true && correctCode == false || 
			this.GetComponent<SCR_Door>().Player2enteredBounds == true && correctCode == false)
		{
			CanvasNoteSheet.SetActive (true);
		} 
		else
		{
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
		if ((this.GetComponent<SCR_Door>().Player1enteredBounds == true && Robotcode.Count <= 3))
			{
				//beeps Y
				if (prevState.DPad.Up == ButtonState.Released &&
				state.DPad.Up == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha1))
				{
					//if player 1 has the left arm
					//limbs[0] is left arm. so if it was limbs[1] that would be right arm
					if(GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[0].name.Contains("LeftArm"))
					{
						//allow you to play the chirp
						source.PlayOneShot (Chirps[0]);
						Robotcode.Add (1);
						Notes[noteCounter].GetComponent<RawImage>().texture = Arrows[0].texture;
						//whenever a note is played
						noteCounter += 1;
					} 
					else
					{
						//no beep
						// want to add a cross? to show that you don't have the correct limbs
						Debug.Log("player 1 has no left arm");
					}
				}
				else if (prevState.DPad.Left == ButtonState.Released &&
				state.DPad.Left == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha2))
					{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
					{
						source.PlayOneShot (Chirps[1]);
						Robotcode.Add (2);
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows[1].texture;
						//whenever a note is played
						noteCounter += 1;
					} 
					else
					{
						Debug.Log ("player 1 has no right arm");
					}

				}
				else if (prevState.DPad.Right == ButtonState.Released &&
				state.DPad.Right == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha3))
				{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
					{
						source.PlayOneShot (Chirps [2]);
						Robotcode.Add (3);
						//whenever a note is played
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows [2].texture;
						//whenever a note is played
						noteCounter += 1;
					} else
					{
						Debug.Log ("player 1 has no left leg");
					}

				}
				else if (prevState.DPad.Down == ButtonState.Released &&
				state.DPad.Down == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha4))
				{
					if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
					{
						source.PlayOneShot (Chirps [3]);
						Robotcode.Add (4);
						//whenever a note is played
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows [3].texture;
						//whenever a note is played
						noteCounter += 1;
					} else
					{
					Debug.Log ("player1 has no right leg");
					}
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
			if ((this.GetComponent<SCR_Door>().Player2enteredBounds == true && Robotcode.Count <= 3))
			{
				//beeps Y
				if (player2PrevState.DPad.Up == ButtonState.Released &&
				player2State.DPad.Up == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha6))
				{
					
					if(GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[0].name.Contains("LeftArm"))
					{
						//allow you to play the chirp
						Robotcode.Add (5);
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows[4].texture;
						//whenever a note is played
						noteCounter += 1;
					} 
					else
					{
						//no beep
						// want to add a cross? to show that you don't have the correct limbs
						Debug.Log("player2 has no left arm");
					}
						
				}
				else if (player2PrevState.DPad.Left == ButtonState.Released &&
				player2State.DPad.Left == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha7))
				{

					if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
					{
						Robotcode.Add (6);
						//whenever a note is played
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows [5].texture;
						//whenever a note is played
						noteCounter += 1;
					} else
					{
						Debug.Log ("player 2 has no right arm");
					}

				}
				else if (player2PrevState.DPad.Right == ButtonState.Released &&
				player2State.DPad.Right == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha8))
				{
					if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
					{
						Robotcode.Add (7);
						//whenever a note is played
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows [6].texture;
						//whenever a note is played
						noteCounter += 1;
					} 
					else
					{
						Debug.Log ("player2 has no left leg");
					}
				}
				else if (player2PrevState.DPad.Down == ButtonState.Released &&
				player2State.DPad.Down == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Alpha9))
				{
					if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
					{
						Robotcode.Add (8);
						//whenever a note is played
						Notes [noteCounter].GetComponent<RawImage> ().texture = Arrows [7].texture;
						//whenever a note is played
						noteCounter += 1;
					}
					else
					{
						Debug.Log ("player 2 has no right leg");
					}
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
		for(int i = 0; i < Robotcode.Count; i++)
		{
			//if doorcode is not the same as robotcode the code is wrong
			if (this.GetComponent<SCR_Door>().Doorcode[i] != Robotcode [i])
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
		for (int i = 0; i < Notes.Count; i++)
		{
			Notes[i].GetComponent<RawImage>().texture = null;
		}

	}
		
}
