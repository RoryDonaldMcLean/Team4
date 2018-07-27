using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player1Initalise : SCR_TradeLimb 
{
	int levelCounter;

	protected override void LimbDetails()
	{
		levelCounter = GameObject.FindGameObjectWithTag ("GameController").GetComponent<LevelController>().currentLevel;

		//change the total number of limbs needed at the start of the level
		if (levelCounter == 0)
		{
			//tutorial level
			Exchange ("RightArm", this.gameObject.tag, true);
			Exchange ("LeftLeg", this.gameObject.tag, true);
			Exchange ("RightLeg", this.gameObject.tag, true);
		} 
		else if (levelCounter == 1)
		{
			//factory part 1
			Exchange ("RightArm", this.gameObject.tag, true);
			Exchange ("LeftLeg", this.gameObject.tag, true);
			Exchange ("RightLeg", this.gameObject.tag, true);
		} 
		else if (levelCounter == 2)
		{
			//factory part 2
			Exchange ("RightArm", this.gameObject.tag, true);
			Exchange ("LeftLeg", this.gameObject.tag, true);
			Exchange ("RightLeg", this.gameObject.tag, true);
		} 
		else if (levelCounter == 3)
		{
			//factory hights
			Exchange ("RightArm", this.gameObject.tag, true);
			Exchange ("RightLeg", this.gameObject.tag, true);

		} 
		else if (levelCounter == 4)
		{
			//tower
			Exchange ("RightArm", this.gameObject.tag, true);
		}


    }
	
}
