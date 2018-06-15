using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player1Initalise : SCR_TradeLimb 
{
	protected override void LimbDetails()
	{
		//to be overwritten by inhertance
		//give player1 the arms to start with
		Exchange ("RightArm", this.gameObject.tag);

		Exchange ("LeftLeg", this.gameObject.tag);

		Exchange ("RightLeg", this.gameObject.tag);



	}
}
