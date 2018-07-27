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
    private bool active = false;
    private Transform connectedObject;
    private bool isDeleting = false;

    // Use this for initialization
    void Start()
    {
        splitBeams = new List<GameObject>();
    }

    //Upon a collison being detected with a Lightbeam 
    public void OnEnter(Collider lightBeam)
    {
        if ((!lightBeam.transform.IsChildOf(this.transform)) && (lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer"))&&(!active))
        {
            active = true;
            connectedObject = lightBeam.transform.parent.parent;
            beamColour = lightBeam.GetComponentInParent<LineRenderer>().startColor;
            CreateExtendedBeam();
        }
    }

    public void OnExit(Transform exitingObject)
    {
        if(((splitBeams.Count > 0) && isRightObject(exitingObject)) && (!isDeleting))
        {
            isDeleting = true;
            DestroyBeam();
        }
    }

    private bool isRightObject(Transform exitingObject)
    {
        return (exitingObject == connectedObject);
    }

    private IEnumerator BeamNotification()
    {
        yield return new WaitUntil(() => splitBeams != null);
        DestroyBeam();
    }

    private IEnumerator BeamDestructionConfirm(int index)
    {
        splitBeams[index].GetComponent<StraightSplineBeam>().ToggleBeam();
        Destroy(splitBeams[index]);
        yield return new WaitUntil(() => splitBeams[index] == null);
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
        ExitBeam();
    }

    private void ExitBeam()
    {
        active = false;
        connectedObject = null;
        isDeleting = false;

        RaycastHit hit;
        if (RayCast(this.transform.forward, 1.0f, out hit))
        {
            //OnEnter(hit.collider);
        }
    }

    private bool RayCast(Vector3 direction, float length, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer("LightBeam");
        Vector3 offsetPos = Vector3.Scale(direction, this.transform.GetChild(0).GetComponent<Transform>().localScale);
        Vector3 raycastStartLocation = this.transform.position;
        raycastStartLocation -= offsetPos * 2.165f;
        raycastStartLocation.y = 3.49f;

        Debug.DrawRay(raycastStartLocation, direction, Color.red, length);
        return Physics.BoxCast(raycastStartLocation, this.GetComponent<Transform>().localScale, direction, out hit, Quaternion.identity, length, layerMask);
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

        if ((splitColour) && (beamColour.Equals(new Color(1,0,1,1)))) SplitColourBetweenBeams();

        AkSoundEngine.SetState("Drone_Modulator", "Splitter");
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
