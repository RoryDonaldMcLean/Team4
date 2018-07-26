using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismColourCombo : MonoBehaviour
{
    private List<Color> colourBeams = new List<Color>();
    private List<Transform> beams = new List<Transform>();
    private Color newBeamColour;
    private StraightSplineBeam splineCurve;
    private Color beamBeingDestroyed = Color.black;
    public int beamLength = 5;

    //Upon a collison being detected with a Lightbeam, first a check to make sure that its not infact itself is conducted.
    //Then a check to ensure that it is a newly detected beam, and not one already found is performed.
    //Then the beam is added to the light and its colour is processed to be blended with any others already in that list.
    public void OnEnter(Collider lightbeam)
    {
        Transform beam = lightbeam.transform.parent.parent;
        if (beam.name != this.transform.name)
        {
            if (!CheckBeamExists(beam))
            {
                beams.Add(beam);
                CalculateColourBeams();
                CreateNewBeamCheck();
            }
        }
    }

    //Upon lightbeam leaving the prism
    public void OnExit(Collider lightbeam)
    {
        BeamExit(lightbeam.transform.parent.parent);
    }

    public void TriggerExitFunction(Transform beam)
    {
        BeamExit(beam);
    }

    //Checks if the exiting beam is indeed in the list before trying to remove it, this prevent multiple exit calls
    //accidentally causing issues trying to remove something already deleted.
    //Then it is simply removed from the list and a new beam is created using what colours are left, if any. 
    private void BeamExit(Transform beam)
    {
        if (beams.Count > 2)
        {
            for (int i = 0; i < beams.Count; i++)
            {
                Debug.Log("object " + beams[i].name);
            }
        }

        if (CheckBeamExists(beam))
        {
            beams.Remove(beam);
            CalculateColourBeams();
            ModifyBeam();
        }
    }

    //Finds all the beams colours and adds them to a freshly cleared list, ready to then be used to calculate
    //the resultant blended colour, to be then used for the new correct combined beam. 
    private void CalculateColourBeams()
    {
        colourBeams.Clear();
        foreach (Transform beam in beams)
        {
            Color lineColour = beam.GetChild(beam.childCount - 1).GetComponent<LineRenderer>().startColor;
            colourBeams.Add(lineColour);
        }
    }

    //simply checks if that beam has been added previously, using transform parameter to discern this.
    private bool CheckBeamExists(Transform line)
    {
        foreach (Transform beam in beams)
        {
            if (beam.Equals(line))
            {
                return true;
            }
        }

        return false;
    }

    //If the beam leaving has reduced the amount of beams touching the object to zero, delete the beam.
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
            colourBeams.Clear();
        }
    }

    void DestroyBeam()
    {
        if (splineCurve != null)
        {
            splineCurve.ToggleBeam();
            Destroy(splineCurve);
        }
    }

    //when creating a new beam if there are multiple beams touching, colour combine them into one singular colour beam
    //otherwise just create a beam using the one beam that is hitting the object.
    void CreateNewBeamCheck()
    {
        if(colourBeams.Count > 1)
        {
            BlendColours();
            //AkSoundEngine.SetState("Drone_Modulator", "Combiner");
        }
        else
        {
            newBeamColour = colourBeams[0];
            
			//AkSoundEngine.SetState("Drone_Modulator", "Colour_Change");
        }
        CreateNewLightBeam();
    }

    //To blend the colours together, first all the connected beams colours were added together and then divided by the total
    //number of beams, finding the blended colour from it. A simple alpha channel correction was then performed.  
    void BlendColours()
    {
        newBeamColour = new Color(0,0,0,0);
        foreach(Color line in colourBeams)
        {
            newBeamColour += line;
        }
        newBeamColour.a = 1;
    }

    //Destroys a beam that exists if one is still in use, before creating a new one that replaces it.
    //Uses the colour property that can either be a combined colour or singular one, depending on the
    //situation.
    void CreateNewLightBeam()
    {
       DestroyBeam();

       splineCurve = this.gameObject.AddComponent<StraightSplineBeam>();
       splineCurve.beamColour = newBeamColour;
       splineCurve.beamLength = beamLength;
    }
}
