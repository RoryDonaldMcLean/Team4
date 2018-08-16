using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class InControlMelody : MonoBehaviour 
{
	//is there 2 players in the game. if so use different controls for player 1 and 2 
	//but allows it all to be in 1 script
	AudioClip[] Chirps;

	//The code the robots use to compare to the door code
	public List<int> Robotcode = new List<int> ();

	//this is the bool check for if the robotCode is the same as the door code
	bool test = true;
	public bool correctCode = false;

	//public GameObject Canvas;
	GameObject CanvasNoteSheet;
	GameObject RedArrows;
	List<GameObject> RedDPAD = new List<GameObject> ();

	GameObject BlueArrows;
	List<GameObject> BlueDPAD = new List<GameObject> ();

	//counter used to track which note you are inputting i.e.
	//the first input, the second etc
	public int noteCounter = 0;

	//array of arrow textures that will be applied to the rawimages when you input a note
	Sprite[] Arrows;

	//array of rawimages that are being used in the UI for the melodies
	List<GameObject> Notes = new List<GameObject>();

	int playerNum =0;

	GameObject RedCancel;
	List<GameObject> RedCancelChildren = new List<GameObject>();

	GameObject BlueCancel;
	List<GameObject> BlueCancelChildren = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
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


		RedArrows = GameObject.FindGameObjectWithTag ("RedArrows");
		for (int i = 0; i < RedArrows.transform.childCount; i++)
		{
			RedDPAD.Add (RedArrows.transform.GetChild (i).gameObject);
		}


		BlueArrows = GameObject.FindGameObjectWithTag ("BlueArrows");
		for (int i = 0; i < BlueArrows.transform.childCount; i++)
		{
			BlueDPAD.Add(BlueArrows.transform.GetChild(i).gameObject);
		}


		RedCancel = GameObject.FindGameObjectWithTag ("RedCancel");
		for (int i = 0; i < RedCancel.transform.childCount; i++)
		{
			RedCancelChildren.Add (RedCancel.transform.GetChild (i).gameObject);
		}

		BlueCancel = GameObject.FindGameObjectWithTag ("BlueCancel");
		for (int i = 0; i < BlueCancel.transform.childCount; i++)
		{
			BlueCancelChildren.Add (BlueCancel.transform.GetChild (i).gameObject);
		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices [playerNum] : null;
		if (inputDevice == null)
		{
			//no controllers
			RedDPAD [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - 1234") as Sprite;
			BlueDPAD [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - 5678") as Sprite;

			RedCancelChildren [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Red") as Sprite;
			RedCancelChildren [1].SetActive (true);
			RedCancelChildren[1].GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [26].ToString ();
			RedCancelChildren [2].SetActive (true);

			BlueCancelChildren [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/UITutorialBox_Blue") as Sprite;
			BlueCancelChildren [1].SetActive (true);
			BlueCancelChildren[1].GetComponent<Text> ().text = "Press: " + GameManager.Instance.playerSetting.currentButton [27].ToString ();
			BlueCancelChildren [2].SetActive (true);
		} 
		else
		{
			RedDPAD [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - Unedited") as Sprite;
			BlueDPAD [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/LimbsMenu/UIDPadIcon - Unedited") as Sprite;

			RedCancelChildren [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/White_Controller_Cancel Melody Code") as Sprite;
			RedCancelChildren [1].SetActive (false);
			RedCancelChildren [2].SetActive (false);

			BlueCancelChildren [0].GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Art/UI/TutorialCue/NewCues/White/White_Controller_Cancel Melody Code") as Sprite;
			BlueCancelChildren [1].SetActive (false);
			BlueCancelChildren [2].SetActive (false);
		}


		//player1 touching the door, if the correct code is inputted then no longer show the UI
		if (this.GetComponent<SCR_Door>().Player1enteredBounds == true && correctCode == false || 
			this.GetComponent<SCR_Door>().Player2enteredBounds == true && correctCode == false)
		{
			CanvasNoteSheet.SetActive (true);
			RedArrows.SetActive (true);
			BlueArrows.SetActive (true);

			RedCancel.SetActive (true);
			BlueCancel.SetActive (true);
		} 
		else
		{
			CanvasNoteSheet.SetActive (false);
			RedArrows.SetActive (false);
			BlueArrows.SetActive (false);
			RedCancel.SetActive (false);
			BlueCancel.SetActive (false);
		}

	}

	public void ClearCode()
	{
		Robotcode.Clear ();
		noteCounter = 0;

		//the 4 RawImages that your using to show the arrows, set their textures to null
		for (int i = 0; i < Notes.Count; i++)
		{
			Notes[i].GetComponent<Image>().sprite = Resources.Load<Sprite> ("Art/PlaceHolder/UIBlankArrowBox") as Sprite;
		}
	}
		

	public void CheckCode()
	{
		Debug.Log ("doorcode count: " + this.GetComponent<SCR_Door> ().Doorcode.Count);
		Debug.Log ("robotCode count: " + Robotcode.Count);

		//check each element of the doorcode and compare it to the robot code
		for(int i = 0; i < Robotcode.Count; i++)
		//for(int i =0; i < this.GetComponent<SCR_Door>().Doorcode.Count; i++)
		{
			//if doorcode is not the same as robotcode the code is wrong
			if (this.GetComponent<SCR_Door>().Doorcode[i] != Robotcode [i] ||
				this.GetComponent<SCR_Door>().Doorcode.Count != Robotcode.Count)
			{
				test = false;
				Debug.Log ("Wrong code");
			} 
		}

		if (test == true)
		{
			//if the robot code is the same as the doorcode then display UI message
			Debug.Log ("code correct");
			CancelInvoke ("CheckCode");
			correctCode = true;
            //check to see what level your in and position the floor accordingly.
            this.GetComponent<SCR_Door>().SpawnWalkway = true;
            AkSoundEngine.PostEvent("GateNoteFinish", gameObject);
            GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
            GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
            p1.GetComponent<Animator>().SetBool("IsButtonPressed", false);
            p2.GetComponent<Animator>().SetBool("IsButtonPressed", false);
            p1.GetComponent<SCR_player1Initalise>().enabled = false;
            p2.GetComponent<SCR_player2Initalise>().enabled = false;
            //next whatever
        } 
		else
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
			Notes[i].GetComponent<Image>().sprite = Resources.Load<Sprite> ("Art/PlaceHolder/UIBlankArrowBox") as Sprite;
		}
			

	}
		
}
