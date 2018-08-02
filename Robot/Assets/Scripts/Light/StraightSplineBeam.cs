using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightSplineBeam : MonoBehaviour
{
    private LightResize dynamicLightBeam;
    private GameObject splineCurve;
    public Color beamColour = Color.white;
    public bool active = true; 
    public bool notStraightBeam = false;

    //Defines a length range from 1 to 10 to ensure an invalid length is not inputted.
    [Range(1, 20)]
    public int beamLength = 5;

    //Upon creating a lightbeam, the light resize script is also created and
    //its linked to this object. This insures that whenever a beam is created
    //it will also be able to resize and handle interactions correctly as it
    //will be always have the required light resize script with it.
    void Start()
    {
        CreateBeam();
        dynamicLightBeam = this.gameObject.AddComponent<LightResize>();
        dynamicLightBeam.lineBeam = this;
    }

    //Not only does this destroy the beam when called, if one exists that is,
    //but it also calls a cleanup function that calls exit code on the 
    //object that this lightbeam was touching, if it was touching one.
    void OnDestroy()
    {
        if (dynamicLightBeam != null)
        {
            dynamicLightBeam.TriggerConnectedObjectsExit();
            Destroy(dynamicLightBeam);
        }
    }

	//Either creates or destroys the beam depending on the active state of the beam.
    public void ToggleBeam()
    {
        active = !active;
    
        CreateBeam();
        DestroyBeam();

        if(this.gameObject.activeInHierarchy) StartCoroutine(LightResizeToggleControl(active));
    }

    //Either creates or destroys the beam depending on the active state of the beam.
    public void ToggleCustomBeam(Vector3 startPoint, List<Vector3> midPoints, Vector3 endPoint)
    {
        active = !active;

        CustomBeam(startPoint, midPoints, endPoint);
        DestroyBeam();
    }

    //Creates the line render object (beam drawing Unity component) and sets up the 
    //bezier code script that is used to supply the line renderer with points to map 
    //the line out. This function also allows the points of the bezier curve to be chosen
    //allowing for a customised beam to be created if required.   
    private void CustomBeam(Vector3 startPoint, List<Vector3> midPoints, Vector3 endPoint)
    {
        if (active)
        {
            splineCurve = Instantiate(Resources.Load("Prefabs/Light/LineRender")) as GameObject;
            splineCurve.name = "splineLine";
            splineCurve.GetComponent<SplineCurve>().color = beamColour;
            splineCurve.GetComponent<SplineCurve>().CustomLine(beamLength, startPoint, midPoints, endPoint);            
            splineCurve.transform.SetParent(this.transform);
            splineCurve.GetComponent<SplineCurve>().Initialise();
        }
    }

    //Creates the line render object (beam drawing Unity component) and sets up the 
    //bezier code script that is used to supply the line renderer with points to map 
    //the line out. Uses a predefined straightbeam function to generate the points
    //for the line.
    private void CreateBeam()
    {
        if (active)
        {
            splineCurve = Instantiate(Resources.Load("Prefabs/Light/LineRender")) as GameObject;
            splineCurve.name = "splineLine";
            splineCurve.GetComponent<SplineCurve>().color = beamColour;
            splineCurve.GetComponent<SplineCurve>().StraightLine(beamLength);
            splineCurve.transform.SetParent(this.transform);
            splineCurve.GetComponent<SplineCurve>().Initialise();
        }
    }

    private void DestroyBeam()
    {
        if (!active)
        {
            Destroy(splineCurve);
        }
    }

    //Every lightbeam has a collider on the end of the beam, which
    //is used to register collisions with light objects in the scene.
    //This helper function simply supplies that collider object.
    public Transform BeamChild()
    {
        return splineCurve.transform.GetChild(0);
    }

    //Simply rotates the object, for simplicity, only a vector is needed
    //with the conversion taking place here internally. This allowed for
    //not only an easier time writing out the type of rotation in terms of
    //degrees rather than radians, but also, prevented the conversion from
    //having to be written out every time a rotation request was being coded.  
    public void RotateBeam(Vector3 newRot)
    {
        StartCoroutine(BeamNotification(Quaternion.Euler(newRot)));
    }

    //Makes sure the object is not null before proceeding with its instruction, 
    //meaning it makes sure it exists in the scene first. This is useful when 
    //the object has just been created and this line is called directly after
    //as it prevents it from trying to run code on an object that Unity has
    //finished generating/building. Updates the lightresize script with the
    //state of the lightbeam.  
    IEnumerator LightResizeToggleControl(bool active)
    {
        yield return new WaitUntil(() => dynamicLightBeam != null);
        dynamicLightBeam.ToggleLight(active);
    }

    //Makes sure the object is not null before proceeding with its instruction, 
    //meaning it makes sure it exists in the scene first. This is useful when 
    //the object has just been created and this line is called directly after
    //as it prevents it from trying to run code on an object that Unity has
    //finished generating/building. Applies a rotation to the line object.
    IEnumerator BeamNotification(Quaternion newRot)
    {
        yield return new WaitUntil(()=> splineCurve != null);
        splineCurve.transform.localRotation = newRot;
    }

    //Makes sure the object is not null before proceeding with its instruction, 
    //meaning it makes sure it exists in the scene first. This is useful when 
    //the object has just been created and this line is called directly after
    //as it prevents it from trying to run code on an object that Unity has
    //finished generating/building. Changes the lines current position.
    IEnumerator BeamNotification(Vector3 newPos)
    {
        yield return new WaitUntil(() => splineCurve != null);
        splineCurve.transform.position = newPos;
    }

    public void WaitForBeamDestruction()
    {
        StartCoroutine(BeamNotification());
    }

    public IEnumerator LayerControl()
    {
        yield return StartCoroutine(BeamNotification());
        SetCorrectLayer();
    }

    private void SetCorrectLayer()
    {
        splineCurve.gameObject.layer = LayerMask.NameToLayer("BeamLayer");
    }

    //Makes sure the object is not null before proceeding with its instruction, 
    //meaning it makes sure it exists in the scene first. This is useful when 
    //the object has just been created and this line is called directly after
    //as it prevents it from trying to run code on an object that Unity has
    //finished generating/building. 
    public IEnumerator BeamNotification()
    {
        yield return new WaitUntil(() => splineCurve != null);
    }

    //A simple check that ensures that the beam is active and still exists.
    //Used in various places before lightbeam code is executed. 
    public bool IsBeamAlive()
    {
        return ((splineCurve!=null)&&(active));
    }

    public void ChangePos(Vector3 newPos)
    {
        StartCoroutine(BeamNotification(newPos));
    }
}
