using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player2Initalise : SCR_TradeLimb 
{
	protected override void LimbDetails()
	{
        //to be overwritten by inhertance

        Exchange("LeftArm", this.gameObject.tag);
        //Exchange("RightLeg", this.gameObject.tag);
        Exchange("RightArm", this.gameObject.tag);
        Exchange("LeftLeg", this.gameObject.tag);
        Exchange("RightLeg", this.gameObject.tag);

    }

}
