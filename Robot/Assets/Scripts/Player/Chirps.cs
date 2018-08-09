﻿using System.Collections;
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

	bool chirpsButton = false;

	GameObject MelodyDoor;

	GameObject UIEmoteImage;
	List<GameObject> EmoteUI = new List<GameObject> ();

	GameObject UIEmoteImage2;
	List<GameObject> EmoteUI2 = new List<GameObject>();


	bool scriptStart = false;
	bool ControllersUsed;

	GameObject GameController;

	// Use this for initialization
	private void Start () 
	{
		MelodyDoor = GameObject.FindGameObjectWithTag ("Doors");
		Face = GameObject.FindGameObjectWithTag ("Player1");
		Face2 = GameObject.FindGameObjectWithTag ("Player2");

		GameController = GameObject.FindGameObjectWithTag ("GameController");

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

					if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							//play chirp
							Debug.Log ("player 1, left arm chirp");
							//example. 
							GameController.GetComponent<ChirpCollector> ().startTimer = true;
                            GameController.GetComponent<ChirpCollector>().playertwoChirped = true;

                            //eventCount += 1;
                            AkSoundEngine.SetState ("Chirp_Type", "Happy");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

						} else if (inputDevice.DPadUp.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
						{
							//duff chirp
							Debug.Log ("player 1, No Left Arm");
						}



					if (inputDevice.DPadDown.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							GameController.GetComponent<ChirpCollector> ().startTimer = true;
                        GameController.GetComponent<ChirpCollector>().playertwoChirped = true;
                        Debug.Log ("player 1, right arm chirp");
							AkSoundEngine.SetState ("Chirp_Type", "Sad");
							AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadDown.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
						{
							//duff
							Debug.Log ("Player1, No right arm");
						}



					if (inputDevice.DPadLeft.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							//GameController.GetComponent<ChirpCollector> ().startTimer = true;
							//GameController.GetComponent<ChirpCollector> ().eventCount += 1;
							Debug.Log ("player1, left leg chirp");
							AkSoundEngine.SetState ("Chirp_Type", "Here");
							AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadLeft.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
						{
							//duff
							Debug.Log ("player1, No left leg");
						}



					if (inputDevice.DPadRight.WasPressed && GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							//GameController.GetComponent<ChirpCollector> ().startTimer = true;
							//GameController.GetComponent<ChirpCollector> ().eventCount += 1;
							Debug.Log ("player1, right leg chirp");
							AkSoundEngine.SetState ("Chirp_Type", "There");
							AkSoundEngine.PostEvent ("Chirp", gameObject);
						} else if (inputDevice.DPadRight.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
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


					if (inputDevice.DPadUp.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();
							GameController.GetComponent<ChirpCollector> ().startTimer = true;
                            GameController.GetComponent<ChirpCollector>().playertwoChirped = true;
                            //play chirp
                            AkSoundEngine.SetState ("Chirp_Type", "Happy");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player 2, left Arm Chirp");
						} else if (inputDevice.DPadUp.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
						{
							Debug.Log ("player2, NO left arm");
						}



					if (inputDevice.DPadDown.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();
							//play chirp
							GameController.GetComponent<ChirpCollector> ().startTimer = true;
                            GameController.GetComponent<ChirpCollector>().playertwoChirped = true;
                            AkSoundEngine.SetState ("Chirp_Type", "Sad");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player 2, right arm chirp");
						} else if (inputDevice.DPadDown.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
						{
							Debug.Log ("Player2, No right arm");
						}



					if (inputDevice.DPadLeft.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();
							//GameController.GetComponent<ChirpCollector> ().startTimer = true;
							//GameController.GetComponent<ChirpCollector> ().eventCount += 1;
							//play chirp
							AkSoundEngine.SetState ("Chirp_Type", "Here");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player2, left leg chirp");
						} else if (inputDevice.DPadLeft.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
						{
							Debug.Log ("player2, No left leg");
						}



					if (inputDevice.DPadRight.WasPressed && GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();
							//GameController.GetComponent<ChirpCollector> ().startTimer = true;
							//GameController.GetComponent<ChirpCollector> ().eventCount += 1;
							//play chirp
							AkSoundEngine.SetState ("Chirp_Type", "There");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("player2, right leg chirp");
						} else if (inputDevice.DPadRight.WasPressed &&
						!GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
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
						&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
							{
								EmoteNumber = 1;
								Emotes ();
								GameController.GetComponent<ChirpCollector> ().startTimer = true;
                                GameController.GetComponent<ChirpCollector>().playeroneChirped = true;
                        
                        //play chirp
                        AkSoundEngine.SetState ("Chirp_Type", "Happy");
								AkSoundEngine.PostEvent ("Chirp", gameObject);

								Debug.Log ("player 1, left arm chirp");
							} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [4])
						&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
							{
								Debug.Log ("player 1, No Left Arm");
							}



							if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5])
						&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
							{
								EmoteNumber = 2;
								Emotes ();
								GameController.GetComponent<ChirpCollector> ().startTimer = true;
                                GameController.GetComponent<ChirpCollector>().playeroneChirped = true;
								//play chirp
								AkSoundEngine.SetState ("Chirp_Type", "Sad");
								AkSoundEngine.PostEvent ("Chirp", gameObject);

								Debug.Log ("player 1, right arm chirp");
							} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [5])
						&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
							{
								Debug.Log ("player 1, No right Arm");
							}



							if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6])
						&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
							{
								EmoteNumber = 3;
								Emotes ();
								//GameController.GetComponent<ChirpCollector> ().startTimer = true;
                               // GameController.GetComponent<ChirpCollector>().playeroneChirped = true;
                        
                                //play chirp
                                AkSoundEngine.SetState ("Chirp_Type", "Here");
								AkSoundEngine.PostEvent ("Chirp", gameObject);
								Debug.Log ("player 1, left leg chirp");

							} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [6])
						&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
							{
								Debug.Log ("player 1, No Left leg");
							}



							if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
						&& GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
							{
								EmoteNumber = 4;
								Emotes ();
								//GameController.GetComponent<ChirpCollector> ().startTimer = true;
                               // GameController.GetComponent<ChirpCollector>().playeroneChirped = true;
                        
                                //play chirp
                                AkSoundEngine.SetState ("Chirp_Type", "There");
								AkSoundEngine.PostEvent ("Chirp", gameObject);
								Debug.Log ("player 1, right leg chirp");
							}

                            else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [7])
						&& !GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
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
					
				

			}
            
            //player2 
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
						&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
						{
							EmoteNumber = 1;
							Emotes ();

							GameController.GetComponent<ChirpCollector> ().startTimer = true;
                            GameController.GetComponent<ChirpCollector>().playertwoChirped = true;
                        //play chirp
                        AkSoundEngine.SetState ("Chirp_Type", "Happy");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, left arm chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [17])
						&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
						{
							Debug.Log ("player 2, no left arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18])
						&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
						{
							EmoteNumber = 2;
							Emotes ();

							GameController.GetComponent<ChirpCollector> ().startTimer = true;
                            GameController.GetComponent<ChirpCollector>().playertwoChirped = true;
                            //play chirp
                            AkSoundEngine.SetState ("Chirp_Type", "Sad");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, right arm chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [18])
						&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
						{
							Debug.Log ("player 2, no right arm");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19])
						&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
						{
							EmoteNumber = 3;
							Emotes ();

							//GameController.GetComponent<ChirpCollector> ().startTimer = true;
							//GameController.GetComponent<ChirpCollector> ().eventCount += 1;
							//play chirp
							AkSoundEngine.SetState ("Chirp_Type", "Here");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, left leg chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [19])
						&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
						{
							Debug.Log ("player 2, no left leg");
						}



						if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [20])
						&& GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
						{
							EmoteNumber = 4;
							Emotes ();

							//GameController.GetComponent<ChirpCollector> ().startTimer = true;
							//GameController.GetComponent<ChirpCollector> ().eventCount += 1;
							//play chirp
							AkSoundEngine.SetState ("Chirp_Type", "There");
							AkSoundEngine.PostEvent ("Chirp", gameObject);

							Debug.Log ("Player2, right leg chirp");	
						} else if (Input.GetKeyDown (GameManager.Instance.playerSetting.currentButton [20])
						&& !GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
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


	public void EmoteUICheck ()
	{
		if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
		{
			List<GameObject> ls = !GameManager.Instance.whichAndroid.player1ControlBlue ? EmoteUI : EmoteUI2;

			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Happy_StateActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Happy_StateNonActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Sad_StateActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Sad_StateNonActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			}

			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookAtMe_StateActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookAtMe_StateNonActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookOverThere_StateActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookOverThere_StateNonActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			}

			//dpad image
			if (ControllersUsed == false)
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - 1234") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - Unedited") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			}


		} 
		else
		{ //player2

			List<GameObject> ls = GameManager.Instance.whichAndroid.player1ControlBlue ? EmoteUI : EmoteUI2;

			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftArm"))
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Happy_StateActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [0].SetActive (true);
				ls [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Happy_StateNonActive") as Sprite;
				ls [0].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightArm"))
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Sad_StateActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [1].SetActive (true);
				ls [1].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_Sad_StateNonActive") as Sprite;
				ls [1].GetComponent<Image> ().preserveAspect = true;
			}

			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("LeftLeg"))
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookAtMe_StateActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [2].SetActive (true);
				ls [2].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookAtMe_StateNonActive") as Sprite;
				ls [2].GetComponent<Image> ().preserveAspect = true;
			}


			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<SCR_TradeLimb> ().LimbActiveCheck  ("RightLeg"))
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookOverThere_StateActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			} else
			{
				ls [3].SetActive (true);
				ls [3].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/Emotes/UIChirpWheel_LookOverThere_StateNonActive") as Sprite;
				ls [3].GetComponent<Image> ().preserveAspect = true;
			}

			//dpad image
			if (ControllersUsed == false)
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - 5678") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				ls [4].SetActive (true);
				ls [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - Unedited") as Sprite;
				ls [4].GetComponent<Image> ().preserveAspect = true;
			}
		}


	}



}