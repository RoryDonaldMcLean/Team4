using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnBoardingProcess : LevelControlBaseClass
{
    bool doorStateOpen = false;
	// Update is called once per frame
	void Update()
    {
		if((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
        }
        else if ((!IsAllLightTriggersActive()) && (doorStateOpen))
        {
            Debug.Log("close");
            doorStateOpen = !doorStateOpen;
            exitDoor.CloseDoor();
        }
	}
}
