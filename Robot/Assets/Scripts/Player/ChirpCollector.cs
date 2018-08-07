using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChirpCollector : MonoBehaviour 
{
	public int eventCount = 0;

	public float timeLeft = 2.0f;
	public bool startTimer = false;
	public bool playEvent = false;

	GameObject GameController;

	bool addMovementCheck = false;

	// Use this for initialization
	void Start () 
	{
		GameController = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () 
	{
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

	void Event()
	{
		//the event you want
		startTimer = false;
		playEvent = false;
		eventCount = 0;
		timeLeft = 2.0f;

		AkSoundEngine.SetRTPCValue ("Chirps_Combine", 1); 

		if (addMovementCheck == false)
		{
			GameController.GetComponent<PuzzleOnBoardingProcess> ().ActivatePlayerMovement ();
			addMovementCheck = true;
		}


	}

	void FalseEvent()
	{
		//if the timer reaches 0
		Debug.Log("false hope");
	}
}
