using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBarrier : MonoBehaviour
{
    public bool inverseBlockProcess = false;
    public Color colourToAllow;
    private Color resultantColour;

    void Start()
    {
        Color transparentVersionOfColour = colourToAllow;
        transparentVersionOfColour.a = 0.5f;

        this.transform.GetComponent<Renderer>().material.color = transparentVersionOfColour;
        //this.transform.GetComponent<Renderer>().material.SetColor("_EmissionColor", (colourToBlock * 0.5f));
    }

    //Upon a collison being detected with a Lightbeam 
    public bool OnEnter(Color colour)
    {
        resultantColour = colour;

        if (inverseBlockProcess)
        {
            return BlockChosenColour();
        }
        else
        {
            return AllowOnlyChosenColour();
        }
    }

    private bool BlockChosenColour()
    {
        resultantColour = resultantColour - colourToAllow;
        resultantColour.a = 1.0f;

        return (resultantColour.Equals(new Color(0, 0, 0, 1)));
    }

    private bool AllowOnlyChosenColour()
    {
        return (!resultantColour.Equals(colourToAllow));
    }
}
