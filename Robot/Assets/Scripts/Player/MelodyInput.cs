using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class MelodyInput : MonoBehaviour 
{
	public int playerNum;
	GameObject MelodyDoor;

	GameObject CanvasNoteSheet;

	Sprite[] Arrows;
	List<GameObject> Notes = new List<GameObject>();
	List<GameObject> blankNotes = new List<GameObject>();

	public bool isBlue;
	GameObject MelodyDoorContainer;

	GameObject GameController;
	int levelCounter;
	int CodeTotal;


	// Use this for initialization
	void Start () 
	{
		MelodyDoorContainer = GameObject.FindGameObjectWithTag ("Doors");
		if(MelodyDoorContainer != null) MelodyDoor = MelodyDoorContainer.transform.GetChild(0).gameObject;

		Arrows = new Sprite[8];
		for(int i = 0; i < 8; i++)
		{
			Arrows[i] = Resources.Load<Sprite> ("Art/PlaceHolder/" + (i + 1)) as Sprite;
		}

		CanvasNoteSheet = GameObject.FindGameObjectWithTag ("CanvasNoteSheet");

		if (CanvasNoteSheet != null)
		{
			for (int i = 0; i < CanvasNoteSheet.transform.childCount; i++)
			{
				Notes.Add(CanvasNoteSheet.transform.GetChild(i).gameObject);
			}
		}

		blankNotes = Notes;

		GameController = GameObject.FindGameObjectWithTag ("GameController");

		levelCounter = GameController.GetComponent<LevelController>().currentLevel;

		//change the total number of digits needed depending on the what level it is
		if (levelCounter == 0)
		{
			CodeTotal = 8;
		} 
		else if (levelCounter == 1)
		{
			CodeTotal = 7;
		} 
		else if (levelCounter == 2)
		{
			CodeTotal = 6;
		} 
		else if (levelCounter == 3)
		{
			CodeTotal = 4;
		} 
		else if (levelCounter == 4)
		{
			CodeTotal = 2;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;

		if (inputDevice == null)
		{
			ProcessInput ();
		} 
		else
		{
			ProcessInputIncontrol (inputDevice);
		}
	}


	//keyboard controls
	void ProcessInput()
	{
		if (MelodyDoorContainer != null)
		{
			//player 1
			if ((MelodyDoor.GetComponent<SCR_Door> ().Player1enteredBounds == true &&
				MelodyDoor.GetComponent<InControlMelody>().Robotcode.Count < CodeTotal))
			{
				if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
				{
					if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4]))
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (1);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [0].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);
						} else
						{
							Debug.Log ("Player1 has no left arm");
						}
					} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5]))
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (2);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [3].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player 1 has no right arm");
						}
					} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6]))
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (3);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [1].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player 1 has no left leg");
						}
					} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7]))
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (4);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [2].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player1 has no right leg");
						}
					}

					if (MelodyDoor.GetComponent<InControlMelody> ().noteCounter == CodeTotal)
					{
						StartCoroutine (UIDelay ());
					}

				}
			}

			//player 2
			if ((MelodyDoor.GetComponent<SCR_Door> ().Player2enteredBounds == true &&
				MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Count < CodeTotal))
			{
				if (isBlue != GameManager.Instance.whichAndroid.player1ControlBlue)
				{
					if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [16]))
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (5);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [4].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no left arm");
						}
					} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [17]))
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (6);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [7].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no right arm");
						}
					} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18]))
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (7);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [5].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no left leg");
						}
					} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19]))
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (8);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [6].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no right leg");
						}
					}

					if (MelodyDoor.GetComponent<InControlMelody> ().noteCounter == CodeTotal)
					{
						StartCoroutine (UIDelay ());
					}
				}
			}
		}
	}


	//controller input
	void ProcessInputIncontrol(InputDevice inputDevice)
	{
		if (MelodyDoorContainer != null)
		{

			//player 1
			if ((MelodyDoor.GetComponent<SCR_Door> ().Player1enteredBounds == true &&
				MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Count < CodeTotal))
			{
				if (playerNum == 0)
				{
					if (inputDevice.DPadUp.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (1);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [0].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player1 has no left arm");
						}
					} else if (inputDevice.DPadDown.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (2);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [3].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player 1 has no right arm");
						}
					} else if (inputDevice.DPadLeft.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (3);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [1].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player 1 has no left leg");
						}
					} else if (inputDevice.DPadRight.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (4);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [2].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);


						} else
						{
							Debug.Log ("Player1 has no right leg");
						}
					}

					if (MelodyDoor.GetComponent<InControlMelody> ().noteCounter == CodeTotal)
					{
						StartCoroutine (UIDelay ());
					}

				}
			}

			//player 2
			if ((MelodyDoor.GetComponent<SCR_Door> ().Player2enteredBounds == true &&
				MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Count < CodeTotal))
			{
				if (playerNum == 1)
				{
					if (inputDevice.DPadUp.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (5);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [4].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no left arm");
						}
					} else if (inputDevice.DPadDown.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (6);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [7].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no right arm");
						}
					} else if (inputDevice.DPadLeft.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (7);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [5].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no left leg");
						}
					} else if (inputDevice.DPadRight.WasPressed)
					{
						if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							MelodyDoor.GetComponent<InControlMelody> ().Robotcode.Add (8);
							Notes [MelodyDoor.GetComponent<InControlMelody> ().noteCounter].GetComponent<RawImage> ().texture = Arrows [6].texture;
							MelodyDoor.GetComponent<InControlMelody> ().noteCounter += 1;
							//AkSoundEngine.PostEvent ("DoorChirp", gameObject);

						} else
						{
							Debug.Log ("Player2 has no right leg");
						}
					}

					if (MelodyDoor.GetComponent<InControlMelody> ().noteCounter == CodeTotal)
					{
						StartCoroutine (UIDelay ());
					}
				}
			}

		}
	}

	IEnumerator UIDelay()
	{
		yield return new WaitForSeconds (1);
		MelodyDoor.GetComponent<InControlMelody> ().CheckCode ();
	}
}
