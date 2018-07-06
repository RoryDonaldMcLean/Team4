using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightResize : MonoBehaviour
{
    private RaycastHit objectInfo;
    private Collider puzzleObject;
    public StraightSplineBeam lineBeam;

    private float colliderWidth;
    private float oldBeamEndPoint;
    private float newBeamEndPoint = 10;
    private float originalBeamInverse;
    private bool contact = false;
    private int defaultBeamLength;
    private float defaultBeamEndPoint;

    void Start()
	{
        defaultBeamLength = lineBeam.beamLength * 2;
        defaultBeamEndPoint = defaultBeamLength + Vector3.Dot(this.transform.position, this.transform.forward);

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
            //CancelInvoke("DefaultLightRaycast");

            StopCoroutine(RaycastOnRepeat());
        }
    }

    private void CheckForPrevCollidedObject()
    {
        if (puzzleObject != null)
        {
            RaycastHit hit;
            if (ObjectFoundBehindBlockedBeam(out hit))
            {
                TriggerExitControl(hit.transform);
            }
        }
    }

    private void RaycastControl()
	{
		//CancelInvoke("DefaultLightRaycast");
		//InvokeRepeating("DefaultLightRaycast", 0.0f, 0.16666f);

        StopCoroutine(RaycastOnRepeat());
        StartCoroutine(RaycastOnRepeat());
	}

    private IEnumerator RaycastOnRepeat()
    {
        while(!contact)
        {
            DefaultLightRaycast();

            yield return new WaitForFixedUpdate();
        }
    }

    private void DefaultLightRaycast()
	{
        if (lineBeam.active)
		{
            if (RaycastBeam())
            {
                if (puzzleObject != objectInfo.collider)
                {
                    OnEnterObject();
                }
            }  
		}
	}

    private void ResizeLightRaycast()
    {
        if (lineBeam.active)
        {
            if (RaycastBeam())
            {
                OnEnterObject();
            }
            else 
            {
                CleanUpCollidedObject();
            }
        }
    }

    private bool RaycastBeam()
    {
        float raycastSize = 0.18f;

        float maxDraw = lineBeam.beamLength * 2.0f;
        Vector3 raycastStartLocation = this.transform.GetChild(this.transform.childCount - 1).position;
        int layerMask = ~(1 << LayerMask.NameToLayer("LightBeam") | 1 << LayerMask.NameToLayer("BeamLayer") | 1 << LayerMask.NameToLayer("PlayerLayer"));

        return (Physics.BoxCast(raycastStartLocation, new Vector3(raycastSize, raycastSize, raycastSize), this.GetComponent<Transform>().forward, out objectInfo, this.transform.localRotation, maxDraw, layerMask));
    }

    //Upon a collison being detected with a object 
    void OnEnterObject()
    {
        if (objectInfo.collider.name.Contains("ColourBarrier"))
        {
            if(objectInfo.transform.GetComponent<LightBarrier>().OnEnter(lineBeam.beamColour))
            {
                BeamResizeController();
            }
            else if (ObjectFoundBehindIgnoredObject())
            {
                if (puzzleObject != objectInfo.collider) BeamResizeController();
            }
        }
        //prolly remove this if statement at some pt
        else if ((!objectInfo.collider.name.Contains("Pole"))&&(!objectInfo.collider.name.Contains("LineColliderObject"))) BeamResizeController();
    }

    public void TriggerExitControl(Transform objectBlocked)
    {
        string objectBlockedName = objectBlocked.name;
        if (objectBlockedName.Contains("SlideBox"))
        {
            //base class this when you have time
            objectBlockedName = objectBlocked.parent.GetComponent<SCR_Movable>().movableObjectString;
        }
        else if (objectBlockedName.Contains("RotateBox"))
        {
            objectBlockedName = objectBlocked.parent.GetComponent<SCR_Rotatable>().rotatableObjectString;
        }
        switch (objectBlockedName)
        {
            case "LightSplitter":
                objectBlocked.GetComponent<LightSplitter>().ForceTriggerExit();
                break;
            case "LightPrismColourCombo":
                objectBlocked.GetComponent<PrismColourCombo>().TriggerExitFunction(lineBeam.beamColour);
                break;
            case "LightTrigger":
                objectBlocked.GetComponent<LightTrigger>().ForceOnTriggerExit(lineBeam.beamColour);
                break;
            case "LightRedirect":
                objectBlocked.GetComponent<LightRedirect>().TriggerExitFunction();
                break;
            case "LimbLight":
                LightRedirect objectRedirect = objectBlocked.GetComponent<LightRedirect>();
                if (objectRedirect != null) objectRedirect.TriggerExitFunction();
                break;
        }
    }

    void Update()
    {
        if(contact)
        {
            if(!lineBeam.active)
            {
                LightBeamSwitchOffControl();
            }
            else if(AwayFromBeam())
            {
				Debug.Log("away" + this.transform.root.name);
                CleanUpCollidedObject();
            }
            else if(ShouldResizeBeam())
            {
                Debug.Log("resize");
                ResizeBeam();
            }
        }
    }

    private void ResizeBeam()
    {
        float endPoint = (Vector3.Dot(this.transform.GetChild(this.transform.childCount - 1).GetChild(0).transform.position, this.transform.forward));

        if (!Mathf.Approximately(endPoint, defaultBeamEndPoint))
        {
            if (endPoint < defaultBeamEndPoint)
            {
                contact = false;
                ResizeLightRaycast();
            }
            else
            {
                CleanUpCollidedObject();
            }
        }
    }

    private void CleanUpCollidedObject()
    {
        if (lineBeam.IsBeamAlive())
        {
            ObjectExitBeamAreaResponse();
        }

        lineBeam.ToggleBeam();
        lineBeam.ToggleBeam();
    }

    private IEnumerator LightInterruptionControl()
    {
        yield return StartCoroutine(LightInterruptionCheck());
    }

    private IEnumerator LightInterruptionCheck()
    {
        while (contact)
        {
            Invoke("DefaultLightRaycast", 0.1666f);
            yield return null;
        }
        StopCoroutine("LightInterruptionCheck");
    }

    public void TriggerConnectedObjectsExit()
    {
        if(puzzleObject != null)
        {
            TriggerExitControl(puzzleObject.transform);
            puzzleObject = null;
        }
    }

    private void LightBeamSwitchOffControl()
    {
        contact = false;
        TriggerConnectedObjectsExit();
    }

    private void ObjectExitBeamAreaResponse()
    {       
        contact = false;
        TriggerConnectedObjectsExit();
		//RaycastControl();
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
        float raycastSize = 1.0f;
        float maxDraw = (oldBeamEndPoint - newBeamEndPoint);

        Vector3 raycastStartLocation = objectInfo.point;
        int layerMask = ~(1 << LayerMask.NameToLayer("LightBeam") | 1 << LayerMask.NameToLayer("BeamLayer") | 1 << LayerMask.NameToLayer("PlayerLayer"));

        return (Physics.BoxCast(raycastStartLocation, new Vector3(raycastSize, raycastSize, raycastSize), this.GetComponent<Transform>().forward, out hit, this.transform.localRotation, maxDraw, layerMask));
    }

    private bool ObjectFoundBehindIgnoredObject()
    {
        float raycastSize = 0.2f;
        float maxDraw = (defaultBeamEndPoint - Vector3.Dot(objectInfo.transform.position, this.transform.forward));

        Vector3 raycastStartLocation = objectInfo.point;
        int layerMask = ~(1 << LayerMask.NameToLayer("LightBeam") | 1 << LayerMask.NameToLayer("BeamLayer") | 1 << LayerMask.NameToLayer("PlayerLayer"));

        return (Physics.BoxCast(raycastStartLocation, new Vector3(raycastSize, raycastSize, raycastSize), this.GetComponent<Transform>().forward, out objectInfo, this.transform.localRotation, maxDraw, layerMask));
    }

    private void BeamResizeController()
    {
        Collider collidedObject = objectInfo.collider;

        //finds the point where the picked up object hit the lightbeam, only in z since its the axis right represents the length of the beam and forward for the object.   
        newBeamEndPoint = Vector3.Dot(objectInfo.point, this.transform.forward);

        //finds the collider object attached to every lightbeam, that is used as the collider for most of the lightbeam code
        Transform endPointObject = this.transform.GetChild(this.transform.childCount - 1).GetChild(0).transform;    
        float endPointObjectValue = (Vector3.Dot(endPointObject.position, this.transform.forward));

        //float objectPoint = Vector3.Dot(collidedObject.transform.position, this.transform.forward);
        if (!Mathf.Approximately(endPointObjectValue, newBeamEndPoint))
        {
            BeamResize(ref endPointObject, ref collidedObject);
        }
        else
        {
            CleanUpCollidedObject();
        }
        /*
        //Means the object is in Z area, but the beam needs to be resized       
        if (newBeamEndPoint != objectPoint)
        {
            //Debug.Log("occurs" + collidedObject.name);
            //stay within beams length
            if(objectPoint < defaultBeamEndPoint)
            {
                //if inside beam length on Z
                newBeamEndPoint = objectPoint;
                BeamResize(ref endPointBeam, ref collidedObject);
            }
            else if(objectPoint < (defaultBeamEndPoint + (collidedObject.transform.lossyScale.z / 2.0f)))
            {
                if(contact)
                {
                    ObjectExitBeamAreaResponse();
                }
            }
            else
            {
                Debug.Log("error king 0 " + collisionPoint);
                Debug.Log("error king 1 " + newBeamEndPoint);
                Debug.Log("error king 2 " + objectPoint);
            }
        }
        //To prevent the code from being repeated pointlessly, a check is done to ensure that its a new, unique point, before proceeding. 
        else
        {
            float endPointBeamPoint = (Vector3.Dot(endPointBeam.position, this.transform.forward));
            if(endPointBeamPoint != newBeamEndPoint)
            {
                BeamResize(ref endPointBeam, ref collidedObject);
            }
        }
        */
    }

    private IEnumerator CheckIfBeamEntersFurthur(RaycastHit hit)
    {
        Collider collidedObject = hit.collider;
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
                    BeamResizeController();
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
        colliderWidth = WidthCalculate();

        CheckForPrevCollidedObject();

        //CancelInvoke("DefaultLightRaycast");
        StopCoroutine(RaycastOnRepeat());

        contact = true;

        StartCoroutine(LightInterruptionControl());
    }

    private float WidthCalculate()
    {
        Vector3 scale = Vector3.Scale(puzzleObject.transform.lossyScale, puzzleObject.transform.right);

        float widthOnX = 0;
        float widthOnZ = 0;

        if (scale.x != 0) widthOnX = Mathf.Sqrt(scale.x * scale.x);
        if (scale.z != 0) widthOnZ = Mathf.Sqrt(scale.z * scale.z);

        float width = (widthOnZ + widthOnX) / 2.0f;

        return width;
    }

    private void CalculateNewBeam(ref Transform endPointBeam)
    {
        //sets the collider to the new pos on the z axis, and obtains its x pos to be used later.
        Vector3 newPos = endPointBeam.localPosition;
        newPos.z = newBeamEndPoint + Vector3.Dot(this.transform.GetChild(this.transform.childCount - 1).position, -this.transform.forward);
        newPos.y = 0;

        endPointBeam.position = newPos;

        //for this spline implementation last two are always end point, first two are always start point, sub all in the middle for average values between the two
        SplineCurve lightCurve = this.transform.GetChild(this.transform.childCount - 1).GetComponent<SplineCurve>();
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
        Transform endPointBeam = this.transform.GetChild(this.transform.childCount - 1).GetChild(0).transform;

        float positionOfObject = (Vector3.Dot(puzzleObject.transform.position, endPointBeam.transform.right));      
        float positionOfBeam = (Vector3.Dot(objectInfo.point, endPointBeam.transform.right));

        float right = (positionOfObject + colliderWidth);
        float left = (positionOfObject - colliderWidth);

        return ((positionOfBeam > right) || (positionOfBeam < left));
    }
    //if the picked up object is no longer is range for a collision
    private bool ShouldResizeBeam()
    {
        float positionOfObject = Vector3.Dot(objectInfo.point, this.transform.forward);

        return (!Mathf.Approximately(positionOfObject, newBeamEndPoint));
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;

        if ((lineBeam.active) && (this.transform.name.Contains("beep")))
        {
            //float maxDraw = lineBeam.beamLength * 2.0f;
            //Vector3 raycastStartLocation = this.transform.position;
   
            //int layerMask = ~(1 << LayerMask.NameToLayer("LightBeam") | 1 << LayerMask.NameToLayer("BeamLayer"));
            //Check if there has been a hit yet
            //float range = 1.0f;

            float raycastSize = 0.18f;
            float maxDraw = 3;
            
            Vector3 raycastStartLocation = this.transform.position;
            //int layerMask = ~(1 << LayerMask.NameToLayer("LightBeam") | 1 << LayerMask.NameToLayer("BeamLayer") | 1 << LayerMask.NameToLayer("PlayerLayer"));

            if (Physics.BoxCast(raycastStartLocation, new Vector3(raycastSize, raycastSize, raycastSize), this.GetComponent<Transform>().forward, out hit, this.transform.localRotation, maxDraw))
            {
                Debug.Log("?" + hit.transform.name);
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(transform.position, transform.forward * maxDraw);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(raycastStartLocation + transform.forward * maxDraw, new Vector3(raycastSize, raycastSize, raycastSize));
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(transform.position, transform.forward * maxDraw);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(raycastStartLocation + transform.forward * maxDraw, new Vector3(raycastSize, raycastSize, raycastSize));
            }
        }
    }
    */
}
