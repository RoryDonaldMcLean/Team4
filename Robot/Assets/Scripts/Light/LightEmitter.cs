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

    void Start()
    {
        lineBeam = this.gameObject.AddComponent<StraightSplineBeam>();
        lineBeam.beamColour = colouredBeam;
        lineBeam.beamLength = beamLength;
        beamLength = beamLength * 2;
        if (!switchedOn) ToggleLight();
		AkSoundEngine.PostEvent("Drone", gameObject);

    }

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
               // AkSoundEngine.PostEvent("Drone_Stop", gameObject);
            }
        }
    }

    public void TurnOffForGood()
    {
        switchedOn = false;
        ToggleLight();
        canBeTurnedOff = false;
       // AkSoundEngine.PostEvent("Drone_Stop", gameObject);
        AkSoundEngine.SetState("Environment", "Empty");
    }

    private void ToggleLight()
    {
        lineBeam.ToggleBeam();
        if(switchedOn) WaitForBeamDestruction();
    }

    private void WaitForBeamDestruction()
    {
        StartCoroutine(CollisionControl());
    }

    private IEnumerator CollisionControl()
    {
        yield return StartCoroutine(lineBeam.BeamNotification());
        CollisionCheck();
    }

    private void CollisionCheck()
    {
        RaycastHit hit;
        if (ObjectFoundInfrontOfBeam(out hit))
        {
            this.GetComponent<LightResize>().TriggerExitControl(hit.transform);
        }
    }

    private bool ObjectFoundInfrontOfBeam(out RaycastHit hit)
    {
        Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward, Color.green, lineBeam.beamLength * 2);
        return Physics.BoxCast(this.GetComponent<Transform>().position, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, lineBeam.beamLength * 2);
    }
}
