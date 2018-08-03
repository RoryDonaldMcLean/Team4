using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTriggerVFXResponse : MonoBehaviour
{
    //Sets up the VFX to show either the green colour, correct reponse
    //or the incorrect, black colour reponse.
    public void Initialize(Transform parent, bool correctColour)
    {
        this.transform.parent = parent;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (correctColour)
        {
            CorrectVFXSetup();
        }
        else
        {
            IncorrectVFXSetup();
        }

        Destroy(this.gameObject, 6.0f);
    }

    //Part of the VFX is not needed for the correct reponse
    private void CorrectVFXSetup()
    {
        Destroy(this.transform.GetChild(1).gameObject);
    }

    //Converts the colour of all the VFX parts to black colour 
    private void IncorrectVFXSetup()
    {
        ParticleSystem[] allVFXParts = this.transform.GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem vfx in allVFXParts)
        {
            ParticleSystem.MainModule particleMain = vfx.main;
            Color colour = Color.black;
            colour.a = 0.5f;
            particleMain.startColor = colour;
        }
    }
}
