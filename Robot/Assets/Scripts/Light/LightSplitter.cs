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

    //Upon a collison being detected with a Lightbeam, the beams are split, the connected object 
    //is stored for checks later on and its state is set to active, preventing any more onEnter calls 
    //from being processed while the system is operating. 
    public void OnEnter(Collider lightBeam)
    {
        if (!lightBeam.transform.IsChildOf(this.transform) && (!active))
        {
            active = true;
            connectedObject = lightBeam.transform.parent.parent;
            beamColour = lightBeam.GetComponentInParent<LineRenderer>().startColor;
            CreateExtendedBeam();
        }
    }

    //When a beam exits the splitter, its checked to make sure that there are split beams, 
    //meaning there is a split operation in progress, that its the relevant beam that is
    //causing the split and finally that its not already in the process of deleting the beams.
    public void OnExit(Transform exitingObject)
    {
        if(((splitBeams.Count > 0) && IsRightObject(exitingObject)) && (!isDeleting))
        {
            isDeleting = true;
            DestroyBeam();
        }
    }

    //Checks to make sure that the object leaving is the relevant one that this splitter is 
    //currently operating for.
    private bool IsRightObject(Transform exitingObject)
    {
        return (exitingObject == connectedObject);
    }

    //Destroys the two beams, clearing its list and calling an extra cleanup function.
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

    //After the beam has been destroyed, more cleanup code here is ran that resets various checks.
    //Also checks for any new beams nearby that is touching the object and should auto switch to.  
    private void ExitBeam()
    {
        active = false;
        connectedObject = null;
        isDeleting = false;

        RaycastHit hit;
        if (RayCast(this.transform.forward, 1.0f, out hit))
        {
            if (hit.transform.parent.parent.GetComponent<LightResize>().contact)
            {
                OnEnter(hit.collider);
            }
        }
    }

    //A very precise raycast that it used to check if any objects are near the prism of the splitter. 
    //This is used to determine if, after the beam has left the prism area, any other beams are touching that
    //should now be used as the connected point.
    private bool RayCast(Vector3 direction, float length, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer("LightBeam");
        Vector3 offsetPos = Vector3.Scale(direction, this.transform.GetChild(0).GetComponent<Transform>().localScale);
        Vector3 raycastStartLocation = this.transform.GetChild(0).position;
        raycastStartLocation -= offsetPos * 2.1f;
        raycastStartLocation.y = 3.49f;

        Debug.DrawRay(raycastStartLocation, direction, Color.red, length);
        return Physics.BoxCast(raycastStartLocation, this.GetComponentInChildren<Transform>().localScale, direction, out hit, Quaternion.identity, length, layerMask);
    }

    //Creates a beam that functions as an extension, continuing on from the original collided lightbeams 
    //connection, all while taking the original beams colour into account. Since the light splitter object
    //is designed to split the beam into two beams, its two beams not one that continues on from the original.
    //Furthermore, additional functionality was added to this object so that the colour of original beam could also
    //be split into its components. The two new beams are split by 45 degrees on either side by design.     
    private void CreateExtendedBeam()
    {
        if (splitBeams.Count > 0) DestroyBeam();

        for (int i = 0; i < totalLightSplits; i++)
        {
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

    //As there is only one two-tone colour in the game, the colour split function can be very simple,
    //since its purple, only the red and blue channels need to used to split the colour correctly.
    //A check is performed that also provides designers more control over which side gains 
    //which singular colour.  
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
