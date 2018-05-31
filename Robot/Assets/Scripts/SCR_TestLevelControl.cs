using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_TestLevelControl : MonoBehaviour 
{

	public GameObject[] doors;

	public GameObject[] PressurePlates;
	public GameObject EventSystem_;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check if the first pressure plate as been pressed down
		//destroy the first door (temp)
		if (PressurePlates[0].GetComponent<WeightCheckNew> ().pressed == true)
		{
			//disable the first door
			doors [0].gameObject.SetActive (false);
		}

		//check to see if the roboCode is correct when played next to the melody puzzle
		//destroy the second door (temp)
		if (EventSystem_.GetComponent<SCR_Melody> ().correctCode == true)
		{
			doors [1].gameObject.SetActive (false);
		}

		//last door opens when the second pressure plate is lowered
		if (PressurePlates [1].GetComponent<WeightCheckNew> ().pressed == true)
		{
			doors [2].gameObject.SetActive (false);
		}


	}
}
