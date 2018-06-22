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
    //defines a length range from 1 to 10 to ensure an invalid length is not inputted.
    [Range(1, 20)]
    public int beamLength = 5;
    // Use this for initialization
    void Start()
    {
        CreateBeam();
        dynamicLightBeam = this.gameObject.AddComponent<LightResize>();
        dynamicLightBeam.lineBeam = this;
    }

    void OnDestroy()
    {
        Destroy(dynamicLightBeam);
    }

	//either creates or destroys the beam depending on the active state of the beam.
    public void ToggleBeam()
    {
        active = !active;

        CreateBeam();
        DestroyBeam();
    }
    //either creates or destroys the beam depending on the active state of the beam.
    public void ToggleCustomBeam(Vector3 startPoint, List<Vector3> midPoints, Vector3 endPoint)
    {
        active = !active;

        CustomBeam(startPoint, midPoints, endPoint);
        DestroyBeam();
    }

    private void CustomBeam(Vector3 startPoint, List<Vector3> midPoints, Vector3 endPoint)
    {
        if (active)
        {
            splineCurve = Instantiate(Resources.Load("Prefabs/Light/LineRender")) as GameObject;
            splineCurve.name = "splineLine";
            splineCurve.GetComponent<SplineCurve>().color = beamColour;
            splineCurve.GetComponent<SplineCurve>().CustomLine(beamLength, startPoint, midPoints, endPoint);
            splineCurve.transform.SetParent(this.transform);
        }
    }

    private void CreateBeam()
    {
        if (active)
        {
            splineCurve = Instantiate(Resources.Load("Prefabs/Light/LineRender")) as GameObject;
            splineCurve.name = "splineLine";
            splineCurve.GetComponent<SplineCurve>().color = beamColour;
            splineCurve.GetComponent<SplineCurve>().StraightLine(beamLength);
            splineCurve.transform.SetParent(this.transform);
        }
    }

    private void DestroyBeam()
    {
        if (!active)
        {
            Destroy(splineCurve);
        }
    }

    public void RotateBeam(Vector3 newRot)
    {
        StartCoroutine(BeamNotification(Quaternion.Euler(newRot)));
    }

    IEnumerator BeamNotification(Quaternion newRot)
    {
        yield return new WaitUntil(()=> splineCurve != null);
        splineCurve.transform.localRotation = newRot;
    }

    IEnumerator BeamNotification(Vector3 newPos)
    {
        yield return new WaitUntil(() => splineCurve != null);
        splineCurve.transform.position = newPos;
    }

    IEnumerator BeamNotification()
    {
        yield return new WaitUntil(() => splineCurve != null);
        Debug.Log("GoBeam");
    }

    public bool IsBeamAlive()
    {
        return ((splineCurve!=null)&&(active));
    }

    public void ChangePos(Vector3 newPos)
    {
        StartCoroutine(BeamNotification(newPos));
    }
}
