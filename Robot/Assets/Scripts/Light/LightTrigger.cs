﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    public Color correctLightBeamColour = Color.white;
    public bool doorOpen = false;

    public AudioClip Light;
    public AudioSource LightSource;


    //sets the door colour indicator to the correct defined colour required in order to open the door
    //an emissive level is added to better match the colour with the bright lightbeams 
    void Start()
    {
        this.transform.GetChild(0).GetComponent<Renderer>().material.color = correctLightBeamColour;
        this.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", (correctLightBeamColour*0.5f));

        LightSource.clip = Light;

    }

    //Upon a collison being detected with a Lightbeam 
    void OnTriggerEnter(Collider lightbeam)
    {
        LineRenderer line = lightbeam.GetComponentInParent<LineRenderer>();
        //checks to see if the predefined colour that is required to open this door, matches the lightbeams colour.
        if (CheckBeamColour(line.startColor))
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

    }
   
    private void OpenDoor()
    {
        if(!doorOpen)
        {
            Debug.Log("DOOROPEN!");
            doorOpen = true;
            LightSource.Play();

        }
    }

    private void CloseDoor()
    {
        if (doorOpen)
        {
            Debug.Log("DOORCLOSED!");
            doorOpen = false;
        }
    }

    //a simple colour comparision to indicate a match or not was found
    private bool CheckBeamColour(Color beamColour)
    {
        return (correctLightBeamColour.Equals(beamColour));
    }
}