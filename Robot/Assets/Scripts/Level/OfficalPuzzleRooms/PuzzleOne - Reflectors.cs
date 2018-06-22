using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneReflectors : LevelControlBaseClass
{
    bool doorStateOpen = false;

    void Awake()
    {
        PuzzleIdentifier = "PuzzleOne";
    }

    // Update is called once per frame
    void Update ()
    {
        if ((IsAllLightTriggersActive()) && (!doorStateOpen))
        {        
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
        }
        else if ((!IsAllLightTriggersActive()) && (doorStateOpen))
        {
            //counter--;
            Debug.Log("close");
            doorStateOpen = !doorStateOpen;
            exitDoor.CloseDoor();
        }
    }
}
