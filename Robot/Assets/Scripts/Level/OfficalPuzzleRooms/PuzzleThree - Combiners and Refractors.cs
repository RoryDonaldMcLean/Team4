﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleThreeCombinersandRefractors : LevelControlBaseClass
{

	GameObject MelodyDoor;
	GameObject Exit;

    void Awake()
    {
        puzzleIdentifier = "PuzzleThree";
		MelodyDoor = GameObject.FindGameObjectWithTag ("Doors");
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
			doors [0].enabled = true;
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
			AkSoundEngine.SetState("Drone_Modulator", "Complete");
            
            EndOfLevel();
        }

		if(doorStateOpen)
		{
			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().SpawnWalkway == true)
			{
				GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
				walkway.name = "tempFloor";
				//Vector3 pos = walkway.transform.position;
				//pos.z += 54.4f * 3.0f;
				Exit = GameObject.FindGameObjectWithTag ("ExitDoor");
				Exit.SetActive (false);

				MelodyDoor.GetComponentInChildren<SCR_Door> ().SpawnWalkway = false;
				MelodyDoor.GetComponentInChildren<SCR_Door> ().Correct = false;
			}
		}
    }
}
