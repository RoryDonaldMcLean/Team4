using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    public Color colouredBeam;
    private StraightSplineBeam lineBeam;
    public int beamLength;
    public bool switchedOn = false;
    public bool canBeTurnedOff = true;

    //Sets up a lightbean object with its colour and size specified from public variables.
    //This allows for quick customised emitters to be made, using prefabs to duplicate them.
    //The length is doubled internally to make the length match the desired value, to fix
    //an issue with the bezier curve math setup halving the length. 
    void Start()
    {
        lineBeam = this.gameObject.AddComponent<StraightSplineBeam>();
        lineBeam.beamColour = colouredBeam;
        lineBeam.beamLength = beamLength;
        beamLength = beamLength * 2;
        if (!switchedOn)
        {
            ToggleLight();
        }
        else
        {
            AkSoundEngine.PostEvent("Drone", gameObject);
        }
    }

    //This function is called when the player hits the interact button on the emitter,
    //calls the toggle beam functionality, to either then turn off or turn on the beam.
    //A check is performed to make sure that before the toggle occurs, that this object
    //is allowed to have player interaction, to be allowed to be toggled.
    public void InteractWithEmitter()
    {
        switchedOn = !switchedOn;
        if (canBeTurnedOff)
        {
            ToggleLight();

            if (switchedOn)
            {
                //turn on 
                AkSoundEngine.PostEvent("Drone", gameObject);
            }
            else
            {
                //turn off
                AkSoundEngine.SetState("Drone_Modulator", "Off");
            }
        }
    }

    //Called when the puzzle and the subsequent light show had been completed
    //disables the ability for the lights to be turned on, while turning them off as well.  
    public void TurnOffForGood()
    {
        switchedOn = false;
        ToggleLight();
        canBeTurnedOff = false;
       // AkSoundEngine.PostEvent("Drone_Stop", gameObject);
        AkSoundEngine.SetState("Environment", "Empty");
    }

    //A simple toggle control that allows for the lightbeam to be turned on and off.
    private void ToggleLight()
    {
        lineBeam.ToggleBeam();
        if(switchedOn) WaitForBeamDestruction();
    }

    private void WaitForBeamDestruction()
    {
        StartCoroutine(CollisionControl());
    }

    //A simply coroutine that waits for the newly created beam to finish its initialization
    //before then running a collision check for any objects.  
    private IEnumerator CollisionControl()
    {
        yield return StartCoroutine(lineBeam.BeamNotification());
        CollisionCheck();
    }

    //Simply raycasts to find any objects infront of the beam, which, if found, then calls
    //a cleanup function that also resets that object. This method insures that on toggle of
    //a lightbeam, things are reset back to a normal start point before any interactions occur. 
    private void CollisionCheck()
    {
        RaycastHit hit;
        if (ObjectFoundInfrontOfBeam(out hit))
        {
            this.GetComponent<LightResize>().TriggerExitControl(hit.transform);
        }
    }

    //For the raycast, a boxcast is used in order to better find any objects that the beam may of
    //hit, as a raycast is far to narrow. For the boxcast, the width was calculated using the beams
    //natural width, so that they would match, and therefore, guarantee correct results where gained.
    private bool ObjectFoundInfrontOfBeam(out RaycastHit hit)
    {
        Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward, Color.green, lineBeam.beamLength * 2);
        return Physics.BoxCast(this.GetComponent<Transform>().position, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, lineBeam.beamLength * 2);
    }
}
