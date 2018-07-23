using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismColourCombo : MonoBehaviour
{
    private List<Color> colourBeams = new List<Color>();
    private Color newBeamColour;
    private StraightSplineBeam splineCurve;
    private bool destroyingBeam = false;
    public int beamLength = 5;

    //Upon a collison being detected with a Lightbeam
    void OnTriggerEnter(Collider lightbeam)
    {
        Color lineColour = lightbeam.GetComponentInParent<LineRenderer>().startColor;
        if(!CheckBeamExists(lineColour))
        {
            colourBeams.Add(lineColour);
            CreateNewBeamCheck();
        }
    }

    //Upon lightbeam leaving the prism
    void OnTriggerExit(Collider lightbeam)
    {
        Color lineColour = lightbeam.GetComponentInParent<LineRenderer>().startColor;
        BeamExit(lineColour);
    }

    public void TriggerExitFunction(Color lineColour)
    {
        BeamExit(lineColour);
    }

    private void BeamExit(Color lineColour)
    {
        if(!destroyingBeam)
        {
            destroyingBeam = true;
            while (CheckBeamExists(lineColour))
            {
                colourBeams.Remove(lineColour);
            }
            ModifyBeam();
            StartCoroutine(BeamDestroyedCheck());
        }
    }

    private IEnumerator BeamDestroyedCheck()
    {
        yield return new WaitUntil(()=> splineCurve == null);
        destroyingBeam = false;
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
        if (splineCurve != null)
        {
            splineCurve.ToggleBeam();
            Destroy(splineCurve);
            this.transform.GetComponent<Renderer>().material.color = Color.white;
        }
    }
    //simply checks if that beam has been added previously, using colour parameter to discern this.
    bool CheckBeamExists(Color lineColour)
    {
        foreach(Color line in colourBeams)
        {  
            if (line.Equals(lineColour))
            {
                return true;
            }
        }
       
        return false;
    }
    //when creating a new beam if there are multiple beams hitting the prism, colour combine them into one singular colour beam
    //otherwise just create a beam using the one beam that is hitting the prism.
    void CreateNewBeamCheck()
    {
        if(colourBeams.Count > 1)
        {
            BlendColours();
            AkSoundEngine.SetState("Drone_Modulator", "Combiner");
        }
        else
        {
            newBeamColour = colourBeams[0];
            
			AkSoundEngine.SetState("Drone_Modulator", "Colour_Change");
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
        newBeamColour.a = 1;
    }

    //destroys a prism beam that exists if one was still in use, before creating the new one it replaces.
    void CreateNewLightBeam()
    {
       DestroyBeam();

       splineCurve = this.gameObject.AddComponent<StraightSplineBeam>();
       splineCurve.beamColour = newBeamColour;
       splineCurve.beamLength = beamLength;

       this.transform.GetComponent<Renderer>().material.color = newBeamColour;
    }
}
