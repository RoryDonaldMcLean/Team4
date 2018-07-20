using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRedirect : MonoBehaviour
{
    public int beamLength = 5;
    public Color beamColour = Color.white;
    public bool beamColourRedirectControl = true;
    private Color arrowDefaultColour = Color.white;
    private StraightSplineBeam splineCurve;
    private bool connectedBeam = false;
    private float originalBeamInverse;
    private float fadedColourAlpha = 0.1f;
    private bool defaultColour = true;

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        if((!lightBeam.transform.IsChildOf(this.transform)) && (lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer")))
        {
            if (splineCurve != null)
            {
                DestroyBeam();
            }

            //InverseBeamCalculate(lightBeam.transform.rotation);

            //this check ensures that the extended beam is not facing the original beam.
            //if ((this.transform.rotation.y != originalBeamInverse) && (this.transform.rotation.y != -originalBeamInverse))
            {
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
        if ((splineCurve != null)&&(connectedBeam))
        {
            connectedBeam = false;
            DestroyBeam();
        }
    }

    public void TriggerExitFunction()
    {
        if ((splineCurve != null)&&(connectedBeam))
        {
            connectedBeam = false;
            DestroyBeam();
        }
    }

    //simply finds the inverse of the beam, would indicates the beams source
    //this prevents a beam being created that simply overlaps the original beam
    private void InverseBeamCalculate(Quaternion rotation)
    {
        Vector3 rot = rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        Quaternion convertToQuaternion = Quaternion.Euler(rot);
        originalBeamInverse = convertToQuaternion.y;
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

        CancelInvoke("ColourOverTime");

        if (arrowDefaultColour.Equals(Color.grey))
        {
            arrowDefaultColour = Color.white;
        }

        Color newColour = arrowDefaultColour;
        //if object is picked up, and is therefore, faded, carry over this change in the blinking arrow code
        if (IsPickedUp()) newColour.a = fadedColourAlpha;

        this.transform.GetComponent<Renderer>().material.color = newColour;
    }
    //creates a beam that functions as an extension of the beam that this object has collided with
    //taking in the original beams colour
    private void CreateExtendedBeam()
    {
        splineCurve = this.gameObject.AddComponent<StraightSplineBeam>();
        splineCurve.beamColour = beamColour;
        splineCurve.beamLength = beamLength;

        AkSoundEngine.SetState("Drone_Modulator", "Reflector");

        BlinkingArrowsSetup();
    }

    private void BlinkingArrowsSetup()
    {
        if(beamColour.Equals(arrowDefaultColour))
        {
            arrowDefaultColour = Color.grey;
        }
        else if(arrowDefaultColour.Equals(Color.grey))
        {
            arrowDefaultColour = Color.white;
        }

        CancelInvoke("ColourOverTime");
        InvokeRepeating("ColourOverTime", 0.3f, 0.75f);
    }
    
    private void ColourOverTime()
    {
        Color newColour;

        if (defaultColour)
        {
            newColour = arrowDefaultColour;
        }
        else
        {
            newColour = beamColour;
        }
        defaultColour = !defaultColour;

        //if object is picked up, and is therefore, faded, carry over this change in the blinking arrow code
        if (IsPickedUp()) newColour.a = fadedColourAlpha;
        this.transform.GetComponent<Renderer>().material.color = newColour;
    }
    
    //if the object isnt 1 in alpha channel, it has been picked up
    private bool IsPickedUp()
    {
        return (this.transform.GetComponent<Renderer>().material.color.a != 1);
    } 
}
