﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneReflectors : LevelControlBaseClass
{
	//Used as reference point for the base class
	//allowing various operations to stay dynamic.
    void Awake()
    {
        puzzleIdentifier = "PuzzleOne";
    }

	protected override void LevelSpecificInit()
	{
		walkwayPosition = new Vector3(9, 0, 42.55f);
		walkwayScale = new Vector3 (4.5f, 1.0f, 1.5f);
	}
}
