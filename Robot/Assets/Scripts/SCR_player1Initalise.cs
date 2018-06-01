using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player1Initalise : MonoBehaviour 
{

	public Transform[] Hinges;

	public GameObject leftArmPrefab;
	public GameObject LimbArea;
	private GameObject leftArm;

	// Use this for initialization
	void Awake () 
	{
		//at the start of the game have the left arm be on top of the first hinge.
		leftArm = (GameObject)Instantiate (leftArmPrefab, Hinges [0].position, Hinges [0].rotation);

		//make the left arm a child of the player
		leftArm.transform.parent = LimbArea.transform;

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
