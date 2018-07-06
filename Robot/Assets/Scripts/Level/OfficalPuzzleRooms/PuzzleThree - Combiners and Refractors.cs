﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleThreeCombinersandRefractors : LevelControlBaseClass
{
    void Awake()
    {
        puzzleIdentifier = "PuzzleThree";
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
