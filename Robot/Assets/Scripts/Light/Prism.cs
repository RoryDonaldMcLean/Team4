using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        if (lightBeam.gameObject.layer != LayerMask.NameToLayer("BeamLayer"))
        {
            if (this.transform.parent.name.Contains("LightSplitter"))
            {
                this.transform.parent.GetComponent<LightSplitter>().OnEnter(lightBeam);
            }
            else
            {
                this.transform.parent.GetComponent<PrismColourCombo>().OnEnter(lightBeam);
            }
        }
    }

    //Upon lightbeam leaving the door trigger
    void OnTriggerExit(Collider lightBeam)
    {
        if (this.transform.parent.name.Contains("LightSplitter"))
        {
            this.transform.parent.GetComponent<LightSplitter>().OnExit(lightBeam.transform.parent.parent);
        }
        else
        {
            this.transform.parent.GetComponent<PrismColourCombo>().OnExit(lightBeam);
        }
    }
}
