using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour 
{
	GameObject UITutorial;
	List<GameObject> UIButtons = new List<GameObject>();
	bool scriptStart = false;

	int levelCounter;

	float timeLeft = 4.0f;
	bool startTimer;

	// Use this for initialization
	void Start () 
	{
		levelCounter = GameObject.FindGameObjectWithTag ("GameController").GetComponent<LevelController>().currentLevel;

		UITutorial = GameObject.FindGameObjectWithTag ("TutorialImage");
		if (UITutorial != null)
		{
			for (int i = 0; i < UITutorial.transform.childCount; i++)
			{
				UIButtons.Add (UITutorial.transform.GetChild (i).gameObject);
			}
		}

		scriptStart = true;

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
			
	}


	void OnTriggerEnter(Collider col)
	{
		if ((col.transform.name.Contains ("LeftArm") && levelCounter == 0)
		    || (col.transform.name.Contains ("RightArm") && levelCounter == 0))
		{
			startTimer = true;
			UITutorial.SetActive (true);
			UIButtons [0].SetActive (true);
			UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick Limb") as Sprite;
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("LightEmitter") && levelCounter == 0)
		{
			startTimer = true;
			UITutorial.SetActive (true);
			UIButtons [0].SetActive (true);
			UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Turn ONOff Light Emitter") as Sprite;
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("SlideBox") && levelCounter == 1)
		{
			startTimer = true;
			UITutorial.SetActive (true);
			UIButtons [0].SetActive (true);
			UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick-Drop") as Sprite;
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("RotateBox") && levelCounter == 1)
		{
			startTimer = true;
			UITutorial.SetActive (true);
			UIButtons [0].SetActive (true);
			UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick-Drop") as Sprite;
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
		} 
		else if (col.transform.name.Contains ("LimbLight(ARM_MOVE)"))
		{
			if (col.transform.GetChild (0).name.Contains ("Arm") && levelCounter == 1)
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Pick-Drop") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;
			} 
			else
			{
				startTimer = true;
				UITutorial.SetActive (true);
				UIButtons [0].SetActive (true);
				UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Insert Withdraw Limb") as Sprite;
				UIButtons [0].GetComponent<Image> ().preserveAspect = true;
			}

		} 
		else if (col.transform.name.Contains ("Elevation") && levelCounter == 0)
		{
			startTimer = true;
			UITutorial.SetActive (true);
			UIButtons [0].SetActive (true);
			UIButtons [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/Controller_Jump") as Sprite;
			UIButtons [0].GetComponent<Image> ().preserveAspect = true;
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
