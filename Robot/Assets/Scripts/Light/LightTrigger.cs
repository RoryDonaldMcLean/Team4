using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    public Color correctLightBeamColour = Color.white;
    public bool correctLight = false;
    private bool connectedToLight = false;

    //Sets the trigger colour indicator to the correct defined colour required in order to open the door
    void Start()
    {
        Transform TargetColour = this.transform.GetChild(this.transform.childCount - 1);
        for(int i = 0; i < TargetColour.childCount; i++)
        {
            TargetColour.GetChild(i).GetComponent<Renderer>().material.color = correctLightBeamColour;
        }
    }

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        if ((lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer")) && (!connectedToLight))
        {
            LineRenderer line = lightBeam.GetComponentInParent<LineRenderer>();

            //Checks to see if the predefined colour that is required to open this door, matches the lightbeams colour.
            if (CheckBeamColour(line.startColor))
            {
                CorrectColour();
                GenerateVFXResponse(true);
            }
            else
            {
                IncorrectColour();
                GenerateVFXResponse(false);
            }
        }
    }

    //Generates a VFX response after a lightbeam hits the trigger
    //with a simply bool changing the effects displayed. 
    private void GenerateVFXResponse(bool correctLight)
    {
        if(this.transform.childCount == 3)
        {
            GameObject responseVFX = Instantiate(Resources.Load("Prefabs/Particle/ParticleOrbElectric")) as GameObject;
            responseVFX.GetComponent<LightTriggerVFXResponse>().Initialize(this.transform, correctLight);
        }
    }

    void OnTriggerExit(Collider lightBeam)
    {
        if (lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer"))
        {
            LineRenderer line = lightBeam.GetComponentInParent<LineRenderer>();
            if (CheckBeamColour(line.startColor))
            {
                IncorrectColour();
            }
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
            connectedToLight = true;

            AkSoundEngine.SetState("Drone_Modulator", "Hit_Switch");
        }
    }

    private void IncorrectColour()
    {
        if (correctLight)
        {
            correctLight = false;
            connectedToLight = false;

            AkSoundEngine.SetState("Drone_Modulator", "Hit_Wall");
        }
    }

    //A simple colour comparision to indicate a match or not was found
    private bool CheckBeamColour(Color beamColour)
    {
        return (correctLightBeamColour.Equals(beamColour));
    }
}
