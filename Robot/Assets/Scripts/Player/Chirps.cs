using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class Chirps : MonoBehaviour 
{
	public int playerNum;
	public bool isBlue = false;

	int EmoteNumber = 0;
	private bool ActiveEmote = false;

	private GameObject Face;
	private GameObject Face2;
	private float range = 100f;

	float timeLeft = 2.0f;
	bool startTimer = false;
	bool playEvent = false;
	int eventCount = 0;

	GameObject MelodyDoor;

	GameObject UIEmoteImage;
	GameObject UIEmoteImage2;


	// Use this for initialization
	void Start () 
	{
		MelodyDoor = GameObject.FindGameObjectWithTag ("Doors");
		Face = GameObject.FindGameObjectWithTag ("Test");
		Face2 = GameObject.FindGameObjectWithTag ("Test2");


	}


	
	// Update is called once per frame
	void Update () 
	{
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
		if (MelodyDoor != null)
		{

			if (playerNum == 0)
			{
				//AkSoundEngine.SetSwitch ("PlayerID", "Player_1", gameObject);

				if (inputDevice.RightBumper.IsPressed)
				{
					if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player1enteredBounds == false)
					{
						if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							Debug.Log ("player 1, left arm chirp");
							//example. 
							startTimer = true;
							eventCount += 1;
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

						} else if (inputDevice.DPadUp.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							//duff chirp
							Debug.Log ("player 1, No Left Arm");
						}



						if (inputDevice.DPadDown.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							Debug.Log ("player 1, right arm chirp");
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadDown.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("RightArm"))
						{
							//duff
							Debug.Log ("Player1, No right arm");
						}



						if (inputDevice.DPadLeft.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							Debug.Log ("player1, left leg chirp");
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadLeft.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							//duff
							Debug.Log ("player1, No left leg");
						}



						if (inputDevice.DPadRight.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							Debug.Log ("player1, right leg chirp");
							//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadRight.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							//duff
							Debug.Log ("player 1, No right leg");
						}
					}
				}

			}
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (playerNum == 1)
			{
				//AkSoundEngine.SetSwitch ("PlayerID", "Player_2", gameObject);
				if (inputDevice.RightBumper.IsPressed)
				{
					if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player2enteredBounds == false)
					{
						if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player 2, left Arm Chirp");
						} else if (inputDevice.DPadUp.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							Debug.Log ("player2, NO left arm");
						}



						if (inputDevice.DPadDown.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player 2, right arm chirp");
						} else if (inputDevice.DPadDown.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							Debug.Log ("Player2, No right arm");
						}



						if (inputDevice.DPadLeft.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player2, left leg chirp");
						} else if (inputDevice.DPadLeft.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							Debug.Log ("player2, No left leg");
						}



						if (inputDevice.DPadRight.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player2, right leg chirp");
						} else if (inputDevice.DPadRight.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							Debug.Log ("player2, No right leg");
						}
					}
				}

			}
		}
	}
		

	void ProcessInput()
	{
		if (MelodyDoor != null)
		{

			//player 1
			if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
			{
				//AkSoundEngine.SetSwitch ("PlayerID", "Player_1", gameObject);
				if (Input.GetKey (GameManager.Instance.playerSetting.currentButton [24]))
				{
					if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player1enteredBounds == false)
					{
						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4])
						    && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player 1, left arm chirp");
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4])
						           && !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							Debug.Log ("player 1, No Left Arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5])
						    && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player 1, right arm chirp");
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5])
						           && !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							Debug.Log ("player 1, No right Arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6])
						    && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);
							Debug.Log ("player 1, left leg chirp");
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6])
						           && !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							Debug.Log ("player 1, No Left leg");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
						    && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);
							Debug.Log ("player 1, right leg chirp");
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
						           && !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							Debug.Log ("player 1, No right leg");
						}
					}

				}

			}//player2 
		else
			{
				if (Input.GetKey (GameManager.Instance.playerSetting.currentButton [25]))
				{
					//AkSoundEngine.SetSwitch ("PlayerID", "Player_2", gameObject);

					if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player2enteredBounds == false)
					{
						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [16])
						    && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, left arm chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [16])
						           && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							Debug.Log ("player 2, no left arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [17])
						    && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, right arm chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [17])
						           && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							Debug.Log ("player 2, no right arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18])
						    && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, left leg chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18])
						           && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							Debug.Log ("player 2, no left leg");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19])
						    && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							//play chirp
							//AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							//AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, right leg chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19])
						           && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							Debug.Log ("player 2, no right leg");
						}
					}

				} 

			}
		}
	}


	void Emotes()
	{
		if (EmoteNumber == 1 && ActiveEmote == false)
		{
			StartCoroutine (EmoteActive ());
			ActiveEmote = true;
			GameObject Emote1 = Resources.Load ("Prefabs/Particle/Happy") as GameObject;
			Instantiate (Emote1, transform.position, Quaternion.identity, transform);
		}
		if (EmoteNumber == 2 && ActiveEmote == false)
		{
			StartCoroutine (EmoteActive ());
			ActiveEmote = true;
			GameObject Emote2 = Resources.Load ("Prefabs/Particle/Sad") as GameObject;
			Instantiate (Emote2, transform.position, Quaternion.identity, transform);
		}
		if (EmoteNumber == 3 && ActiveEmote == false)
		{
			StartCoroutine (EmoteActive ());
			ActiveEmote = true;
			GameObject Emote3 = Resources.Load ("Prefabs/Particle/OverHere") as GameObject;
			Instantiate (Emote3, transform.position, Quaternion.identity, transform);
		}
		if (EmoteNumber == 4 && ActiveEmote == false)
		{
			RaycastHit hit;
			if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
			{
				if (Physics.Raycast (Face.transform.position, Face.transform.forward, out hit, range))
				{
					Debug.Log (hit.transform.name);
					GameObject impactEffect = Resources.Load ("Prefabs/Particle/OverThereSwirl") as GameObject;
					GameObject Impact = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
					Destroy (Impact, 2f);
				}
				//player 1
			} else
			{
				if (Physics.Raycast (Face2.transform.position, Face2.transform.forward, out hit, range))
				{
					Debug.Log (hit.transform.name);
					GameObject impactEffect = Resources.Load ("Prefabs/Particle/OverThereSwirl") as GameObject;
					GameObject Impact = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));
					Destroy (Impact, 2f);
				}
				//player2
			}
			StartCoroutine (EmoteActive ());
			ActiveEmote = true;
			GameObject Emote4 = Resources.Load ("Prefabs/Particle/OverThereSwirl") as GameObject;
			Instantiate (Emote4, transform.position, Quaternion.identity, transform);
			Debug.Log ("mercy");
		}	
	}	

	IEnumerator EmoteActive()
	{
		yield return new WaitForSeconds (1);
		ActiveEmote = false;
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