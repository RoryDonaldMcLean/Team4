using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLevelOneControl : LevelControlBaseClass
{
   void Update()
    {
        if(lightDoors[0].GetComponent<LightTrigger>().correctLight)
        {
            Debug.Log("LEVEL DONE");
        }
    }
}
