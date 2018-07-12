using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneReflectors : LevelControlBaseClass
{
    void Awake()
    {
        puzzleIdentifier = "PuzzleOne";
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsAllLightTriggersActive()) && (!doorStateOpen))
        {        
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
			GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
			walkway.name = "tempFloor";

			Vector3 pos = walkway.transform.position;
			pos.z += 54.4f;
			walkway.transform.position = pos;
            EndOfLevel();
        }
    }
}
