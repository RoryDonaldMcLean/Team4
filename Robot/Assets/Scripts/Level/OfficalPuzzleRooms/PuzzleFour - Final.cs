using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFourFinal : LevelControlBaseClass 
{
    private LightTrigger lightTriggerInteract;
    private bool activatedLightBarrier = false;

	GameObject MelodyDoor;
	GameObject Exit;

    void Awake()
    {
        puzzleIdentifier = "PuzzleFour";
    }

    protected override void LevelSpecificInit()
    {
        foreach(LightTrigger lightrigger in lightDoors)
        {
            if (lightrigger.name.Contains("Interact")) lightTriggerInteract = lightrigger;
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsAllLightTriggersActive()) && (!doorStateOpen))
        {
			doors [0].enabled = true;
            Debug.Log("open");
            doorStateOpen = !doorStateOpen;
            exitDoor.OpenDoor();
			//AkSoundEngine.SetState("Drone_Modulator", "Complete");
            EndOfLevel();
        }
        else if ((!activatedLightBarrier) && (lightTriggerInteract.correctLight))
        {
            activatedLightBarrier = true;

            //create purple light barrier
            GameObject allLightWall = GameObject.Find("AllLightBarrier");
            GameObject lightBarrier = Instantiate(Resources.Load("Prefabs/Light/LightBarrier")) as GameObject;
            lightBarrier.transform.SetParent(allLightWall.transform.parent);
            lightBarrier.transform.position = allLightWall.transform.position;
            float xScale = allLightWall.transform.localScale.x;
            xScale /= 6.0f;
            lightBarrier.transform.localScale = new Vector3(xScale, 1, 1);

            lightBarrier.GetComponentInChildren<LightBarrier>().colourToAllow = new Color(1,0,1,1);

            Destroy(allLightWall);
        }

		if(doorStateOpen)
		{
			if (MelodyDoor.GetComponentInChildren<SCR_Door> ().SpawnWalkway == true)
			{
				GameObject walkway = Instantiate(Resources.Load("Prefabs/PuzzleGenericItems/tempFloor")) as GameObject;
				walkway.name = "tempFloor";
				//Vector3 pos = walkway.transform.position;
				//pos.z += 54.4f * 3.0f;
				Exit = GameObject.FindGameObjectWithTag ("ExitDoor");
				Exit.SetActive (false);

				MelodyDoor.GetComponentInChildren<SCR_Door> ().SpawnWalkway = false;
				MelodyDoor.GetComponentInChildren<SCR_Door> ().Correct = false;
			}
		}

    }
}
