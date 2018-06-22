﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControlBaseClass : MonoBehaviour
{
    //containers of all the possible puzzle elements in the scene
    protected List<GameObject> barriers;
    protected List<WeightCheck> buttons;
    protected List<SCR_Melody> doors;
    protected List<StraightSplineBeam> beams;
    protected List<LightTrigger> lightDoors;
    protected PuzzleExitDoor exitDoor;
    protected string PuzzleIdentifier;

    void Start()
    {
        SpecficPuzzleSetup("Barriers");
        SpecficPuzzleSetup("Buttons");
        SpecficPuzzleSetup("Doors");
        SpecficPuzzleSetup("Beams");
        SpecficPuzzleSetup("LightDoors");
        GameObject door = GameObject.FindGameObjectWithTag("PuzzleExitDoor");
        exitDoor = door.transform.GetChild(0).GetComponent<PuzzleExitDoor>();
        Debug.Log(">" + lightDoors.Count);
    }

    protected bool IsAllLightTriggersActive()
    {
        bool activeState = false;
        foreach(LightTrigger trigger in lightDoors)
        {
            activeState = trigger.correctLight;
            if (!activeState) return activeState;
        }

        return activeState;
    }

    //adds up all the objects in this level into their respective data containers
    //uses a parent gameobject as a container for them, in order to order and store them with ease 
    private List<GameObject> InitialiseGenericPuzzleElements(string parentTag)
    {
        List<GameObject> puzzleObjects = new List<GameObject>();
        
        //GameObject puzzleContainer = GameObject.FindGameObjectWithTag(PuzzleIdentifier);
        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag(parentTag);

        if (parentObjects != null)
        {
            foreach (GameObject parentObject in parentObjects)
            {
                Transform puzzleObjectContainer = parentObject.transform;
                if (puzzleObjectContainer.parent.name.Contains(PuzzleIdentifier))
                {
                    for (int i = 0; i < puzzleObjectContainer.childCount; i++)
                    {
                        puzzleObjects.Add(puzzleObjectContainer.GetChild(i).gameObject);
                    }
                }
            }
        }
        return puzzleObjects;
    }

    private void SpecficPuzzleSetup(string parentTag)
    {
        List<GameObject> puzzleObjects = InitialiseGenericPuzzleElements(parentTag); 
        switch (parentTag)
        {
            case "Barriers":
                barriers = new List<GameObject>();
                barriers = puzzleObjects;
                break;
            case "Buttons":
                buttons = new List<WeightCheck>();
                foreach (GameObject puzzleObject in puzzleObjects)
                {
                    buttons.Add(puzzleObject.GetComponent<WeightCheck>());
                }
                break;
            case "Doors":
                doors = new List<SCR_Melody>();
                foreach (GameObject puzzleObject in puzzleObjects)
                {
                    doors.Add(puzzleObject.GetComponent<SCR_Melody>());
                }
                break;
            case "Beams":
                beams = new List<StraightSplineBeam>();
                foreach (GameObject puzzleObject in puzzleObjects)
                {
                    beams.Add(puzzleObject.GetComponent<StraightSplineBeam>());
                }
                break;
            case "LightDoors":
                lightDoors = new List<LightTrigger>();
                foreach (GameObject puzzleObject in puzzleObjects)
                {
                    lightDoors.Add(puzzleObject.GetComponent<LightTrigger>());
                }
                break;
        }
    }
}
