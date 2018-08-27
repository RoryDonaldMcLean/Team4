using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBaseClass : MonoBehaviour
{
    protected List<LightTrigger> lightTriggers;
    protected GameObject entranceWall;
    protected GameObject exitWall;
    protected List<GameObject> lightSources;
    protected List<GameObject> lightObjects;
    protected List<GameObject> destroyableWalls;
    protected string tutorialIdentifier;
    protected GameObject tutorialPrompt;

    //Waits until the blinking lightbeam control is finished, then turns off the lightbeams
    //for good, to once again provide a visual response to the level/puzzle being completed.
    //This code simply disabled all light sources, preventing them for working, signalling
    //that the lightbeam puzzle was over.   
    private void TurnOffLights()
    {
        foreach (GameObject lightSource in lightSources)
        {
            lightSource.GetComponentInChildren<LightEmitter>().TurnOffForGood();
        }
    }

    //Sets up the visual aid component of the door, which is used to show that the door is now
    //turned on following a correctly completed level.  
    private void SpecialWallsSetup()
    {
        entranceWall = GameObject.FindGameObjectWithTag("EntranceWall");
        //entranceWall.SetActive(false);
        entranceWall.GetComponentInChildren<Animator>().Play("DoorOpen");

        exitWall = GameObject.FindGameObjectWithTag("ExitWall");
    }

    public void TurnOnEntranceWall()
    {
        entranceWall.SetActive(true);
        entranceWall.GetComponentInChildren<Animator>().Play("Idle");
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
                if (puzzleObjectContainer.parent.name.Contains(tutorialIdentifier))
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
            case "LightDoors":
                lightTriggers = new List<LightTrigger>();
                foreach (GameObject puzzleObject in puzzleObjects)
                {
                    lightTriggers.Add(puzzleObject.GetComponent<LightTrigger>());
                }
                break;
            case "LightSources":
                lightSources = puzzleObjects;
                break;
            case "DestroyableWalls":
                destroyableWalls = puzzleObjects;
                break;
            case "LightObjects":
                lightObjects = puzzleObjects;
                break;
        }
    }

    protected virtual void TextPromptTutorialSetup()
    {

    }

    protected virtual void TextPromptTutorial()
    {

    }

    // Use this for initialization
    void Start()
    {
        SpecficPuzzleSetup("LightDoors");
        SpecficPuzzleSetup("LightSources");
        SpecficPuzzleSetup("DestroyableWalls");
        SpecficPuzzleSetup("LightObjects");

        SpecialWallsSetup();

        TextPromptTutorialSetup();
    }

    // Update is called once per frame
    void Update()
    {
        int numberLeft = lightTriggers.Count;
        if (numberLeft == 0) ExitTutorial();

        for (int i = 0; i < numberLeft; i++)
        {
            if (lightTriggers[i].correctLight)
            {
                if (destroyableWalls.Count != 0)
                {
                    Destroy(destroyableWalls[i]);
                    destroyableWalls.RemoveAt(i);
                }
                lightTriggers.RemoveAt(i);
                break;
            }
        }

        TextPromptTutorial();
    }

    private void LightObjectsDisablePickup()
    {
        foreach(GameObject lightObject in lightObjects)
        {
            lightObject.tag = "Untagged";
        }
        GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PickupAndDropdown_Trigger>().DropObject();
        GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PickupAndDropdown_Trigger>().DropObject();
    }

    private void ExitTutorial()
    {
        TurnOffLights();     
        ToggleExitControl();
        LightObjectsDisablePickup();
        Destroy(this);
    }

    private void ToggleExitControl()
    {
        //bool wallState = exitWall.GetComponent<MeshRenderer>().enabled;
        //exitWall.GetComponent<MeshRenderer>().enabled = !wallState;
        //exitWall.GetComponent<BoxCollider>().isTrigger = wallState;
        exitWall.GetComponentInChildren<Animator>().Play("DoorOpen");
    }
}
