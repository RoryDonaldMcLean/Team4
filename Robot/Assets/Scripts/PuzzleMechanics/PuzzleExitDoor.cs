using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleExitDoor : MonoBehaviour
{
    private bool doorOpen = true;

    private Color correctLightBeamColour = Color.green;
    private Color incorrectLightBeamColour = Color.red;
    private Color currentColour;
    private Color fadedColour;

    private List<Renderer> switchRend = new List<Renderer>();
    // Use this for initialization
    void Start()
    {
        switchRend.Add(this.transform.GetChild(0).GetComponent<Renderer>());
        switchRend.Add(this.transform.GetChild(1).GetComponent<Renderer>());

        SwitchColourControl();
    }

    private void LightBlink()
    {
        CancelInvoke("ColourOverTime");
        InvokeRepeating("ColourOverTime", 0.3f, 1.00f);
    }

    private void ColourOverTime()
    {
        float fadeLength = 1.0f;
        Color lerpedColor = Color.Lerp(currentColour, fadedColour, Mathf.PingPong(Time.time, fadeLength));

        foreach (Renderer rend in switchRend)
        {
            rend.material.color = lerpedColor;
        }
    }

    private void SwitchColourControl()
    {
        doorOpen = !doorOpen;

        if(doorOpen)
        {
            currentColour = correctLightBeamColour;
        }
        else
        {
            currentColour = incorrectLightBeamColour;
        }

        fadedColour = currentColour / 2.0f;
        SwitchColour(ref currentColour);
    }

    private void SwitchColour(ref Color newColour)
    {
        foreach(Renderer rend in switchRend)
        {
            rend.material.color = newColour;
        }

        LightBlink();
    }

    public void OpenDoor()
    {
        if(!doorOpen)
        {
            SwitchColourControl();
        }
    }

    public void CloseDoor()
    {
        if (doorOpen)
        {
            SwitchColourControl();
        }
    }
}
