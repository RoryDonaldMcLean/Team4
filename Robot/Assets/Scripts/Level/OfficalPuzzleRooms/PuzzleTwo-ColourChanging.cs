using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTwoColourChanging : LevelControlBaseClass
{
    //Used as reference point for the base class
    //allowing various operations to stay dynamic.
    void Awake()
    {
        puzzleIdentifier = "PuzzleTwo";
    }

	protected override void LevelSpecificInit()
	{
		walkwayPosition = new Vector3(9, 0, 112.33f);
	}
}
