using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    public Color colouredBeam;
    private StraightSplineBeam lineBeam;
    public int beamLength;
    public bool switchedOn = false;

    void Start()
    {
        lineBeam = this.gameObject.AddComponent<StraightSplineBeam>();
        lineBeam.beamColour = colouredBeam;
        lineBeam.beamLength = beamLength;
        beamLength = beamLength * 2;
        if (!switchedOn) ToggleLight();
    }

    public void ToggleLight()
    {
        lineBeam.ToggleBeam();

        //RaycastHit hit;
        //if(ObjectFoundInfrontOfBeam(out hit))
        {
            //TriggerExitControl(hit.transform);
        }
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
        }
    }

    private bool ObjectFoundInfrontOfBeam(out RaycastHit hit)
    {
        Debug.DrawRay(this.GetComponent<Transform>().position, this.GetComponent<Transform>().forward, Color.red, beamLength);
        return Physics.BoxCast(this.GetComponent<Transform>().position, this.GetComponent<Transform>().localScale, this.GetComponent<Transform>().forward, out hit, this.GetComponent<Transform>().rotation, beamLength);
    }
}
