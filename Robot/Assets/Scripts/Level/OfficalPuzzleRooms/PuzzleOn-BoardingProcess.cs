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
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
            GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
            walkway.name = "tempFloor";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelController>().Level(1);
            EndOfLevel();
        }
	}
}
