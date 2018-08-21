using System.Collections;
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
    private string limbOwner;

    //Finds the limb object, and runs a quick setup procedure
    //Since its always a hinge at the start, this simply acts
    //as a method to obtain the transform of the hinge, which
    //is needed.
    void Start()
    {
        LimbJointSetup();
    }

    //A check that looks at the limb object and checks to make
    //sure its not a hinge, which therefore, would mean its a
    //limb object. 
    public bool IsLimbAttached()
    {
        return (!limbJoint.name.Contains("Hinge"));
    }

    //When called this function takes the players tag in order
    //to identify which player it is, allowing limb removal
    //code to be correctly used to the right player.
    //A check is also performed in order to indicate if its a
    //arm or leg limb box, which changes not only what limbs
    //it accepts, but also if it should also change the beams
    //colour.
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
            LimbJointSetup();
            limbOwner = playerTag;
        }
    }

    //Called when its a leg limb box, this changes the beams colour,
    //with the colour chosen depending on which player it is.
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

    //When interacted with when a limb is already attached, this 
    //function is called, removing the limb from the specfic player.
    //Then the limb box resets once more.
    public void RemoveLimbFromLightBox(string playerTag)
    {
        if (GameObject.FindGameObjectWithTag(playerTag).GetComponent<SCR_TradeLimb>().LimbLightTakeLimb(limbJoint))
        {
            LimbJointSetup();
            limbOwner = "Reset";
        }
    }

    public void ReturnLimbsToPlayer()
    {
        if(IsLimbAttached())
        {
            GameObject.FindGameObjectWithTag(limbOwner).GetComponent<SCR_TradeLimb>().ResetLimbsFromLimbBoxes(limbJoint);
        }
    }

    //Finds the hinge/limb object and sets up the box to use the correct
    //response to what ever is attached. 
    private void LimbJointSetup()
    {
        limbJoint = this.transform.GetChild(this.transform.childCount - 1).gameObject;
        LimbOnMode();
    }

    //Toggles if the light redirect code should be ran or not, as the limbs
    //toggle from hinge to limb accordingly. Also a beam raycast is also
    //performed when the object becomes a limb attached unit, to see it 
    //there is already a beam hitting the object back when it was just a
    //hinge attached.  
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
            CheckDirectionForLightBeam();
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
            lightRedirect.TriggerEnterFunction(hit.collider);
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
}

