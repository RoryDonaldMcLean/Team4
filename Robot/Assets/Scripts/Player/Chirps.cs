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
	bool chirpsButton = false;


	int eventCount = 0;

	GameObject MelodyDoor;

	GameObject UIEmoteImage;
	List<GameObject> EmoteUI = new List<GameObject> ();

	GameObject UIEmoteImage2;
	List<GameObject> EmoteUI2 = new List<GameObject>();


	bool scriptStart = false;
	bool ControllersUsed;

	// Use this for initialization
	private void Start () 
	{
		MelodyDoor = GameObject.FindGameObjectWithTag ("Doors");
		Face = GameObject.FindGameObjectWithTag ("Test");
		Face2 = GameObject.FindGameObjectWithTag ("Test2");

		UIEmoteImage = GameObject.FindGameObjectWithTag ("EmoteIcon");
		if (UIEmoteImage != null)
		{
			for (int i = 0; i < UIEmoteImage.transform.childCount; i++)
			{
				EmoteUI.Add (UIEmoteImage.transform.GetChild (i).gameObject);
				EmoteUI [i].SetActive (false);
			}
		} 

		UIEmoteImage2 = GameObject.FindGameObjectWithTag ("EmoteIcon2");
		if (UIEmoteImage2 != null)
		{
			for (int i = 0; i < UIEmoteImage2.transform.childCount; i++)
			{
				EmoteUI2.Add (UIEmoteImage2.transform.GetChild (i).gameObject);
				EmoteUI2 [i].SetActive (false);
			}
		}

		//UIEmoteImage.SetActive (false);
		//UIEmoteImage2.SetActive (false);
		scriptStart = true;
	}


	
	// Update is called once per frame
	void Update () 
	{
		//bullshit way of getting around the null referance pointer for the UIEmoteImages
		if (scriptStart == true)
		{
			UIEmoteImage.SetActive (false);
			UIEmoteImage2.SetActive (false);
			scriptStart = false;
		}

		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//Debug.Log ("no controllers plugged in");
			ProcessInput();
			ControllersUsed = false;
		} 
		else
		{
			ProcessInputInControl (inputDevice);
			ControllersUsed = true;
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
		if ((UIEmoteImage != null) && (UIEmoteImage2 != null))
		{
			EmoteUICheck ();
		}

			if (playerNum == 0)
			{
				AkSoundEngine.SetSwitch ("PlayerID", "Player_1", gameObject);
		
				if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player1enteredBounds == false)
				{
					if (inputDevice.RightBumper.IsPressed)
					{

						chirpsButton = true;
						Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
						if (UIEmoteImage != null)
						{
							UIEmoteImage.transform.position = UIposition;
							UIEmoteImage.SetActive (true);
						}

						if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							Debug.Log ("player 1, left arm chirp");
							//example. 
							startTimer = true;
							eventCount += 1;
							AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

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
							AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);
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
							AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);
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
							AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadRight.WasPressed &&
						            !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							//duff
							Debug.Log ("player 1, No right leg");
						}
					} 
					else if((chirpsButton) && (UIEmoteImage != null))
					{	
						//set the emote UI to no be active unless the player is holding down the key
						UIEmoteImage.SetActive (false);
					}
				}

			}
			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (playerNum == 1)
			{
				AkSoundEngine.SetSwitch ("PlayerID", "Player_2", gameObject);

			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player2enteredBounds == false)
				{
				if (inputDevice.RightBumper.IsPressed)
					{
						chirpsButton = true;
						Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
						if (UIEmoteImage2 != null)
						{
							UIEmoteImage2.transform.position = UIposition;
							UIEmoteImage2.SetActive (true);
						}


						if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

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
							AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

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
							AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

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
							AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player2, right leg chirp");
						} else if (inputDevice.DPadRight.WasPressed &&
						           !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							Debug.Log ("player2, No right leg");
						}
					}
					else if((chirpsButton) && (UIEmoteImage2 != null))
					{	//set the emote UI to no be active unless the player is holding down the key
						if (UIEmoteImage2 != null)
						{
							UIEmoteImage2.SetActive (false);
						}
					}
				}

			}
		
	}
		

	void ProcessInput()
	{
		//if (MelodyDoor != null)
		//{

		if ((UIEmoteImage != null) && (UIEmoteImage2 != null))
		{
			EmoteUICheck ();
		}

			//player 1
			if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
			{
				//GameObject ui = !GameManager.Instance.whichAndroid.player1ControlBlue ? UIEmoteImage : UIEmoteImage2;

				AkSoundEngine.SetSwitch ("PlayerID", "Player_1", gameObject);

				if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player1enteredBounds == false)
				{

					if (Input.GetKey (GameManager.Instance.playerSetting.currentButton [12]))
					{
							chirpsButton = true;
							Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
							if (UIEmoteImage != null)
							{
								UIEmoteImage.transform.position = UIposition;
								UIEmoteImage.SetActive (true);
							}

							if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4])
							     && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
							{
								EmoteNumber = 1;
								Emotes ();
								//play chirp
								AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
								AkSoundEngine.PostEvent ("Chirp", gameObject);

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
								AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
								AkSoundEngine.PostEvent ("Chirp", gameObject);

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
								AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
								AkSoundEngine.PostEvent ("Chirp", gameObject);
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
								AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
								AkSoundEngine.PostEvent ("Chirp", gameObject);
								Debug.Log ("player 1, right leg chirp");
							} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
							            && !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
							{
								Debug.Log ("player 1, No right leg");
							}
					}
					else if((chirpsButton) && (UIEmoteImage != null))
					{	
						//set the emote UI to no be active unless the player is holding down the key
						UIEmoteImage.SetActive (false);
					}

				}// end of if your inside the melody door bounds
					
				

			}//player2 
		else
			{
				AkSoundEngine.SetSwitch ("PlayerID", "Player_2", gameObject);

				if (MelodyDoor.GetComponentInChildren<SCR_Door> ().Player2enteredBounds == false)
				{
					if (Input.GetKey (GameManager.Instance.playerSetting.currentButton [25]))
					{
						chirpsButton = true;
						Vector3 UIposition = Camera.main.WorldToScreenPoint (this.transform.position);
						if (UIEmoteImage2 != null)
						{
							UIEmoteImage2.transform.position = UIposition;
							UIEmoteImage2.SetActive (true);
						}


						
						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [17])
						      && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							AkSoundEngine.SetSwitch ("Chirp_Type", "Happy", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, left arm chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [17])
						             && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
						{
							Debug.Log ("player 2, no left arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18])
						      && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							AkSoundEngine.SetSwitch ("Chirp_Type", "Sad", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, right arm chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18])
						             && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
						{
							Debug.Log ("player 2, no right arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19])
						      && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							//play chirp
							AkSoundEngine.SetSwitch ("Chirp_Type", "Here", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, left leg chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19])
						             && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
						{
							Debug.Log ("player 2, no left leg");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [20])
						      && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							//play chirp
							AkSoundEngine.SetSwitch ("Chirp_Type", "There", gameObject);
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, right leg chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [20])
						             && !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
						{
							Debug.Log ("player 2, no right leg");
						}

					}
					else if((chirpsButton) && (UIEmoteImage2 != null))
					{	//set the emote UI to no be active unless the player is holding down the key
						if (UIEmoteImage2 != null)
						{
							UIEmoteImage2.SetActive (false);
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

		AkSoundEngine.SetRTPCValue ("Chirps_Combine", 1); 
	}

	void FalseEvent()
	{
		//if the timer reaches 0
		Debug.Log("false hope");
	}


	public void EmoteUICheck ()
	{
		if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
		{
			List<GameObject> ls = !GameManager.Instance.whichAndroid.player1ControlBlue ? EmoteUI : EmoteUI2;

			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Happy_Colour") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Happy_Gray") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Sad_Colour") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_Sad_Gray") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			}

			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookatme_Colour") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookatme_Gray") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookoverthere_Colour") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookoverthere_Gray") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			}

			//dpad image
			if (ControllersUsed == false)
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/1234") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/DPad") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			}


		} 
		else
		{ //player2

			List<GameObject> ls = GameManager.Instance.whichAndroid.player1ControlBlue ? EmoteUI : EmoteUI2;

			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [0].name.Contains ("LeftArm"))
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Happy_Colour") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Happy_Gray") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [1].name.Contains ("RightArm"))
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Sad_Colour") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Sad_Gray") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			}

			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [2].name.Contains ("LeftLeg"))
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookatme_Colour") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookatme_Gray") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().limbs [3].name.Contains ("RightLeg"))
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookoverthere_Colour") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/Chirp_UI_Lookoverthere_Gray") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			}

			//dpad image
			if (ControllersUsed == false)
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/5678") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/DPad") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			}
		}


	}



}