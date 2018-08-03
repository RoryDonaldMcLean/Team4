﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_player2Initalise : SCR_TradeLimb 
{
	int levelCounter;

	protected override void LimbDetails()
	{
        PickUpInit();

        levelCounter = GameObject.FindGameObjectWithTag ("GameController").GetComponent<LevelController>().currentLevel;

        //to be overwritten by inhertance
		//change the total number of limbs needed at the start of the level
		if (levelCounter == 0)
		{
			//tutorial level
			Exchange ("LeftArm", this.gameObject.tag, true);
			Exchange ("LeftLeg", this.gameObject.tag, true);
			Exchange ("RightLeg", this.gameObject.tag, true);
		} 
		else if (levelCounter == 1)
		{
			//factory part 1
			Exchange ("LeftArm", this.gameObject.tag, true);
			Exchange ("RightArm", this.gameObject.tag, true);
			Exchange ("LeftLeg", this.gameObject.tag, true);
			Exchange ("RightLeg", this.gameObject.tag, true);
		} 
		else if (levelCounter == 2)
		{
			//factory part 2
			Exchange ("LeftArm", this.gameObject.tag, true);
			Exchange ("RightArm", this.gameObject.tag, true);
			Exchange ("LeftLeg", this.gameObject.tag, true);
		} 
		else if (levelCounter == 3)
		{
			//factory hights
			Exchange ("LeftLeg", this.gameObject.tag, true);
			Exchange ("LeftArm", this.gameObject.tag, true);

		} 
		else if (levelCounter == 4)
		{
			//tower
			Exchange ("LeftLeg", this.gameObject.tag, true);
<<<<<<< HEAD
		}
=======
        }

>>>>>>> Development-John-27.07.18V2
    }

    private void PickUpInit()
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Player/pickup"));
        go.GetComponent<PickupAndDropdown_Trigger>().isBlue = true;
        go.GetComponent<PickupAndDropdown_Trigger>().playerNum = 1;
        go.transform.parent = this.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
