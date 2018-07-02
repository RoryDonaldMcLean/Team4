using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismColourCombo : MonoBehaviour
{
    private List<Color> colourBeams = new List<Color>();
    private Color newBeamColour;
    private StraightSplineBeam splineCurve;

    //Upon a collison being detected with a Lightbeam
    void OnTriggerEnter(Collider lightbeam)
    {
        Color line = lightbeam.GetComponentInParent<LineRenderer>().startColor;
        if(!CheckBeamExists(line))
        {
            colourBeams.Add(line);
            CreateNewBeamCheck();
        }
    }

    //Upon lightbeam leaving the prism
    void OnTriggerExit(Collider lightbeam)
    {
        Color line = lightbeam.GetComponentInParent<LineRenderer>().startColor;
        if (CheckBeamExists(line))
        {
            colourBeams.Remove(line);
            ModifyBeam();
        }
    }

    public void TriggerExitFunction(Color line)
    {
        if (CheckBeamExists(line))
        {
            colourBeams.Remove(line);
            ModifyBeam();
        }
    }

    //if the beam leaving has reduced the amount of beams hitting the prism to zero, delete the prism beam.
    //otherwise create a new beam using the left over beams.
    void ModifyBeam()
    {
        if (colourBeams.Count > 0)
        {
            CreateNewBeamCheck();
        }
        else
        {
            DestroyBeam();
        }
    }

    void DestroyBeam()
    {
       splineCurve.ToggleBeam();
       Destroy(splineCurve);
       this.transform.GetComponent<Renderer>().material.color = Color.white;
    }
    //simply checks if that beam has been added previously, using colour parameter to discern this.
    bool CheckBeamExists(Color collidedLine)
    {
        bool beamExists = false;
        foreach(Color line in colourBeams)
        {  
            if (line.Equals(collidedLine))
            {
                beamExists = true;
            }
        }
       
        return beamExists;
    }
    //when creating a new beam if there are multiple beams hitting the prism, colour combine them into one singular colour beam
    //otherwise just create a beam using the one beam that is hitting the prism.
    void CreateNewBeamCheck()
    {
        if(colourBeams.Count > 1)
        {
            BlendColours();
        }
        else
        {
            newBeamColour = colourBeams[0];
        }
        CreateNewLightBeam();
    }
    //adds all the colours from all the beams together, finding the blended colour from it.
    void BlendColours()
    {
        newBeamColour = new Color(0,0,0,0);
        foreach(Color line in colourBeams)
        {
            newBeamColour += line;
        }
        newBeamColour /= colourBeams.Count;
    }

    //destroys a prism beam that exists if one was still in use, before creating the new one it replaces.
    void CreateNewLightBeam()
    {
        if(splineCurve != null)
        {
            DestroyBeam();
        }

        splineCurve = this.gameObject.AddComponent<StraightSplineBeam>();
        splineCurve.beamColour = newBeamColour;

        this.transform.GetComponent<Renderer>().material.color = newBeamColour;
    }
}
