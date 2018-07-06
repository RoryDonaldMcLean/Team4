using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlock : MonoBehaviour
{
    private StraightSplineBeam lineBeam;
    private float colliderXPoint;
    private float colliderWidth;
    private float oldZ;
    private float newZ;
    private float originalBeamInverse;

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        BeamResize(ref lightBeam);
    }

    private void TriggerExitControl(Transform objectBlocked)
    {
        switch(objectBlocked.name)
        {
            case "LightSplitter":
            objectBlocked.GetComponent<LightSplitter>().ForceTriggerExit();
            break;
            case "LightPrismColourCombo":
            objectBlocked.GetComponent<PrismColourCombo>().TriggerExitFunction(lineBeam.beamColour);
            break;
        }
    }

    void Update()
    {
        if (lineBeam != null)
        {
            if (AwayFromBeam())
            {
                lineBeam.ToggleBeam();
                lineBeam.ToggleBeam();
                lineBeam = null;
            }
        }
    }

    //simply finds the inverse of the beam, which indicates the beams source
    //this prevents a beam being created that simply overlaps the original beam
    private void InverseBeamCalculate(Quaternion rotation)
    {
        Vector3 rot = rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        Quaternion convertToQuaternion = Quaternion.Euler(rot);
        originalBeamInverse = convertToQuaternion.y;
        Debug.Log("Inverse" + originalBeamInverse);
    }

    private bool ObjectFoundBehindBlockedBeam(out RaycastHit hit)
    {
        float maxDraw = (oldZ - newZ);

        Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward, Color.red, maxDraw);
        return Physics.BoxCast(this.GetComponent<Transform>().position, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, maxDraw);
    }

    private void BeamResize(ref Collider lightBeam)
    {
        //finds the point where the picked up object hit the lightbeam, only in z since its the axis right represents the length of the beam and forward for the object.
        newZ = lightBeam.ClosestPointOnBounds(transform.position).z;// - lightBeam.transform.root.position.z;
        //finds the collider object attached to every lightbeam, that is used as the collider for most of the lightbeam code
        Transform lightBeamColliderObject = lightBeam.transform.GetChild(0).transform;
        //To prevent the code from being repeated pointlessly, a check is done to ensure that its a new, unique point, before proceeding. 
        if (lightBeamColliderObject.position.z != newZ)
        {
            oldZ = lightBeamColliderObject.position.z;

            //sets the collider to the new pos on the z axis, and obtains its x pos to be used later.
            Vector3 PosCollidePoint = lightBeamColliderObject.position;
            PosCollidePoint.z = newZ;
            PosCollidePoint.y = 0;
            colliderXPoint = PosCollidePoint.x;
            lightBeamColliderObject.position = PosCollidePoint;

            //for this spline implementation last two are always end point, first two are always start point, sub all in the middle for average values between the two
            SplineCurve lightCurve = lightBeam.GetComponent<SplineCurve>();
            int totalControlPoints = lightCurve.controlPoints.Count;

            Vector3 startPoint = lightCurve.controlPoints[0];
            List<Vector3> midPoints = new List<Vector3>();
            Vector3 endPoint = lightBeamColliderObject.position;
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

            //finds the width of the possible collision, using the real scale of and size of the objects being collided with. Used to ascertain if the beam is no longer in range. (in order to stop checking)
            colliderWidth = (this.GetComponent<BoxCollider>().size.x / 2.0f) + (lightBeamColliderObject.transform.lossyScale.x / 4.0f);

            RaycastHit hit;
            if (ObjectFoundBehindBlockedBeam(out hit))
            {
                TriggerExitControl(hit.transform);
            }
        }
    }

    //if the picked up object is no longer is range for a collision
    private bool AwayFromBeam()
    {
        float xPositionOfObject = this.transform.position.x;
        return ((xPositionOfObject > (colliderXPoint + colliderWidth)) || (xPositionOfObject < (colliderXPoint - colliderWidth)));
    }
}
