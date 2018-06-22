using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPoint : MonoBehaviour
{
    public Transform pickedUpTransform;
    private StraightSplineBeam lineBeam;
    private float colliderXPoint;
    private float colliderWidth;
    private Color beamColor;

    //script that allows for a lightbeam that is collided with the currently being picked up object, to be resized and processed accordingly.
    //Upon a collison being detected with a Lightbeam 
    void OnTriggerStay(Collider lightBeam)
    {
        //a check that ensures that the lightbeam being triggered isnt from the object being held currently.
        if(lightBeam.transform.parent != pickedUpTransform)
        {
            if(pickedUpTransform.name.Contains("Redirect")) LightRedirectInitial(ref lightBeam);
            if(pickedUpTransform.name.Contains("LimbLight")) LightRedirectInitial(ref lightBeam);
        }
    }

    private void LightRedirectInitial(ref Collider lightBeam)
    {
        //finds the point where the picked up object hit the lightbeam, only in z since its the axis right represents the length of the beam and forward for the object.
        float newZ = lightBeam.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position).z;// - lightBeam.transform.root.position.z;
        //finds the collider object attached to every lightbeam, that is used as the collider for most of the lightbeam code
        Transform lightBeamColliderObject = lightBeam.transform.GetChild(0).transform;
        //To prevent the code from being repeated pointlessly, a check is done to ensure that its a new, unique point, before proceeding. 
        if (lightBeamColliderObject.position.z != newZ)
        {
            //sets the collider to the new pos on the z axis, and obtains its x pos to be used later.
            Vector3 PosCollidePoint = lightBeamColliderObject.position;
            PosCollidePoint.z = newZ;
            PosCollidePoint.y = 0;
            colliderXPoint = PosCollidePoint.x;
            lightBeam.transform.GetChild(0).transform.position = PosCollidePoint;

            BeamResize(ref lightBeam);
            //Upon resizing the object through delete and create methods, triggers need to be forceable activated as they wont do so by default.
            pickedUpTransform.GetComponent<LightRedirect>().TriggerEnterFunction(lightBeamColliderObject.GetComponent<Collider>());
            //if there is a beam present, then find its colour.
            if (pickedUpTransform.childCount > 1) beamColor = pickedUpTransform.transform.GetChild(1).GetComponent<LineRenderer>().startColor;
            //finds the width of the possible collision, using the real scale of and size of the objects being collided with. Used to ascertain if the beam is no longer in range. (in order to stop checking)
            colliderWidth = (this.GetComponent<BoxCollider>().size.x / 4.0f) + (pickedUpTransform.GetChild(0).transform.lossyScale.x / 4.0f);
        }
    }

    private void BeamResize(ref Collider lightBeam)
    {
        //for this spline implmentation last two are always end point, first two are always start point, sub all in the middle for average values between the two
        SplineCurve lightCurve = lightBeam.GetComponent<SplineCurve>();
        int totalControlPoints = lightCurve.controlPoints.Count;

        Vector3 startPoint = lightCurve.controlPoints[0];
        List<Vector3> midPoints = new List<Vector3>();
        Vector3 endPoint = lightBeam.transform.GetChild(0).transform.position;
        //only the difference in the z axis is required, zeroing out the x axis is, therfore, important to keep things consistant, no matter where this object is placed, spawned.
        endPoint.x = 0;

        //new middle points cal, finds the amount required, based on the amount of control points that exist on that lightbeam
        float middlePoints = (totalControlPoints - 4);
        //finds the average number based on the amount of controlpoints needed
        Vector3 difference = endPoint - startPoint;
        difference = (difference / middlePoints) / 2.0f;

        for (int i = 2; i < middlePoints + 2; i++)
        {
            midPoints.Add(difference * (i - 1));
        }
        //destroys old default lightbeam, and creates a new one, with custom points
        lineBeam = lightBeam.transform.parent.GetComponent<StraightSplineBeam>();
        lineBeam.ToggleBeam();
        lineBeam.ToggleCustomBeam(startPoint, midPoints, endPoint);
    }

    //if the picked up object is no longer is range for a collision
    private bool AwayFromBeam()
    {
        float xPositionOfPickUpObject = this.transform.position.x;
        return ((xPositionOfPickUpObject > (colliderXPoint + colliderWidth)) || (xPositionOfPickUpObject < (colliderXPoint - colliderWidth)));
    }

    private void TriggerExitControl(Transform objectBlocked)
    {
        switch (objectBlocked.name)
        {
            case "LightSplitter":
                objectBlocked.GetComponent<LightSplitter>().ForceTriggerExit();
                break;
            case "ColourBarrier":
                objectBlocked.GetComponent<LightBarrier>().ForceTriggerExit();
                break;
            case "LightPrismColourCombo":
                objectBlocked.GetComponent<PrismColourCombo>().TriggerExitFunction(beamColor);
                break;
        }
    }

    //a check to trigger colliders if the beam had been resized 
    private void ObjectInteractionCleanup()
    {
        RaycastHit hit;
        if (ObjectFoundInfrontOfBeam(out hit))
        {
            TriggerExitControl(hit.transform);
        }

        pickedUpTransform.GetComponent<LightRedirect>().TriggerExitFunction();
    }

    private bool ObjectFoundInfrontOfBeam(out RaycastHit hit)
    {
        float beamLength = lineBeam.beamLength * 2;
        Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward, Color.blue, beamLength);
        return Physics.BoxCast(this.GetComponent<Transform>().position, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, beamLength);
    }

    void Update()
    {
        if (lineBeam != null)
        {
            if (AwayFromBeam())
            {
                //simply restores the beam back to its defaults if the object has moved out of range
                if (lineBeam.transform != pickedUpTransform)
                {
                    ObjectInteractionCleanup();

                    lineBeam.ToggleBeam();
                    lineBeam.ToggleBeam();
                    lineBeam = null;
                }
            }
        }
    }
}
