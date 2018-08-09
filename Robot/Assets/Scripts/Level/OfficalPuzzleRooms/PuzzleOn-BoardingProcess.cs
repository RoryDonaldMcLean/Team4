using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOnBoardingProcess : LevelControlBaseClass
{
    //Used as reference point for the base class
    //allowing various operations to stay dynamic.
    void Awake()
    {
        puzzleIdentifier = "PuzzleZero";
		walkwayPosition = new Vector3(9, 0, 45.83f);
    }

    //Disables player movement at the start of this 
    //scene, and returns it after the tutorial
    //is completed.  
    protected override void LevelSpecificInit()
    {
        DestroyMovement("Player1");
        DestroyMovement("Player2");
    }

    public void ActivatePlayerMovement()
    {
        AddMovement("Player1");
        AddMovement("Player2");
    }

    //Since these two players are always in the scene, no
    //safety checks are required, simply accessing them 
    //directly is going to work all the time.
    private void AddMovement(string playerTag)
    {
		GameObject.FindGameObjectWithTag (playerTag).GetComponentInChildren<Tutorial> ().ChirpsTutorial = false;
        GameObject.FindGameObjectWithTag(playerTag).GetComponent<InControlMovement>().enabled = true;
    }

    private void DestroyMovement(string playerTag)
    {
        GameObject.FindGameObjectWithTag(playerTag).GetComponent<InControlMovement>().enabled = false; 
    }
}
