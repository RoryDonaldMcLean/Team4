using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFourFinal : LevelControlBaseClass 
{
    private LightTrigger lightTriggerInteract;
    private bool activatedLightBarrier = false;

    //Used as reference point for the base class
    //allowing various operations to stay dynamic.
    void Awake()
    {
        puzzleIdentifier = "PuzzleFour";
    }

    //Finds the specfic light trigger in the scene and gets a reference to it
    //used below in the specfic update.
    protected override void LevelSpecificInit()
    {
        foreach(LightTrigger lightrigger in lightDoors)
        {
            if (lightrigger.name.Contains("Interact")) lightTriggerInteract = lightrigger;
        }     
    }

    //If the light source is correct, swap a complete barrier with a 
    //colour based one.
    protected override void LevelSpecificUpdate()
    {
        if ((!activatedLightBarrier) && (lightTriggerInteract.correctLight))
        {
            activatedLightBarrier = true;

            //Create purple light barrier and destroys complete lightbarrier
            //the scale and position and parent of the complete lightbarrier
            //are used to setup the purple light barrier.
            GameObject allLightWall = GameObject.Find("AllLightBarrier");
            GameObject lightBarrier = Instantiate(Resources.Load("Prefabs/Light/LightBarrier")) as GameObject;
            lightBarrier.transform.SetParent(allLightWall.transform.parent);
            lightBarrier.transform.position = allLightWall.transform.position;
            float xScale = allLightWall.transform.localScale.x;
            xScale /= 6.0f;
            lightBarrier.transform.localScale = new Vector3(xScale, 1, 1);

            lightBarrier.GetComponentInChildren<LightBarrier>().colourToAllow = new Color(1, 0, 1, 1);

            Destroy(allLightWall);
        }
    }
}
