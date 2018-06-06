using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player1Initalise : SCR_TradeLimb 
{
	//public Transform[] Hinges;

	//public GameObject leftArmPrefab;
	//public GameObject LimbArea;
	//private GameObject leftArm;

	protected override void LimbDetails()

	{
		//to be overwritten by inhertance
		//give player1 the arms to start with
		Exchange("LeftArm", this.gameObject.tag);
		Exchange ("RightArm", this.gameObject.tag);

        //Exchange("LeftLeg", this.gameObject.tag);
        Exchange("RightLeg", this.gameObject.tag);
    }
}
