using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBarrier : MonoBehaviour
{
    public bool inverseBlockProcess = false;
    public Color colourToAllow;
    private Color resultantColour;

    //Converts the light barrier's material colour to match the colour 
    //specified in the editor. This provides a simple and effective way
    //to chose and edit the colour a particular instance of light barrier
    //is supposed to block. 
    void Start()
    {
        Color transparentVersionOfColour = colourToAllow;
        transparentVersionOfColour.a = 0.5f;

        this.transform.GetComponent<Renderer>().material.color = transparentVersionOfColour;
    }

    //When a collision is detected a check is performed to see of the colour
    //should be blocked or not. If the barrier is an inverse or otherwise
    //the type of check is different. This result is then used in the 
    //light resize script to process what happens next with the lightbeam.
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

    //The colour of the lightbeam and the light barrier's colour
    //when removed should result in a zeroed out result, if
    //both colours were the same.   
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
