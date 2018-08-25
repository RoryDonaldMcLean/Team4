﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRedirect : MonoBehaviour
{
    public int beamLength = 5;
    public Color beamColour = Color.white;
    public bool beamColourRedirectControl = true;
    //private Color arrowDefaultColour = Color.white;
    private StraightSplineBeam splineCurve;
    private bool connectedBeam = false;
    //private float fadedColourAlpha = 0.1f;
    //private bool defaultColour = true;
    private Transform parent;

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        //can remove this if statement i think
        if ((!lightBeam.transform.IsChildOf(this.transform)) && (lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer")))
        {
            if (splineCurve == null)
            {
                parent = lightBeam.transform.parent.parent;
                connectedBeam = true;
                if (beamColourRedirectControl) beamColour = lightBeam.GetComponentInParent<LineRenderer>().startColor;
                CreateExtendedBeam();
            }
        }
    }

    public void TriggerEnterFunction(Collider lightBeam)
    {
        OnTriggerEnter(lightBeam);
    }

    //Upon lightbeam leaving the door trigger
    void OnTriggerExit(Collider lightBeam)
    {
        Transform exitingLightObject = lightBeam.transform.parent.parent;
        ExitBeamControl(ref exitingLightObject);
    }

    public void TriggerExitFunction(Transform exitingLightObject)
    {
        ExitBeamControl(ref exitingLightObject);
    }

    public void TriggerExitFunction()
    {
        if ((splineCurve != null)&&(connectedBeam))
        {
            ExitBeamExecution();
        }
    }

    private void ExitBeamControl(ref Transform exitingLightObject)
    {
        if ((splineCurve != null) && (connectedBeam) && (ParentLightBeam(ref exitingLightObject)))
        {
            ExitBeamExecution();
            StartCoroutine(CheckForNeighboor());
        }
    }

    private void ExitBeamExecution()
    {
        DestroyBeam();
        connectedBeam = false;
    }

    private IEnumerator CheckForNeighboor()
    {
        yield return new WaitUntil(()=>splineCurve==null);
        CheckDirectionForLightBeam();
    }

    //Raycasts around the object, looking for any nearby objects.
    //If found, processes the lightbeam found, connected it to
    //the limb object.
    private void CheckDirectionForLightBeam()
    {
        RaycastHit hit;
        float nearDistance = 3.0f;

        if (RayCast(this.transform.forward, nearDistance, out hit))
        {
            AkSoundEngine.SetState("Drone_Modulator", "Hit_Switch");
            Transform parent = hit.transform.parent.parent;
            if (!ParentLightBeam(ref parent)) TriggerEnterFunction(hit.collider);
        }
    }

    //A very specfic raycast looking for lightbeams only, around the object, 
    //at the correct height for the lightbeam.
    private bool RayCast(Vector3 direction, float length, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer("LightBeam");
        Vector3 offsetPos = Vector3.Scale(direction, this.GetComponent<Transform>().localScale);

        Vector3 raycastStartLocation = this.transform.position;
        raycastStartLocation -= offsetPos * 3.0f;
        raycastStartLocation.y = 3.49f;

        Debug.DrawRay(raycastStartLocation, direction, Color.red, length);
        return Physics.BoxCast(raycastStartLocation, this.GetComponent<Transform>().localScale, direction, out hit, Quaternion.identity, length, layerMask);
    }

    private bool ParentLightBeam(ref Transform exitingLightObject)
    {
        return (parent == exitingLightObject);
    }

    //This object has to be rotated with user clicks, with the extended beam being rotated as well
    //a checks is in place in order to ensure that after a normal rotation occurs, create the beam if it was deleted previously 
    public void PerpendicularRotate()
    {
        //ensures that the object is a divisble rot of 90, or else sets it to be that, to allow for smooth perp rot to work in this puzzle game
        //counters user defined angles from the pickup object interactions, as most puzzle placed objects are perp based.
        if(this.transform.eulerAngles.y % 90 != 0)
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, 0, rot.z);
            this.transform.rotation = Quaternion.Euler(rot);
        }
        else
        {
            this.transform.Rotate(0, 90, 0);
        }

        if((splineCurve == null) && (connectedBeam))
        {
            CancelInvoke("ColourOverTime");
            CreateExtendedBeam();
        }
    }

    private void DestroyBeam()
    {
        splineCurve.ToggleBeam();
        Destroy(splineCurve);
    }
	
    //creates a beam that functions as an extension of the beam that this object has collided with
    //taking in the original beams colour
    private void CreateExtendedBeam()
    {
        splineCurve = this.gameObject.AddComponent<StraightSplineBeam>();
        splineCurve.beamColour = beamColour;
        splineCurve.beamLength = beamLength;

        AkSoundEngine.SetState("Drone_Modulator", "Reflector");
    }

    //if the object isnt 1 in alpha channel, it has been picked up
    private bool IsPickedUp()
    {
        return (this.transform.GetComponent<Renderer>().material.color.a != 1);
    } 
}
