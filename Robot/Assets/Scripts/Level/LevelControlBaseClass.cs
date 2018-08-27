using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControlBaseClass : MonoBehaviour
{
    //containers of all the possible puzzle elements in the scene,
    //note they are not initialized, they are only setup if those
    //elements are found to exist in that particular scene.
    protected List<GameObject> barriers;
    protected List<WeightCheck> buttons;
	protected List<SCR_Door> doors;
    protected List<StraightSplineBeam> beams;
    protected List<LightTrigger> lightDoors;
    protected PuzzleExitDoor exitDoor;
    protected List<GameObject> lightSources;

    protected string puzzleIdentifier;
    protected bool doorStateOpen = false;
	protected Vector3 walkwayPosition = Vector3.zero;
	protected Vector3 walkwayScale = Vector3.zero;
    private bool lightShowFinished = false;

    void Start()
    {
        SpecficPuzzleSetup("Barriers");
        SpecficPuzzleSetup("Buttons");
        SpecficPuzzleSetup("Doors");
        SpecficPuzzleSetup("Beams");
        SpecficPuzzleSetup("LightDoors");
        SpecficPuzzleSetup("LightSources");

        DoorSetup();

        LevelSpecificInit();

        AkSoundEngine.SetState("Drone_Modulator", "Start");
    }

    //Simply checks if all light triggers have been
    //correctly solved, and that the door isn't already
    //active. The door is activated and its visual/audio
    //responses are also triggered. 
    //If the door is open, spawn the walkway and path to
    //the next part of the game.	
    void Update()
    {
        if ((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
            //This line will enable the melody door when a puzzle is finished
            doors[0].enabled = true;

            LimbBoxesReset();

            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
            AkSoundEngine.SetState("Drone_Modulator", "Complete");

            EndOfLevel();
        }

        LevelSpecificUpdate();
        
        if (doorStateOpen)
        {
            if (doors[0].SpawnWalkway == true)
            {
				GameObject exit = GameObject.FindGameObjectWithTag("ExitDoor");
                //exit.SetActive(false);
                SlideDoorOpen(exit);
                if (!puzzleIdentifier.Contains("PuzzleFour"))
                {
                    //GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
                    //walkway.name = "tempFloor";
                    //walkway.transform.position = walkwayPosition;
					//walkway.transform.localScale = walkwayScale;
                }

                doors[0].SpawnWalkway = false;
                doors[0].Correct = false;
            }
        }
    }

    private void SlideDoorOpen(GameObject exit)
    {
        Debug.Log("PlayOpenDoorAnima");
        exit.GetComponent<Animator>().Play("DoorOpen");
        AkSoundEngine.PostEvent("Door_Open", exit);
    }

    private void LimbBoxesReset()
    {
        Transform puzzleContainer = doors[0].transform.parent.parent;
        LimbLight[] limbLights = puzzleContainer.GetComponentsInChildren<LimbLight>();

        foreach (LimbLight limbLight in limbLights)
        {
            limbLight.ReturnLimbsToPlayer();
        }
    }

    //Useful so that, if any level scripts needed any additional, specific setup, it could be performed.    
    protected virtual void LevelSpecificInit(){}

    //Useful so that, if any level scripts needed any additional, specific update code, it could be performed.    
    protected virtual void LevelSpecificUpdate(){}

    //When called, all light triggers were correct and therefore, the puzzle 
    //was solved. Starts the blinking beam code process.
    protected void EndOfLevel()
    {
        AkSoundEngine.PostEvent("Activate_Crystal", gameObject);
        AkSoundEngine.SetState("Drone_Modulator", "Complete");

        foreach(GameObject lightsource in lightSources)
        {
			if (lightsource.name.Contains("Rotatable"))
            {
                lightsource.GetComponent<SCR_Rotatable>().interactable = false;
                EmittersEndControl(lightsource.GetComponentInChildren<LightEmitter>());
            }
            else if((lightsource.name.Contains("Movable")))
            {
                EmittersEndControl(lightsource.GetComponentInChildren<LightEmitter>());
            }
            else
            {
                EmittersEndControl(lightsource.GetComponent<LightEmitter>());
            }
        }

        StartCoroutine(LevelCompleteLightShow());
        StartCoroutine(TurnOffLights());
    }

    private void EmittersEndControl(LightEmitter emitter)
    {
        emitter.canBeTurnedOff = false;
    }

    //Waits half a second before starting the light show, by calling the blinking
    //function on every beam in the scene.
    private IEnumerator LevelCompleteLightShow()
    {
        yield return new WaitForSeconds(0.5f);

        QuantifyLightSourcesEndGameSetup();
    }

    private void QuantifyLightSourcesEndGameSetup()
    {
        LineRenderer[] lightBeams = GameObject.Find(puzzleIdentifier).GetComponentsInChildren<LineRenderer>();

        foreach (LineRenderer beam in lightBeams)
        {
            if (!beam.transform.parent.GetComponent<LightResize>().finished)
            {
                beam.transform.parent.GetComponent<LightResize>().EndLightBeamInteraction();
                StartCoroutine(BlinkingLightControl(beam));
            }
        }
    }

    //Waits until the blinking lightbeam control is finished, then turns off the lightbeams
    //for good, to once again provide a visual response to the level/puzzle being completed.
    //This code simply disabled all light sources, preventing them for working, signalling
    //that the lightbeam puzzle was over.   
    private IEnumerator TurnOffLights()
    {
        yield return new WaitUntil(()=>lightShowFinished);
        foreach (GameObject lightSource in lightSources)
        {
            lightSource.GetComponentInChildren<LightEmitter>().TurnOffForGood();
        }
    }

    //Controls the execution of the blinking lightbeam, 10 times the blinking
    //beams coroutine would be called. Then the end level code proceeds at the end of this control. 
    private IEnumerator BlinkingLightControl(LineRenderer line)
    {
        int counter = 0;
        Color beamColour = Color.green;

        line.startColor = beamColour;
        line.endColor = beamColour;

        while (counter < 10)
        {
            if (line != null)
            {
                yield return StartCoroutine(BlinkingLight(line));
            }
            counter++;
        }

        if(counter < 0) lightShowFinished = true;
        yield return null;
    }

    //Using a coroutine, every half a second will simply switch the light beam 
    //from visible to invisible. 
    private IEnumerator BlinkingLight(LineRenderer line)
    {
        yield return new WaitForSeconds(0.5f);

        if (line != null)
        {
            Color beamColour = line.startColor;

            if (beamColour.a > 0)
            {
                beamColour.a = 0;
                line.startColor = beamColour;
                line.endColor = beamColour;
            }
            else
            {
                beamColour.a = 1;
                line.startColor = beamColour;
                line.endColor = beamColour;
            }
        }
    }

    //Sets up the visual aid component of the door, which is used to show that the door is now
    //turned on following a correctly completed level.  
    private void DoorSetup()
    {
        exitDoor = doors[0].transform.parent.GetComponentInChildren<PuzzleExitDoor>();
    }

    //Goes through all the light triggers in the scene and checks if they are all
    //activated and the light is correct for all of them.
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

    //Adds up all the objects in this level into their respective data containers
    //uses a parent gameobject as a container for them, in order to order and store them with ease 
    private List<GameObject> InitialiseGenericPuzzleElements(string parentTag)
    {
        List<GameObject> puzzleObjects = new List<GameObject>();
        
        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag(parentTag);

        if (parentObjects != null)
        {
            foreach (GameObject parentObject in parentObjects)
            {
                Transform puzzleObjectContainer = parentObject.transform;
                if (puzzleObjectContainer.parent.name.Contains(puzzleIdentifier))
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

    //Takes the genericly assembled objects and correctly sorts them into their specfic objects
    //with their specfic containers and scripts. 
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
				doors = new List<SCR_Door>();				
                foreach (GameObject puzzleObject in puzzleObjects)
                {
                    doors.Add(puzzleObject.GetComponent<SCR_Door>());
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
            case "LightSources":
                lightSources = puzzleObjects;
                break;
        }
    }
}