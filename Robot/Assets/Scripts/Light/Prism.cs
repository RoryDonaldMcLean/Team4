using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightBeam)
    {
        this.transform.parent.GetComponent<LightSplitter>().OnEnter(lightBeam);
    }

    //Upon lightbeam leaving the door trigger
    void OnTriggerExit(Collider lightBeam)
    {
        this.transform.parent.GetComponent<LightSplitter>().OnExit();
    }
}
