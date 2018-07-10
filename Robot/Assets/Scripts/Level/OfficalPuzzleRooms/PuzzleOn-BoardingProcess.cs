using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnBoardingProcess : LevelControlBaseClass
{
    bool doorStateOpen = false;

	//GameObject MelodyDoor;

    void Awake()
    {
        PuzzleIdentifier = "PuzzleZero";

		//MelodyDoor = GameObject.FindGameObjectWithTag ("Door");
		//MelodyDoor.GetComponent<SCR_Door> ().enabled = false;
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
            //give to box
            GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
            walkway.name = "tempFloor";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().Level(1);
        }
        else if ((!IsAllLightTriggersActive()) && (doorStateOpen))
        {
            Debug.Log("close");
            doorStateOpen = !doorStateOpen;
            exitDoor.CloseDoor();
        }
	}
}
