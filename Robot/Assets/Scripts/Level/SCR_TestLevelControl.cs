using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TestLevelControl : LevelControlBaseClass 
{
	// Update is called once per frame
	void Update () 
	{
		//check if the first pressure plate as been pressed down
		//destroy the first door (temp)
		if (buttons[0].pressed == true)
		{
            //disable the first door
//			Debug.Log("agag");
            barriers[0].gameObject.SetActive(false);
		}

		//check to see if the roboCode is correct when played next to the melody puzzle
		//destroy the second door (temp)
		if (doors[0].GetComponent<SCR_Melody>().correctCode == true)
		{
            barriers[1].gameObject.SetActive (false);
		}

		//last door opens when the second pressure plate is lowered
		if (buttons [1].GetComponent<WeightCheck>().pressed == true)
		{
            barriers[2].gameObject.SetActive (false);
			barriers[3].gameObject.SetActive (false);
		}
	}
}
