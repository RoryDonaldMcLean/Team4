using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChirpCollector : MonoBehaviour
{
    public int eventCount = 0;
    public bool playeroneChirped = false;
    public bool playertwoChirped = false;
    public float timeLeft = 1.0f;
    public bool startTimer = false;
    public bool playEvent = false;

    bool addMovementCheck = false;

    // Use this for initialization
    void Start()
    {
        AkSoundEngine.SetState("ChirpCombine", "Off");
        playeroneChirped = false;
        playertwoChirped = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (eventCount >= 2)
		{
			NormalEvent ();
		}

        //example of timer will be put elsewhere?
        if (startTimer == true)
        {
            timeLeft -= Time.deltaTime;

            if (playeroneChirped == true && playertwoChirped == true)
            {
                playEvent = true;
            }
        }

        if (timeLeft <= 0)
        {
            Debug.Log("timer is at 0, play event");
            startTimer = false;
            timeLeft = 1.0f;
            playeroneChirped = false;
            playertwoChirped = false;
            FalseEvent ();
		}


		if (playEvent == true)
		{
			Event();
		}
	}

	void NormalEvent()
	{
		if ((this.GetComponent<LevelController>().currentLevel==0)&&(addMovementCheck == false))
		{
			this.GetComponent<PuzzleOnBoardingProcess> ().ActivatePlayerMovement ();
			addMovementCheck = true;
		}
	}

	void Event()
	{

        AkSoundEngine.SetState("ChirpCombine", "On");
        AkSoundEngine.PostEvent("ChirpCombine", gameObject);
        Debug.Log(" hope");

        //the event you want
        startTimer = false;
		playEvent = false;
        playeroneChirped = false;
        playertwoChirped = false;
        timeLeft = 1.0f;

	}

	void FalseEvent()
	{
		//if the timer reaches 0
		Debug.Log("false hope");
        AkSoundEngine.SetState("ChirpCombine", "Off");

    }
}
