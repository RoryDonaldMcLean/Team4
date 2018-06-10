using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLevelTwoControl : LevelControlBaseClass
{
	// Update is called once per frame
	void Update ()
    {
        if (lightDoors[0].GetComponent<LightTrigger>().doorOpen)
        {
            Debug.Log("NEXT LEVEL DONE");
        }

        //check if the first pressure plate as been pressed down
        if (buttons[0].GetComponent<WeightCheck>().pressed == true)
        {
            if (beams[0].GetComponent<StraightSplineBeam>().active)
            {
                beams[0].GetComponent<StraightSplineBeam>().ToggleBeam();
                GameObject.FindGameObjectWithTag("Prism").GetComponent<PrismColourCombo>().TriggerExitFunction(beams[0].GetComponent<StraightSplineBeam>().beamColour);
            }
        }
        else
        {
            if (!beams[0].GetComponent<StraightSplineBeam>().active) beams[0].GetComponent<StraightSplineBeam>().ToggleBeam();
        }
    }
}
