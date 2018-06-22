using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnBoardingProcess : LevelControlBaseClass
{
    bool doorStateOpen = false;

    void Awake()
    {
        PuzzleIdentifier = "PuzzleZero";
    }

	// Update is called once per frame
	void Update()
    {
		if((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
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
