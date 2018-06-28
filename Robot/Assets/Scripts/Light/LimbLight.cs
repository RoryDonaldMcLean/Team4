﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbLight : MonoBehaviour
{
    public bool ArmsBox = false;
    public int beamLength = 5;

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
            lightRedirect.beamLength = beamLength;
            if (!ArmsBox)
            {
                lightRedirect.beamColourRedirectControl = false;
                lightRedirect.beamColour = beamColour;
            }
            BeamFoundNearObject();
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
    }

    private void CheckDirectionForLightBeam(Vector3 direction, out RaycastHit hit, ref float nearDistance)
    {
        if(RayCast(direction, nearDistance, out hit))
        {
            lightRedirect.TriggerEnterFunction(hit.collider);
            Debug.Log("!" + hit.transform.name);
        }
    }

    private bool RayCast(Vector3 direction, float length, out RaycastHit hit)
    {
        int layerMask = 1 << LayerMask.NameToLayer("LightBeam");
        Vector3 offsetPos = Vector3.Scale(direction, this.GetComponent<Transform>().localScale);

        Vector3 raycastStartLocation = this.transform.position;
        raycastStartLocation -= offsetPos * 2.0f;

        Debug.DrawRay(raycastStartLocation, direction, Color.red, length);
        return Physics.BoxCast(raycastStartLocation, this.GetComponent<Transform>().localScale, direction, out hit, this.GetComponent<Transform>().rotation, length, layerMask);
    }
    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;

        float nearDistance = 1.0f;
        if (lightRedirect != null)
        {
            float maxDraw = 1.0f;
            Vector3 direction = this.transform.forward;
            Vector3 offsetPos = Vector3.Scale(direction, this.GetComponent<Transform>().localScale);

            Vector3 raycastStartLocation = this.transform.position;
            raycastStartLocation -= offsetPos * 2.0f;

            int layerMask = 1 << LayerMask.NameToLayer("LightBeam");
            //Check if there has been a hit yet
            float range = 1.0f;
            if (Physics.BoxCast(raycastStartLocation, new Vector3(range, range, range), direction, out hit, this.transform.localRotation, maxDraw, layerMask))
            {
                Debug.Log("?" + hit.transform.name);
                //Draw a Ray forward from GameObject toward the hit
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                //Draw a cube that extends to where the hit exists
                Gizmos.DrawWireCube(raycastStartLocation + direction * nearDistance, new Vector3(range, range, range));
            }
            //If there hasn't been a hit yet, draw the ray at the maximum distance
            else
            {
                //Draw a Ray forward from GameObject toward the maximum distance
                Gizmos.DrawRay(transform.position, transform.forward * nearDistance);
                //Draw a cube at the maximum distance
                Gizmos.DrawWireCube(raycastStartLocation + direction * nearDistance, new Vector3(range, range, range));
            }
        }
    }
    */
}

