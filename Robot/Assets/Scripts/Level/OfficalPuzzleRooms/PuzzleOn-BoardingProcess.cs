using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnBoardingProcess : LevelControlBaseClass
{
    void Awake()
    {
        puzzleIdentifier = "PuzzleZero";
    }

	// Update is called once per frame
	void Update()
    {
		if((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
			doors[0].enabled = true;
			//this line will enable the melody door when a puzzle is finished
            //Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
            GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
            walkway.name = "tempFloor";

            EndOfLevel();
        }
	}
}
