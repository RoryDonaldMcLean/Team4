using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSplitter : MonoBehaviour
{
    public bool splitColour = false;
    public bool LeftSideRed = false;
    private List<StraightSplineBeam> splitBeams;
    private Color beamColour = Color.white;
    private int totalLightSplits = 2;
    //private float originalBeamInverse;

    // Use this for initialization
    void Start ()
    {
        splitBeams = new List<StraightSplineBeam>();
    }

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        beamColour = lightBeam.GetComponentInParent<LineRenderer>().startColor;
        CreateExtendedBeam();
        if (splitColour) SplitColourBetweenBeams();
    }

    //Upon lightbeam leaving the door trigger
    void OnTriggerExit(Collider lightBeam)
    {
        DestroyBeam();
    }

    public void ForceTriggerExit()
    {
        StartCoroutine(BeamNotification());
    }

    IEnumerator BeamNotification()
    {
        yield return new WaitUntil(() => splitBeams != null);
        DestroyBeam();
    }

    private void DestroyBeam()
    {
        for (int i = 0; i < totalLightSplits; i++)
        {
            if (splitBeams[i] != null)
            {
                splitBeams[i].ToggleBeam();
                Destroy(splitBeams[i]);
            }
        }
        splitBeams.Clear();
    }
    //creates a beam that functions as an extension of the beam that this object has collided with
    //taking away the original beams colour.
    private void CreateExtendedBeam()
    {
        for(int i = 0; i < totalLightSplits; i++)
        {
            splitBeams.Add(this.gameObject.AddComponent<StraightSplineBeam>());
            splitBeams[i].beamColour = beamColour;
        }
        splitBeams[0].RotateBeam(new Vector3(0, 45, 0));
        splitBeams[1].RotateBeam(new Vector3(0, -45, 0));
    }

    private void SplitColourBetweenBeams()
    {
        foreach(StraightSplineBeam lineBeam in splitBeams)
        {
            lineBeam.beamColour = Color.black;
        }

        if(LeftSideRed)
        {
            splitBeams[0].beamColour.b = beamColour.b;
            splitBeams[1].beamColour.r = beamColour.r;
        }
        else
        {
            splitBeams[0].beamColour.r = beamColour.r;
            splitBeams[1].beamColour.b = beamColour.b;
        }
    }
}
