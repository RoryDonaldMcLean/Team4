using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTwoColourChanging : LevelControlBaseClass
{
    void Awake()
    {
        puzzleIdentifier = "PuzzleTwo";
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
            EndOfLevel();
        }
    }
}
