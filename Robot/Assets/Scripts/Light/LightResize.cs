using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightResize : MonoBehaviour
{
    private Collider puzzleObject;
    private StraightSplineBeam lineBeam;
    private float colliderXPoint;
    private float colliderWidth;
    private float oldPoint;
    private float newPoint;
    private float originalBeamInverse;
    private bool contact = false;

    void Start()
    {
        lineBeam = this.transform.GetComponent<StraightSplineBeam>();
    }

    //Upon a collison being detected with a object 
    void OnTriggerEnter(Collider collidedObject)
    {
        BeamResizeController(ref collidedObject);
    }

    private void TriggerExitControl(Transform objectBlocked)
    {
        switch (objectBlocked.name)
        {
            case "LightSplitter":
                objectBlocked.GetComponent<LightSplitter>().ForceTriggerExit();
                break;
            case "LightBarrier":
                objectBlocked.GetComponent<LightBarrier>().ForceTriggerExit();
                break;
            case "LightPrismColourCombo":
                objectBlocked.GetComponent<PrismColourCombo>().TriggerExitFunction(lineBeam.beamColour);
                break;
        }
    }

    void Update()
    {
        if (contact)
        {
            if (AwayFromBeam())
            {
                lineBeam.ToggleBeam();
                lineBeam.ToggleBeam();

                if(lineBeam.IsBeamAlive())
                {
                    ObjectExitBeamAreaResponse();
                }
            }
            else if(ResizeBeam())
            {
                BeamResizeController(ref puzzleObject);
            }
        }
    }

    private void ObjectExitBeamAreaResponse()
    {
        contact = false;
        puzzleObject = null;
        Invoke("createNewBody", 0.3f);
    }

    private void createNewBody()
    {
        Rigidbody rigid = this.gameObject.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
        rigid.angularDrag = 0.0f;
    }

    private bool ObjectFoundBehindBlockedBeam(out RaycastHit hit)
    {
        float maxDraw = (oldPoint - newPoint);
        Vector3 direction = this.GetComponent<Transform>().forward;
        Vector3 offsetPos = (-direction * 1.1f);
        Vector3 posObject = this.GetComponent<Transform>().position;
        posObject.z = newPoint;
        posObject += offsetPos;

        Debug.DrawRay(posObject, direction, Color.red, maxDraw);
        return Physics.BoxCast(posObject, this.GetComponent<Transform>().localScale, direction, out hit, this.GetComponent<Transform>().rotation, maxDraw);
    }

    private void BeamResizeController(ref Collider collidedObject)
    {
        //finds the point where the picked up object hit the lightbeam, only in z since its the axis right represents the length of the beam and forward for the object.
        newPoint = this.transform.GetChild(1).GetComponent<Collider>().ClosestPointOnBounds(collidedObject.transform.position).z;// - lightBeam.transform.root.position.z;

        //finds the collider object attached to every lightbeam, that is used as the collider for most of the lightbeam code
        Transform endPointBeam = this.transform.GetChild(1).GetChild(0).transform;

        //Means the object is in Z area, but the beam needs to be resized
        if (newPoint != collidedObject.transform.position.z)
        {
            //stay within beams length
            if(collidedObject.transform.position.z < (lineBeam.beamLength * 2.0f) + (collidedObject.transform.lossyScale.z/2.0f))
            {
                newPoint = collidedObject.transform.position.z;
                BeamResize(ref endPointBeam, ref collidedObject);
            }
            else
            {
                Debug.Log("Z Done");
                ObjectExitBeamAreaResponse();
                //exit code check
            }
        }
        //To prevent the code from being repeated pointlessly, a check is done to ensure that its a new, unique point, before proceeding. 
        else if (endPointBeam.position.z != newPoint)
        {
            BeamResize(ref endPointBeam, ref collidedObject);
        }
    }

    private void BeamResize(ref Transform endPointBeam, ref Collider collidedObject)
    {
        puzzleObject = collidedObject;
        oldPoint = endPointBeam.position.z;

        CalculateNewBeam(ref endPointBeam);

        //finds the width of the possible collision, using the real scale of and size of the objects being collided with. Used to ascertain if the beam is no longer in range. (in order to stop checking)
        colliderWidth = (puzzleObject.GetComponent<BoxCollider>().size.x / 2.0f) + (endPointBeam.transform.lossyScale.x / 4.0f);

        RaycastHit hit;
        if (ObjectFoundBehindBlockedBeam(out hit))
        {
            TriggerExitControl(hit.transform);
        }

        Destroy(this.GetComponent<Rigidbody>());
        contact = true;
    }

    private void CalculateNewBeam(ref Transform endPointBeam)
    {
        //sets the collider to the new pos on the z axis, and obtains its x pos to be used later.
        Vector3 newPos = endPointBeam.position;
        newPos.z = newPoint;
        newPos.y = 0;
        colliderXPoint = newPos.x;

        endPointBeam.position = newPos;

        //for this spline implementation last two are always end point, first two are always start point, sub all in the middle for average values between the two
        SplineCurve lightCurve = this.transform.GetChild(1).GetComponent<SplineCurve>();
        int totalControlPoints = lightCurve.controlPoints.Count;

        Vector3 startPoint = lightCurve.controlPoints[0];
        List<Vector3> midPoints = new List<Vector3>();
        Vector3 endPoint = endPointBeam.position;
        //only the difference in the z axis is required, zeroing out the x axis is, therfore, important to keep things consistant, no matter where this object is placed, spawned.
        endPoint.x = 0;

        //new middle points cal, finds the amount required, based on the amount of control points that exist on that lightbeam
        float middlePoints = (totalControlPoints - 4);
        //finds the average number based on the amount of control points needed
        Vector3 difference = endPoint - startPoint;
        difference = (difference / middlePoints) / 2.0f;

        for (int i = 2; i < middlePoints + 2; i++)
        {
            midPoints.Add(difference * (i - 1));
        }
        //destroys old default lightbeam, and creates a new one, with custom points

        lineBeam.ToggleBeam();
        lineBeam.ToggleCustomBeam(startPoint, midPoints, endPoint);
    }

    //if the picked up object is no longer is range for a collision
    private bool AwayFromBeam()
    {
        float xPositionOfObject = puzzleObject.transform.position.x;
        return ((xPositionOfObject > (colliderXPoint + colliderWidth)) || (xPositionOfObject < (colliderXPoint - colliderWidth)));
    }

    //if the picked up object is no longer is range for a collision
    private bool ResizeBeam()
    {
        float zPositionOfObject = puzzleObject.transform.position.z;
        return ((zPositionOfObject > newPoint) || (zPositionOfObject < newPoint));
    }
}
