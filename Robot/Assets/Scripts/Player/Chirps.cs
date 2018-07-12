using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class Chirps : MonoBehaviour 
{
	public int playerNum;
	public bool player2 = false;

	float timeLeft = 2.0f;
	bool startTimer = false;
	bool playEvent = false;
	int eventCount = 0;

	GameObject MelodyDoor;

	// Use this for initialization
	void Start () 
	{
		MelodyDoor = GameObject.FindGameObjectWithTag ("Doors");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ("time left = " + timeLeft);
		//Debug.Log ("event counter = " + eventCount);
		//Debug.Log ("Timer is on = " + startTimer);

		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//Debug.Log ("no controllers plugged in");
			ProcessInput();
		} 
		else
		{
			ProcessInputInControl (inputDevice);
		}

		//example of timer will be put elsewhere?
		if (startTimer == true)
		{
			timeLeft -= Time.deltaTime;
			if (eventCount >= 2)
			{
				playEvent = true;
			}
		}

		if (timeLeft <= 0)
		{
			Debug.Log ("timer is at 0, play event");
			startTimer = false;
			timeLeft = 2.0f;
			eventCount = 0;
			FalseEvent ();
		}


		if (playEvent == true)
		{
			Event ();
		}


	}

	void ProcessInputInControl(InputDevice inputDevice)
	{
		if (playerNum == 0)
		{
			//AkSoundEngine.SetSwitch ("PlayerID", "Player_1", gameObject);

			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player1enteredBounds == false)
			{
				if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					//play chirp
					Debug.Log("player 1, left arm chirp");
					//example. 
					startTimer = true;
					eventCount += 1;
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

				} 
				else if (inputDevice.DPadUp.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					//duff chirp
					Debug.Log("player 1, No Left Arm");
				}



				if (inputDevice.DPadDown.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					//play chirp
					Debug.Log ("player 1, right arm chirp");
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);
				} 
				else if (inputDevice.DPadDown.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("RightArm"))
				{
					//duff
					Debug.Log("Player1, No right arm");
				}



				if (inputDevice.DPadLeft.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					Debug.Log ("player1, left leg chirp");
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);
				} 
				else if (inputDevice.DPadLeft.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					//duff
					Debug.Log("player1, No left leg");
				}



				if (inputDevice.DPadRight.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					Debug.Log ("player1, right leg chirp");
					//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);
				} 
				else if (inputDevice.DPadRight.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					//duff
					Debug.Log("player 1, No right leg");
				}
			}
				
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if (playerNum == 1)
		{
			//AkSoundEngine.SetSwitch ("PlayerID", "Player_2", gameObject);

			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player2enteredBounds == false)
			{
				if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log("Player 2, left Arm Chirp");
				} 
				else if (inputDevice.DPadUp.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					Debug.Log ("player2, NO left arm");
				}



				if (inputDevice.DPadDown.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("player 2, right arm chirp");
				} 
				else if (inputDevice.DPadDown.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					Debug.Log ("Player2, No right arm");
				}



				if (inputDevice.DPadLeft.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					//play chirp
					AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
					AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("player2, left leg chirp");
				} 
				else if (inputDevice.DPadLeft.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					Debug.Log ("player2, No left leg");
				}



				if (inputDevice.DPadRight.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("player2, right leg chirp");
				} 
				else if (inputDevice.DPadRight.WasPressed && 
					!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					Debug.Log ("player2, No right leg");
				}
			}
				
		}
	}
		

	void ProcessInput()
	{
		//player 1
		if (player2)
		{
			//AkSoundEngine.SetSwitch ("PlayerID", "Player_1", gameObject);

		
			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player1enteredBounds == false)
			{
				if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4])
					&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{

					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log("player 1, left arm chirp");
				} 
				else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4])
					&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					Debug.Log("player 1, No Left Arm");
				}



				if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5])
					&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log("player 1, right arm chirp");
				} 
				else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5])
					&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					Debug.Log("player 1, No right Arm");
				}



				if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6])
					&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);
					Debug.Log("player 1, left leg chirp");
				} 
				else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6])
					&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					Debug.Log("player 1, No Left leg");
				}



				if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
					&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);
					Debug.Log("player 1, right leg chirp");
				} 
				else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
					&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					Debug.Log("player 1, No right leg");
				}
			}


		}//player2 
		else
		{
			//AkSoundEngine.SetSwitch ("PlayerID", "Player_2", gameObject);

			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player2enteredBounds == false)
			{
				if (Input.GetKeyDown (KeyCode.Alpha6)
					&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("Player2, left arm chirp");	
				} 
				else if (Input.GetKeyDown (KeyCode.Alpha6)
					&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
				{
					Debug.Log ("player 2, no left arm");
				}



				if (Input.GetKeyDown (KeyCode.Alpha7)
					&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("Player2, right arm chirp");	
				} 
				else if (Input.GetKeyDown (KeyCode.Alpha7)
					&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
				{
					Debug.Log ("player 2, no right arm");
				}



				if (Input.GetKeyDown (KeyCode.Alpha8)
					&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("Player2, left leg chirp");	
				} 
				else if (Input.GetKeyDown (KeyCode.Alpha8)
					&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
				{
					Debug.Log ("player 2, no left leg");
				}



				if (Input.GetKeyDown (KeyCode.Alpha9)
					&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					//play chirp
					//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
					//AkSoundEngine.PostEvent ("Chirp", gameObject);

					Debug.Log ("Player2, right leg chirp");	
				} 
				else if (Input.GetKeyDown (KeyCode.Alpha9)
					&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
				{
					Debug.Log ("player 2, no right leg");
				}
			}
				
		}
	}




	void Event()
	{
		//the event you want
		Debug.Log("Event has triggered");
		startTimer = false;
		playEvent = false;
		eventCount = 0;
		timeLeft = 2.0f;

		//AkSoundEngine.SetRTPCValue ("Chirps_Combine", 1); 
	}

	void FalseEvent()
	{
		//if the timer reaches 0
		Debug.Log("false hope");
	}



}