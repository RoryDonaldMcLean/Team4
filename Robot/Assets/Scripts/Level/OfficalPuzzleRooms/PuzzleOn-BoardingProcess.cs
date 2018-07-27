using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnBoardingProcess : LevelControlBaseClass
{

	GameObject MelodyDoor;
	GameObject Exit;

    void Awake()
    {
        puzzleIdentifier = "PuzzleZero";
		MelodyDoor = GameObject.FindGameObjectWithTag ("Doors");
    }

	// Update is called once per frame
	void Update()
    {
		if((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
			doors[0].enabled = true;
			//this line will enable the melody door when a puzzle is finished
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
			AkSoundEngine.SetState("Drone_Modulator", "Complete");

            EndOfLevel();
        }

		if (doorStateOpen)
		{
			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().SpawnWalkway == true)
			{
				Exit = GameObject.FindGameObjectWithTag ("ExitDoor");
				Exit.SetActive (false);

				GameObject walkway = Instantiate (Resources.Load ("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
				walkway.name = "tempFloor";
				MelodyDoor.GetComponentInChildren<SCR_Door> ().SpawnWalkway = false;
				MelodyDoor.GetComponentInChildren<SCR_Door> ().Correct = false;
			}
		}
	}
}
