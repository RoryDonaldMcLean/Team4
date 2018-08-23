using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

//THIS SCRIPT IS TRAAAAAAAAAAAAAAAAAAAAAAAAAAASH
public class Tutorial : MonoBehaviour 
{
	GameObject UITutorial;
	GameObject UITutorial2;

	GameObject UIText;
	List<GameObject> UIButtons = new List<GameObject>();
	List<GameObject> UIButtons2 = new List<GameObject> ();

	bool scriptStart = false;

	int levelCounter;

	//float timeLeft = 4.0f;
	//bool startTimer;

	GameObject keyboardButtonPlayer1;
	GameObject KeyboardButtonPlayer2;

	int playerNum;
	bool ControllerUsed;
	bool tempBool = false;

	public bool ChirpsTutorial;

	bool UISwitch = false;

	GameObject LeftArm, RightArm;
	bool RightArmOn = true;

	GameObject NarrativeCanvas;

	// Use this for initialization
	void Start () 
	{
		NarrativeCanvas = GameObject.Find ("CanvasNarrative");

		LeftArm = GameObject.Find ("LeftArm");
		RightArm = GameObject.Find ("RightArm");

		playerNum = this.GetComponentInParent<InControlMovement> ().playerNum;
		keyboardButtonPlayer1 = GameObject.FindGameObjectWithTag ("KeyBoardPlayer1");
		KeyboardButtonPlayer2 = GameObject.FindGameObjectWithTag ("KeyBoardPlayer2");

		levelCounter = GameObject.FindGameObjectWithTag ("GameController").GetComponent<LevelController>().currentLevel;

		//PLayer 1 UI
		UITutorial = GameObject.FindGameObjectWithTag ("TutorialImage");
		if (UITutorial != null)
		{
			for (int i = 0; i < UITutorial.transform.childCount; i++)
			{
				UIButtons.Add (UITutorial.transform.GetChild (i).gameObject);
			}
		}

		//player 2 UI
		UITutorial2 = GameObject.FindGameObjectWithTag ("TutorialImage2");
		if (UITutorial2 != null)
		{
			for (int i = 0; i < UITutorial2.transform.childCount; i++)
			{
				UIButtons2.Add (UITutorial2.transform.GetChild (i).gameObject);
			}
		}


		UIText = GameObject.FindGameObjectWithTag ("TutorialText");

		scriptStart = true;
		ChirpsTutorial = true;
	}

	public void PlayerOneToggleUI()
	{
		bool UIState = !UITutorial.activeSelf;
		UITutorial.SetActive (UIState);
		UIButtons [0].SetActive (UIState);
		UIButtons [0].GetComponent<Image> ().preserveAspect = true;

	}

	public void PlayerTwoToggleUI()
	{
		bool UIState = !UITutorial2.activeSelf;
		UITutorial2.SetActive (UIState);
		UIButtons2 [0].SetActive (UIState);
		UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
	}

	// Update is called once per frame
	void Update () 
	{
		if (scriptStart == true)
		{
			UITutorial.SetActive (false);
			UITutorial2.SetActive (false);
			scriptStart = false;
		}

		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//Debug.Log ("no controllers plugged in");
			ControllerUsed = false;
		} 
		else
		{
			ControllerUsed = true;
		}

		InitialText();
		 


	}

	void InitialText()
	{
		if(levelCounter == 0 && NarrativeCanvas.GetComponent<NarrativeText>().TextDone == true)
		{
			if(UIText != null)
			{
				if (UISwitch == false)
				{
					UIText.GetComponent<Text> ().text = "Diagnostic Complete. Movement re-activated";
					StartCoroutine (GetRidOfText (2.0f));
				}

			}

		}

	}

	IEnumerator GetRidOfText(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		UIText.SetActive (false);
		UISwitch = true;
	}

	IEnumerator TestBool(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);

		tempBool = true;
	}

	IEnumerator DisplayText(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		UIText.SetActive (true);
		UIText.GetComponent<Text> ().text = "Found operational arm. Directive; acquite arm.";
	}



	void OnTriggerEnter(Collider col)
	{
		Debug.Log ("you are hitting " + col);
		if ((col.transform.name.Contains ("LeftArm"))
		    || (col.transform.name.Contains ("RightArm")))
		{
			if (ControllerUsed == true)
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Found operational arm. Directive; acquire arm.";
					StartCoroutine (GetRidOfText (3.0f));
				}

				if (playerNum == 0)
				{
					PlayerOneToggleUI ();

					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickLimb_Circle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickLimb_B_XBOX") as Sprite;
					}


					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					PlayerTwoToggleUI ();
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickLimb_Circle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickLimb_B_XBOX") as Sprite;
					}

					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}

			} else
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Found operational arm. Directive; acquite arm.";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if (playerNum == 0)
				{
					PlayerOneToggleUI ();
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Pick Up Limb";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					PlayerTwoToggleUI ();
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Pick Up Limb";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}



			}

		} else if (col.transform.name.Contains ("LightEmitter"))
		{
			if (ControllerUsed == true)
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Industrial equipment discovered. Initiate interact function";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if ((playerNum == 0) && (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true))
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Triangle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Y_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);


				} 

				if ((playerNum == 1) && (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true))
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Triangle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Y_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}

			} else
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Industrial equipment discovered. Initiate interact function";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if ((playerNum == 0) && (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true))
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Turn On Emitter";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				
				}

				if ((playerNum == 1) && (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true))
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Turn On Emitter";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			}
		} else if (col.transform.name.Contains ("SlideBox"))
		{
			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}

			} else
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Pick Up Slider";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Pick Up Slider";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			}


		} else if (col.transform.name.Contains ("RotateBox"))
		{
			if (ControllerUsed == true)
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Industrial equipment requires rotation to achieve maximum efficiency";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [2].SetActive (false);


					if (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true)
					{
						UIButtons [4].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Triangle_PS") as Sprite;
						} else
						{
							UIButtons [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Y_XBOX") as Sprite;
						}
					} else
					{
						UIButtons [4].SetActive (false);
					}

				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						Debug.Log ("hehehehehehehe");
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons2 [1].SetActive (false);
					UIButtons2 [2].SetActive (false);

					if (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true)
					{
						UIButtons2 [4].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Triangle_PS") as Sprite;
						} else
						{
							UIButtons2 [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Y_XBOX") as Sprite;
						}
					} else
					{
						UIButtons2 [4].SetActive (false);
					}
				}


			} else
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Industrial equipment requires rotation to achieve maximum efficiency";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();

					UIButtons [2].GetComponent<Text> ().text = "Pick Up Rotatable";


					if (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true)
					{
						UIButtons [3].SetActive (true);
						UIButtons [3].GetComponent<Text> ().text = "Turn on Emitter " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
					} else
					{
						UIButtons [3].SetActive (false);
					}
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					UIButtons2 [2].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Pick Up Rotatable";

					if (col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true)
					{
						UIButtons2 [3].SetActive (true);
						UIButtons2 [3].GetComponent<Text> ().text = "Turn on Emitter " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
					} else
					{
						UIButtons2 [3].SetActive (false);
					}
					UIButtons2 [4].SetActive (false);
				}



			}


		} else if ((col.transform.name.Contains ("LimbLight(ARM_MOVE)")))
		{
			if (levelCounter == 1)
			{
				UIText.SetActive (true);
				UIText.GetComponent<Text> ().text = "Reflection equipment has sustained damage: Insert Limb to re-activate functionality";
				StartCoroutine (GetRidOfText (3.0f));
			}


			if (col.transform.GetChild (0).name.Contains ("Arm") && col.transform.tag != "Untagged")
			{
				if (ControllerUsed == true)
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons [3].SetActive (false);
						//UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				}

			} else
			{
				if (ControllerUsed == true)
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}



				}

			}

		} else if (col.transform.name.Contains ("Elevation"))
		{
			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Jump_Cross_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Jump_A_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (false);
					//UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
					//UIButtons [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Turn ONOff Light Emitter") as Sprite;

				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Jump_Cross_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Jump_A_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (false);
					UIButtons2 [4].SetActive (false);
					//UIButtons2 [4].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Turn ONOff Light Emitter") as Sprite;
				}


			} else
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [8].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Jump";
					UIButtons [4].SetActive (false);
					//UIButtons [3].GetComponent<Text> ().text = "Turn on Emitter " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [21].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Jump";
					UIButtons2 [4].SetActive (false);
					//UIButtons2 [3].GetComponent<Text> ().text = "Turn on Emitter " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
				}



			}

		} else if (col.transform.name.Contains ("MelodyGate"))
		{
			if (ControllerUsed == true)
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Sound based password door discovered. Attempt override...";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (false);
				}
			} else
			{
				if (levelCounter == 0)
				{
					UIText.SetActive (true);
					UIText.GetComponent<Text> ().text = "Sound based password door discovered. Attempt override...";
					StartCoroutine (GetRidOfText (3.0f));

				}

				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (false);
				}
			}
		} else if (col.transform.name.Contains ("LimbLight(LEG_MOVE)"))
		{
			if (levelCounter == 2)
			{
				UIText.SetActive (true);
				UIText.GetComponent<Text> ().text = "Reflection equipment has sustained damage: Insert Limb to re-activate functionality";
				StartCoroutine (GetRidOfText (3.0f));
			}

			if (col.transform.GetChild (0).name.Contains ("Leg") && col.transform.tag != "Untagged")
			{
				if (ControllerUsed == true)
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons [3].SetActive (false);
						//UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				}

			} else
			{
				if (ControllerUsed == true)
				{
					if (levelCounter == 2)
					{
						UIText.SetActive (true);
						UIText.GetComponent<Text> ().text = "New equipment sighted accessing drive...Equipment function is to change the spectrum of beam, requires limb to function.";
						StartCoroutine (GetRidOfText (3.0f));

					}

					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (levelCounter == 2)
					{
						UIText.SetActive (true);
						UIText.GetComponent<Text> ().text = "New equipment sighted accessing drive...Equipment function is to change the spectrum of beam, requires limb to function.";
						StartCoroutine (GetRidOfText (3.0f));

					}

					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}



				}

			}
		} else if (col.transform.name.Contains ("LightSplitter"))
		{
			if (levelCounter == 3)
			{
				UIText.SetActive (true);
				UIText.GetComponent<Text> ().text = "New equipment sighted: Function: splits the neutron beam into two different beams (can be picked up)";
				StartCoroutine (GetRidOfText (3.0f));

			}

			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			} else
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Pick up device";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			}



		} else if (col.transform.name.Contains ("LightBarrier"))
		{
			if (levelCounter == 3)
			{
				UIText.SetActive (true);
				UIText.GetComponent<Text> ().text = "New equipment sighted: Function: Prevents neutron beam of a specific spectrum from passing through barrier.";
				StartCoroutine (GetRidOfText (3.0f));

			}
		} else if (col.transform.name.Contains ("LightColourCombo"))
		{
			if (levelCounter == 3)
			{
				UIText.SetActive (true);
				UIText.GetComponent<Text> ().text = "New equipment sighted: Function: Combines two different neutron beams to create a new single beam of a different spectrum (can be picked up)";
				StartCoroutine (GetRidOfText (3.0f));

			}

			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			} else
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Pick up device";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			}

		} else if (col.transform.name.Contains ("LightRedirect"))
		{
			if (levelCounter == 1 && col.transform.tag != "Untagged")
			{
				UIText.SetActive (true);
				UIText.GetComponent<Text> ().text = "New equipment sighted: LightRedirect: Place in-frount of light beam to redirect light. Requires Two Arms";
				StartCoroutine (GetRidOfText (3.0f));
			}

			if (ControllerUsed == true)
			{
				if (playerNum == 0 && col.transform.tag != "Untagged")
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1 && col.transform.tag != "Untagged")
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}

			} else
			{
				if (playerNum == 0 && col.transform.tag != "Untagged")
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Pick up device";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1 && col.transform.tag != "Untagged")
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}
			}
		} else if (col.gameObject.name.Contains ("Switch"))
		{
			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Triangle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Y_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);


				} 

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Triangle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_Turn OnOff Light Emitter_Y_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}

			} else
			{
				

				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Turn On Switch";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);

				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Turn On Switch";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}


			}
		}
		else if (col.gameObject.name.Contains ("Cube"))
		{
			Debug.Log ("hit control panel");
			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_UseControlPanel_Triangle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_UseControlPanel_Y_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_UseControlPanel_Triangle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_UseControlPanel_Y_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}
			} 
			else
			{
				if (playerNum == 0)
				{
					Debug.Log ("hello there");
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Activate Device";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Activate Device";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}
			}
		}
		else if (col.gameObject.name.Contains ("TubeMachine_Geo"))
		{
			Debug.Log ("hit the tube");
			if (ControllerUsed == true)
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_EnterSoulMachine_Triangle_PS") as Sprite;
					} else
					{
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_EnterSoulMachine_Y_XBOX") as Sprite;
					}
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					//startTimer = true;
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_EnterSoulMachine_Triangle_PS") as Sprite;
					} else
					{
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_EnterSoulMachine_Y_XBOX") as Sprite;
					}
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons2 [1].SetActive (false);
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}
			}
			else
			{
				if (playerNum == 0)
				{
					//startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
					UIButtons [2].GetComponent<Text> ().text = "Enter Device";
					UIButtons [3].SetActive (false);
					UIButtons [4].SetActive (false);
				}

				if (playerNum == 1)
				{
					UITutorial2.SetActive (true);
					UIButtons2 [0].SetActive (true);
					UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
					UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons2 [1].SetActive (true);
					//test to see if i can display the keyboard controls

					KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
					UIButtons2 [2].GetComponent<Text> ().text = "Enter Device";
					UIButtons2 [3].SetActive (false);
					UIButtons2 [4].SetActive (false);
				}
			}
		}


	}

	void OnTriggerStay(Collider col)
	{
		if (col.transform.name.Contains ("LimbLight(LEG_MOVE)"))
		{
			if (col.transform.GetChild (0).name.Contains ("Leg") && col.transform.tag != "Untagged")
			{
				if (ControllerUsed == true)
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons [3].SetActive (false);
						//UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				}

			} else
			{
				if (ControllerUsed == true)
				{
					if (levelCounter == 2)
					{
						UIText.SetActive (true);
						UIText.GetComponent<Text> ().text = "New equipment sighted accessing drive...Equipment function is to change the spectrum of beam, requires limb to function.";
						StartCoroutine (GetRidOfText (3.0f));

					}

					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (levelCounter == 2)
					{
						UIText.SetActive (true);
						UIText.GetComponent<Text> ().text = "New equipment sighted accessing drive...Equipment function is to change the spectrum of beam, requires limb to function.";
						StartCoroutine (GetRidOfText (3.0f));

					}

					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}



				}

			}


		}

		if (col.transform.name.Contains ("LimbLight(ARM_MOVE)"))
		{
			if (col.transform.GetChild (0).name.Contains ("Arm") && col.transform.tag != "Untagged")
			{
				if (ControllerUsed == true)
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_Circle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_PickDropObject_B_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [9].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons [3].SetActive (false);
						//UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [22].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Pick up device";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				}

			} else
			{
				if (ControllerUsed == true)
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons [1].SetActive (false);
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						if (this.GetComponentInParent<InControlMovement> ().UsingPlayStation == true)
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Triangle_PS") as Sprite;
						} else
						{
							UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Individual/White_InsertWithdrawLimb_Y_XBOX") as Sprite;
						}
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;
						UIButtons2 [1].SetActive (false);
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}


				} else
				{
					if (playerNum == 0)
					{
						//startTimer = true;
						UITutorial.SetActive (true);
						UIButtons [0].SetActive (true);
						UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
						UIButtons [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons [1].SetActive (true);
						//test to see if i can display the keyboard controls
						keyboardButtonPlayer1.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [11].ToString ();
						UIButtons [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons [3].SetActive (false);
						UIButtons [4].SetActive (false);
					}

					if (playerNum == 1)
					{
						//startTimer = true;
						UITutorial2.SetActive (true);
						UIButtons2 [0].SetActive (true);
						UIButtons2 [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
						UIButtons2 [0].GetComponent<Image> ().preserveAspect = true;

						UIButtons2 [1].SetActive (true);
						//test to see if i can display the keyboard controls

						KeyboardButtonPlayer2.GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [24].ToString ();
						UIButtons2 [2].GetComponent<Text> ().text = "Insert/ withdraw Limb";
						UIButtons2 [3].SetActive (false);
						UIButtons2 [4].SetActive (false);
					}



				}

			}
		}



	}

	void OnTriggerExit(Collider col)
	{
		if ((col.transform.name.Contains ("LeftArm"))
		    || (col.transform.name.Contains ("RightArm")))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}

		} else if (col.transform.name.Contains ("LightEmitter"))
		{
			if (playerNum == 0 && col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1 && col.transform.GetComponent<LightEmitter> ().canBeTurnedOff == true)
			{
				PlayerTwoToggleUI ();
			}

		} else if (col.transform.name.Contains ("SlideBox"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}

		} else if (col.transform.name.Contains ("RotateBox"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}


		} else if (col.transform.name.Contains ("LimbLight(ARM_MOVE)"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}


		} else if (col.transform.name.Contains ("Elevation"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}


		} else if (col.transform.name.Contains ("LimbLight(LEG_MOVE)"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}

		} else if (col.transform.name.Contains ("LightSplitter"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}
		} else if (col.transform.name.Contains ("LightBarrier"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}
		} else if (col.transform.name.Contains ("LightColourCombo"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}
		} else if (col.transform.name.Contains ("LightRedirect"))
		{
			if (playerNum == 0 && col.transform.tag != "Untagged")
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1 && col.transform.tag != "Untagged")
			{
				PlayerTwoToggleUI ();
			}
		} else if (col.transform.name.Contains ("Switch"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}
		} else if (col.transform.name.Contains ("TubeMachine_Geo"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}
		} else if (col.transform.name.Contains ("Cube"))
		{
			if (playerNum == 0)
			{
				PlayerOneToggleUI ();
			}

			if (playerNum == 1)
			{
				PlayerTwoToggleUI ();
			}
		}
			
	}
		
}