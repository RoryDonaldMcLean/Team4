using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbLight : MonoBehaviour
{
    public bool ArmsBox = false;

    private bool lightOn = true;
    private Color beamColour = Color.white;
    private LightRedirect lightRedirect;
    private GameObject limbJoint;
    // Use this for initialization
    void Start()
    {
        limbJointSetup();
    }

    public bool IsLimbAttached()
    {
        return (!limbJoint.name.Contains("Hinge"));
    }

    public void AttachLimbToLightBox(string playerTag)
    {
        string boxLimbType;
        if (ArmsBox)
        {
            boxLimbType = "Arm";
        }
        else
        {
            boxLimbType = "Leg";

            PlayerLimbColour(ref playerTag);
        }

        if (GameObject.FindGameObjectWithTag(playerTag).GetComponent<SCR_TradeLimb>().LimbLightGiveLimb(boxLimbType, limbJoint))
        {
            limbJointSetup();
        }
    }

    private void PlayerLimbColour(ref string playerTag)
    {
        if (playerTag.Equals("Player1"))
        {
            beamColour = Color.red;
        }
        else
        {
            beamColour = Color.blue;
        }
    }

    public void RemoveLimbFromLightBox(string playerTag)
    {
        if (GameObject.FindGameObjectWithTag(playerTag).GetComponent<SCR_TradeLimb>().LimbLightTakeLimb(limbJoint))
        {
            limbJointSetup();
        }
    }

    private void limbJointSetup()
    {
        limbJoint = this.transform.GetChild(this.transform.childCount - 1).gameObject;
        LimbOnMode();
    }

    private void LimbOnMode()
    {
        lightOn = !lightOn;

        if (lightOn)
        {
            lightRedirect = this.gameObject.AddComponent<LightRedirect>();
            if (!ArmsBox)
            {
                lightRedirect.beamColourRedirectControl = false;
                lightRedirect.beamColour = beamColour;
            }
            //BeamFoundNearObject();
        }
        else
        {
            if (lightRedirect != null)
            {
                lightRedirect.TriggerExitFunction();
                Destroy(lightRedirect);
            }
        }
    }

    private void BeamFoundNearObject()
    {
        RaycastHit hit;
        float nearDistance = 1.0f;

        CheckDirectionForLightBeam(this.transform.forward, out hit, ref nearDistance);
        CheckDirectionForLightBeam(-this.transform.forward, out hit, ref nearDistance);
        CheckDirectionForLightBeam(this.transform.right, out hit, ref nearDistance);
        CheckDirectionForLightBeam(-this.transform.right, out hit, ref nearDistance);
    }

    private void CheckDirectionForLightBeam(Vector3 direction, out RaycastHit hit, ref float nearDistance)
    {
        if(RayCast(direction, nearDistance, out hit))
        {
            //lightRedirect.TriggerEnterFunction(hit.collider);
            //Debug.Log(hit.transform.name);
        }
    }

    private bool RayCast(Vector3 direction, float length, out RaycastHit hit)
    {
        Vector3 offsetPos = (-direction * 1.1f);
        int layermask = 1 << LayerMask.NameToLayer("BeamLayer");
        Vector3 pos = this.GetComponent<Transform>().position;
        pos += offsetPos;

        Debug.DrawRay(pos, direction, Color.red, length);
        return Physics.BoxCast(pos, this.GetComponent<Transform>().localScale, direction, out hit, this.GetComponent<Transform>().rotation, length, layermask);
    }
}

