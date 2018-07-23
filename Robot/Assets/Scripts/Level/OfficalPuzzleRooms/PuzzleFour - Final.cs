using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFourFinal : LevelControlBaseClass 
{
    private LightTrigger lightTriggerInteract;
    private bool activatedLightBarrier = false;

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
			AkSoundEngine.SetState("Drone_Modulator", "Complete");
            EndOfLevel();
        }
        else if ((!activatedLightBarrier) && (lightTriggerInteract.correctLight))
        {           
            //create purple light barrier
            GameObject allLightWall = GameObject.Find("AllLightBarrier");
            GameObject lightBarrier = Instantiate(Resources.Load("Prefabs/Light/LightBarrier")) as GameObject;
            lightBarrier.transform.SetParent(allLightWall.transform.parent);
            lightBarrier.transform.position = allLightWall.transform.position;
            float xScale = allLightWall.transform.localScale.x;
            xScale /= 6.0f;
            lightBarrier.transform.localScale = new Vector3(xScale, 1, 1);

            lightBarrier.GetComponent<LightBarrier>().colourToAllow = new Color(1,0,1,1);

            activatedLightBarrier = true;

            Destroy(allLightWall);
        }

    }
}
