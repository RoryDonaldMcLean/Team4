using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightResize : MonoBehaviour
{
    private Collider puzzleObject;
    public StraightSplineBeam lineBeam;
    //private float colliderXPoint;
    private float colliderWidth;
    private float oldBeamEndPoint;
    private float newBeamEndPoint;
    private float originalBeamInverse;
    private bool contact = false;

	void Start()
	{
		RaycastControl();
	}

    public void ToggleLight(bool active)
    {
        if(active)
        {
            RaycastControl();
        }
        else
        {
            CancelInvoke("BeamRaycast");
        }
    }

	private void RaycastControl()
	{
		CancelInvoke("BeamRaycast");
		InvokeRepeating("BeamRaycast", 0.0f, 1.0f);
	}

	private void BeamRaycast()
	{
        if (lineBeam.active)
		{
			RaycastHit hit;
			float maxDraw = lineBeam.beamLength * 2.0f;
			Vector3 raycastStartLocation = this.transform.position;
           
			//Check if there has been a hit yet
			Debug.DrawRay(raycastStartLocation, this.GetComponent<Transform>().forward, Color.yellow, maxDraw);
            if ((Physics.BoxCast(raycastStartLocation, new Vector3(1, 1, 1), this.GetComponent<Transform>().forward, out hit, Quaternion.identity, maxDraw))) 
			{
				OnTriggerEnter(hit.collider);
			}
		}
	}

    //Upon a collison being detected with a object 
    void OnTriggerEnter(Collider collidedObject)
    {
        if ((!collidedObject.name.Contains("Pole"))&&(!collidedObject.name.Contains("LineColliderObject"))&&((collidedObject.gameObject.layer != LayerMask.NameToLayer("BeamLayer")))) BeamResizeController(ref collidedObject);
    }

    private void TriggerExitControl(Transform objectBlocked)
    {
        string objectBlockedName = objectBlocked.name;
        if (objectBlockedName.Contains("SlideBox"))
        {
            //base class this when you have time
            objectBlockedName = objectBlocked.GetComponent<SCR_Movable>().movableObjectString;
        }
        else if (objectBlockedName.Contains("RotateBox"))
        {
            objectBlockedName = objectBlocked.GetComponent<SCR_Rotatable>().rotatableObjectString;
        }
        switch (objectBlockedName)
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
        if(contact)
        {
            if(AwayFromBeam())
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
		RaycastControl();
        //Invoke("CreateNewBody", 0.3f);
    }

    private void CreateNewBody()
    {
        Rigidbody rigid = this.gameObject.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;
        rigid.angularDrag = 0.0f;
    }

    private bool ObjectFoundBehindBlockedBeam(out RaycastHit hit)
    {
        float maxDraw = (oldBeamEndPoint - newBeamEndPoint) - (puzzleObject.transform.lossyScale.z / 2.0f);
        Vector3 raycastStartLocation = puzzleObject.transform.position;
        raycastStartLocation.z -= puzzleObject.transform.lossyScale.z;

        //Check if there has been a hit yet
		Debug.DrawRay(raycastStartLocation, this.GetComponent<Transform>().forward, Color.yellow, maxDraw);
        return (Physics.BoxCast(raycastStartLocation, puzzleObject.transform.lossyScale, this.GetComponent<Transform>().forward, out hit, Quaternion.identity, maxDraw));
    }

    private void BeamResizeController(ref Collider collidedObject)
    {
        //finds the point where the picked up object hit the lightbeam, only in z since its the axis right represents the length of the beam and forward for the object.
        Vector3 collisionPoint = this.transform.GetChild(1).GetComponent<Collider>().ClosestPointOnBounds(collidedObject.transform.position);// - lightBeam.transform.root.position.z;     
        collisionPoint.z -= collidedObject.transform.root.position.z;

        newBeamEndPoint = Vector3.Dot(collisionPoint, this.transform.forward);
        float objectPoint = Vector3.Dot(collidedObject.transform.position, this.transform.forward);
        //finds the collider object attached to every lightbeam, that is used as the collider for most of the lightbeam code
        Transform endPointBeam = this.transform.GetChild(1).GetChild(0).transform;
        //Means the object is in Z area, but the beam needs to be resized
        //Debug.Log("meh" + collidedObject.name);
        //Debug.Log("objectpoint" + objectPoint);
        //Debug.Log("newbeamendpoint" + newBeamEndPoint);
        
        if (newBeamEndPoint != objectPoint)
        {
            //stay within beams length
            if(objectPoint < (lineBeam.beamLength * 2.0f))
            {
                //if inside beam length on Z
                newBeamEndPoint = objectPoint;
                BeamResize(ref endPointBeam, ref collidedObject);
            }
            else if(objectPoint < ((lineBeam.beamLength * 2.0f) + (collidedObject.transform.lossyScale.z / 2.0f)))
            {
                if(contact)
                {
                    ObjectExitBeamAreaResponse();
                }
                else
                {
                    StopCoroutine("CheckIfBeamEntersFurthur");
                    StartCoroutine("CheckIfBeamEntersFurthur", collidedObject);
                }
            }
        }
        //To prevent the code from being repeated pointlessly, a check is done to ensure that its a new, unique point, before proceeding. 
        else
        {
            float endPointBeamPoint = (Vector3.Dot(endPointBeam.position, this.transform.forward));// - 8.2f);
            if(endPointBeamPoint != newBeamEndPoint)
            {
                BeamResize(ref endPointBeam, ref collidedObject);
            }
        }
    }

    private IEnumerator CheckIfBeamEntersFurthur(Collider collidedObject)
    {
        float oldPosZ = Vector3.Dot(collidedObject.transform.position, this.transform.forward);
        while (true)
        {
            float currentPosZ = Vector3.Dot(collidedObject.transform.position, this.transform.forward);
            if (oldPosZ != currentPosZ)
            {
                oldPosZ = currentPosZ;
                //stay within beams length
                if (oldPosZ < (lineBeam.beamLength * 2.0f))
                {
                    //if inside beam length on Z
                    BeamResizeController(ref collidedObject);
                    StopCoroutine("CheckIfBeamEntersFurthur");
                }
            }
            yield return null;
        }
    }

    private void BeamResize(ref Transform endPointBeam, ref Collider collidedObject)
    {
        puzzleObject = collidedObject;
        oldBeamEndPoint = Vector3.Dot(endPointBeam.position, this.transform.forward);

        CalculateNewBeam(ref endPointBeam);

        //finds the width of the possible collision, using the real scale of and size of the objects being collided with. Used to ascertain if the beam is no longer in range. (in order to stop checking)
        colliderWidth = (puzzleObject.transform.lossyScale.x /2.0f) + (endPointBeam.transform.lossyScale.x / 4.0f);

        RaycastHit hit;
        if (ObjectFoundBehindBlockedBeam(out hit))
        {
            //Debug.Log("hit" + hit.transform.name);
            //TriggerExitControl(hit.transform);
        }

		CancelInvoke("BeamRaycast");

        //Destroy(this.GetComponent<Rigidbody>());
		//Destroy(collidedObject.GetComponent<Rigidbody>());
		/*
		Rigidbody rigid = collidedObject.transform.gameObject.AddComponent<Rigidbody>();
		rigid.useGravity = false;
		rigid.isKinematic = true;
		rigid.angularDrag = 0.0f;
*/
        contact = true;
    }

    private void CalculateNewBeam(ref Transform endPointBeam)
    {
        //sets the collider to the new pos on the z axis, and obtains its x pos to be used later.
        Vector3 newPos = endPointBeam.localPosition;
        newPos.z = newBeamEndPoint + Vector3.Dot(this.transform.position, -this.transform.forward);
        newPos.y = 0;

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
        float xPositionOfObject = Mathf.Abs(Vector3.Dot(puzzleObject.transform.position, this.transform.right));
        Transform endPointBeam = this.transform.GetChild(1).GetChild(0).transform;
        float colliderXPoint = Mathf.Abs(Vector3.Dot(endPointBeam.position, this.transform.right));
        return ((xPositionOfObject > (colliderXPoint + colliderWidth)) || (xPositionOfObject < (colliderXPoint - colliderWidth)));
    }
    //if the picked up object is no longer is range for a collision
    private bool ResizeBeam()
    {
        float zPositionOfObject = Vector3.Dot(puzzleObject.transform.position, this.transform.forward);
        return ((zPositionOfObject > newBeamEndPoint) || (zPositionOfObject < newBeamEndPoint));
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;

        float nearDistance = 1.0f;
        if (puzzleObject != null)
        {
            float maxDraw = (oldBeamEndPoint - newBeamEndPoint) - (puzzleObject.transform.lossyScale.z / 2.0f);
            Vector3 raycastStartLocation = puzzleObject.transform.position;
            raycastStartLocation.z -= 1;

            //Check if there has been a hit yet
            if (Physics.BoxCast(raycastStartLocation, puzzleObject.transform.lossyScale, this.GetComponent<Transform>().forward, out hit, Quaternion.identity, maxDraw))
            {
                Debug.Log("?>LOP");
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(puzzleObject.transform.position + transform.forward * nearDistance, puzzleObject.transform.lossyScale);
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                Debug.Log("sadasd");
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(transform.position, transform.forward * nearDistance);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(puzzleObject.transform.position + transform.forward * nearDistance, puzzleObject.transform.lossyScale);
            }
        }
    }
    */
}
