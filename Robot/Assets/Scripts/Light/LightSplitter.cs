using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSplitter : MonoBehaviour
{
    public bool splitColour = false;
    public bool LeftSideRed = false;
    private List<GameObject> splitBeams;
    private Color beamColour = Color.white;
    private int totalLightSplits = 2;
    public int beamLength = 5;
    //private float originalBeamInverse;

    // Use this for initialization
    void Start()
    {
        splitBeams = new List<GameObject>();
    }

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        if ((!lightBeam.transform.IsChildOf(this.transform)) && (lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer")))
        {
            beamColour = lightBeam.GetComponentInParent<LineRenderer>().startColor;
            CreateExtendedBeam();
        }
    }

    //Upon lightbeam leaving the door trigger
    void OnTriggerExit(Collider lightBeam)
    {
        if (splitBeams.Count > 0)
        {
            DestroyBeam();
        }
    }

    public void ForceTriggerExit()
    {
        //StartCoroutine(BeamNotification());
        if(splitBeams.Count > 0)
        {
            DestroyBeam();
        }
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
                splitBeams[i].GetComponent<StraightSplineBeam>().ToggleBeam();
                Destroy(splitBeams[i]);
            }
        }
        splitBeams.Clear();
    }
    //creates a beam that functions as an extension of the beam that this object has collided with
    //taking away the original beams colour.
    private void CreateExtendedBeam()
    {
        if (splitBeams.Count > 0) DestroyBeam();

        for (int i = 0; i < totalLightSplits; i++)
        {
            //creates an endpoint collider, to be used for collision detection with the beam.
            GameObject lightBeam = Instantiate(Resources.Load("Prefabs/Light/LightBeam")) as GameObject;
            lightBeam.name = "LightBeamObject " + i;
            lightBeam.transform.SetParent(this.transform);
            lightBeam.transform.position = this.transform.position;
            lightBeam.transform.rotation = this.transform.rotation;

            splitBeams.Add(lightBeam);
            splitBeams[i].GetComponent<StraightSplineBeam>().beamColour = beamColour;
            splitBeams[i].GetComponent<StraightSplineBeam>().beamLength = beamLength;
        }
        splitBeams[0].transform.Rotate(Vector3.up * 45);
        splitBeams[1].transform.Rotate(Vector3.up * -45);

        if (splitColour) SplitColourBetweenBeams();

        //AkSoundEngine.SetState("Drone_Modulator", "Splitter");
    }

    private void SplitColourBetweenBeams()
    {
        foreach(GameObject lineBeam in splitBeams)
        {
            lineBeam.GetComponent<StraightSplineBeam>().beamColour = Color.black;
        }

        if(LeftSideRed)
        {
            splitBeams[0].GetComponent<StraightSplineBeam>().beamColour.b = beamColour.b;
            splitBeams[1].GetComponent<StraightSplineBeam>().beamColour.r = beamColour.r;
        }
        else
        {
            splitBeams[0].GetComponent<StraightSplineBeam>().beamColour.r = beamColour.r;
            splitBeams[1].GetComponent<StraightSplineBeam>().beamColour.b = beamColour.b;
        }
    }
}
