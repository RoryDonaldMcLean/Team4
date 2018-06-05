using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player1Initalise : SCR_TradeLimb 
{
<<<<<<< HEAD

	public Transform[] Hinges;

	public GameObject leftArmPrefab;
	public GameObject LimbArea;
	private GameObject leftArm;

	// Use this for initialization
	void Awake () 
=======
	protected override void LimbDetails()
>>>>>>> master
	{
		//to be overwritten by inhertance
		//give player1 the arms to start with
		Exchange("LeftArm", this.gameObject.tag);
		Exchange ("RightArm", this.gameObject.tag);

	}
}
