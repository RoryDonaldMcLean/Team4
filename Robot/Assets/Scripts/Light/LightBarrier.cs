using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBarrier : MonoBehaviour
{
    public bool inverseBlockProcess = false;
    public Color colourToBlock;

    private Collider originalLightBeam;
    private StraightSplineBeam lineBeam;
    private Color resultantColour;

    void Start()
    {
        Color transparentVersionOfColour = colourToBlock;
        transparentVersionOfColour.a = 0.5f;

        this.transform.GetComponent<Renderer>().material.color = transparentVersionOfColour;
        this.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", (colourToBlock * 0.5f));
    }

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        if(originalLightBeam != null)
        {
            DestroyBeam();
        }

        originalLightBeam = lightBeam;
        resultantColour = originalLightBeam.GetComponentInParent<LineRenderer>().startColor;

        if (inverseBlockProcess)
        {
            BlockChosenColour();
        }
        else
        {
            AllowOnlyChosenColour();
        }
    }

    private void BlockChosenColour()
    {
        resultantColour = resultantColour - colourToBlock;
        resultantColour.a = 1.0f;

        if (!resultantColour.Equals(new Color(0, 0, 0, 1))) CreateExtendedBeam();
    }

    private void AllowOnlyChosenColour()
    {
        if(resultantColour.Equals(colourToBlock)) CreateExtendedBeam();
    }

    void Update()
    {
        if(lineBeam != null)
        {
            Vector3 newPos = originalLightBeam.ClosestPointOnBounds(transform.position);// - originalLightBeam.transform.root.position;
            newPos.y = originalLightBeam.transform.position.y;

            lineBeam.ChangePos(newPos);
        }
    }

    //Upon lightbeam leaving the door trigger
    void OnTriggerExit(Collider lightBeam)
    {
        DestroyBeam();
    }

    public void ForceTriggerExit()
    {
        DestroyBeam();
    }

    private void DestroyBeam()
    {
        if (lineBeam != null)
        {
            lineBeam.ToggleBeam();
            Destroy(lineBeam);
            originalLightBeam = null;
        }
    }
    //creates a beam that functions as an extension of the beam that this object has collided with
    //taking away the original beams colour.
    private void CreateExtendedBeam()
    {
        lineBeam = this.gameObject.AddComponent<StraightSplineBeam>();
        lineBeam.beamColour = resultantColour;
    }
}
