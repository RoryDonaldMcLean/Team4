using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    public Color correctLightBeamColour = Color.white;
    public bool correctLight = false;

    //sets the door colour indicator to the correct defined colour required in order to open the door
    //an emissive level is added to better match the colour with the bright lightbeams 
    void Start()
    {
        this.transform.GetChild(0).GetComponent<Renderer>().material.color = correctLightBeamColour;
    }

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightbeam)
    {
        LineRenderer line = lightbeam.GetComponentInParent<LineRenderer>();
        //checks to see if the predefined colour that is required to open this door, matches the lightbeams colour.
        if (CheckBeamColour(line.startColor))
        {
            CorrectColour();
        }
        else
        {
            IncorrectColour();
        }
    }

    void OnTriggerExit(Collider lightbeam)
    {
        LineRenderer line = lightbeam.GetComponentInParent<LineRenderer>();
        if (CheckBeamColour(line.startColor))
        {
            IncorrectColour();
        }
    }

    public void ForceOnTriggerExit(Color colour)
    {
        if (CheckBeamColour(colour))
        {
            IncorrectColour();
        }
    }

    private void CorrectColour()
    {
        if (!correctLight)
        {
            correctLight = true;
			AkSoundEngine.SetState("Drone_Modulator", "Hit_Switch");

        }
    }

    private void IncorrectColour()
    {
        if (correctLight)
        {
            correctLight = false;
            AkSoundEngine.SetState("Drone_Modulator", "Hit_Wall");
        }
    }

    //a simple colour comparision to indicate a match or not was found
    private bool CheckBeamColour(Color beamColour)
    {
        return (correctLightBeamColour.Equals(beamColour));
    }
}
