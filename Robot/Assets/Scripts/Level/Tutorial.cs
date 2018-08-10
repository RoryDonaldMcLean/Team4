using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class Tutorial : MonoBehaviour 
{
	GameObject UITutorial;
	GameObject UIText;
	List<GameObject> UIButtons = new List<GameObject>();
	bool scriptStart = false;

	int levelCounter;

	float timeLeft = 4.0f;
	bool startTimer;

	GameObject keyboardButtonPlayer1;
	GameObject KeyboardButtonPlayer2;

	int playerNum;
	bool ControllerUsed;

	public bool ChirpsTutorial;

	bool tempBool = false;

	// Use this for initialization
	void Start () 
	{
		playerNum = this.GetComponentInParent<InControlMovement> ().playerNum;
		keyboardButtonPlayer1 = GameObject.FindGameObjectWithTag ("KeyBoardPlayer1");
		KeyboardButtonPlayer2 = GameObject.FindGameObjectWithTag ("KeyBoardPlayer2");

		levelCounter = GameObject.FindGameObjectWithTag ("GameController").GetComponent<LevelController>().currentLevel;

		UITutorial = GameObject.FindGameObjectWithTag ("TutorialImage");
		if (UITutorial != null)
		{
			for (int i = 0; i < UITutorial.transform.childCount; i++)
			{
				UIButtons.Add (UITutorial.transform.GetChild (i).gameObject);
			}
		}

		UIText = GameObject.FindGameObjectWithTag ("TutorialText");

		scriptStart = true;
		ChirpsTutorial = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (scriptStart == true)
		{
			UITutorial.SetActive (false);
			scriptStart = false;
		}

		if (startTimer == true)
		{
			timeLeft -= Time.deltaTime;
		}

		if (timeLeft <= 0)
		{
			startTimer = false;
			timeLeft = 4.0f;
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
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

		if (this.GetComponentInParent<InControlMovement>().enabled == false)
		{
			ChirpsTutorialFunction();

			if (tempBool == false)
			{
				//UIText.GetComponent<Text> ().text = "Please perform Systems diagnostic...";
			}
            //CancelInvoke("func");
			//StartCoroutine (TestBool (1.0f));
		}
		else
		{
			//GetRidOfText ();
			//StartCoroutine(WaitTwoSeconds (2.0f));
            if(UIText != null)
            {
                UIText.GetComponent<Text>().text = "Diagnostic Complete. Movement re-activated";
                StartCoroutine(GetRidOfText(2.0f));

            }
			
		}


	}

	IEnumerator GetRidOfText(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);

		UIText.SetActive (false);
	}

	IEnumerator TestBool(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);

		tempBool = true;
	}


	void ChirpsTutorialFunction()
	{
		//at the beginning of the game, tutorial for the chirps intro
		if (ChirpsTutorial == true && levelCounter == 0)
		{
			if (ControllerUsed == true)
			{
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Open Chirp Menu") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (false);
				UIButtons [2].SetActive (false);
			} else
			{
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;
				UIButtons [1].SetActive (true);
				UIButtons [2].SetActive (true);

				keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [12].ToString ();
				KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [25].ToString ();
			}

		}
	}
		
	void OnTriggerEnter(Collider col)
	{
		if ((col.transform.name.Contains ("LeftArm") && levelCounter == 0)
		    || (col.transform.name.Contains ("RightArm") && levelCounter == 0))
		{
			if (ControllerUsed == true)
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick Limb") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (false);
				UIButtons [2].SetActive (false);
			}
			else
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (true);
				UIButtons [2].SetActive (true);
				//test to see if i can display the keyboard controls
				keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [9].ToString ();
				KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [22].ToString ();

			}

		} 
		else if (col.transform.name.Contains ("LightEmitter") && levelCounter == 0)
		{
			if (ControllerUsed == true)
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Turn ONOff Light Emitter") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (false);
				UIButtons [2].SetActive (false);
			}
			else
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (true);
				UIButtons [2].SetActive (true);
				//test to see if i can display the keyboard controls
				keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [11].ToString ();
				KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [24].ToString ();
			}
		} 
		else if (col.transform.name.Contains ("SlideBox") && levelCounter == 1)
		{
			if (ControllerUsed == true)
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick-Drop Object") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (false);
				UIButtons [2].SetActive (false);
			} 
			else
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (true);
				UIButtons [2].SetActive (true);
				//test to see if i can display the keyboard controls
				keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [9].ToString ();
				KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [22].ToString ();
			}


		} 
		else if (col.transform.name.Contains ("RotateBox") && levelCounter == 1)
		{
			if (ControllerUsed == true)
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick-Drop Object") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;
				UIButtons [1].SetActive (false);
				UIButtons [2].SetActive (false);
			} 
			else
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (true);
				UIButtons [2].SetActive (true);
				//test to see if i can display the keyboard controls
				keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [9].ToString ();
				KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [22].ToString ();
			}


		} 
		else if (col.transform.name.Contains ("LimbLight(ARM_MOVE)"))
		{
			if (col.transform.GetChild (0).name.Contains ("Arm") && levelCounter == 1)
			{
				if (ControllerUsed == true)
				{
					startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick-Drop Object") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [2].SetActive (false);
				}
				else
				{
					startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					UIButtons [2].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [9].ToString ();
					KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [22].ToString ();
				}

			} 
			else
			{
				if (ControllerUsed == true)
				{
					startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Insert Withdraw Limb") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;
					UIButtons [1].SetActive (false);
					UIButtons [2].SetActive (false);
				}
				else
				{
					startTimer = true;
					UITutorial.SetActive (true);
					UIButtons [0].SetActive (true);
					UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
					UIButtons [0].GetComponent<Image> ().preserveAspect = true;

					UIButtons [1].SetActive (true);
					UIButtons [2].SetActive (true);
					//test to see if i can display the keyboard controls
					keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [11].ToString ();
					KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [24].ToString ();
				}

			}

		} 
		else if (col.transform.name.Contains ("Elevation") && levelCounter == 0)
		{
			if (ControllerUsed == true)
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Jump") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (false);
				UIButtons [2].SetActive (false);
			}
			else
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Keyboard promptbox Buttons") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;

				UIButtons [1].SetActive (true);
				UIButtons [2].SetActive (true);
				//test to see if i can display the keyboard controls
				keyboardButtonPlayer1.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [8].ToString ();
				KeyboardButtonPlayer2.GetComponent<Text> ().text = GameManager.Instance.playerSetting.currentButton [21].ToString ();
			}

		}

	}


	void OnTriggerExit(Collider col)
	{
		if ((col.transform.name.Contains ("LeftArm"))
		    || (col.transform.name.Contains ("RightArm")))
		{
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("LightEmitter"))
		{
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("SlideBox"))
		{
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("RotateBox"))
		{
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("LimbLight(ARM_MOVE)"))
		{
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("Elevation"))
		{
			UITutorial.SetActive (false);
			UIButtons [0].SetActive (false);
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		}
	}
}