using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTest : WeightCheck
{
    //in the testbed room, since one is the max weight, it is used as the test weight requirement
    int requiredWeight = 1;

	//temp way of checking to see if the button has been pressed
	public bool pressed = false;

    protected override void WeightResponse()
    {
		if (playersWeight >= requiredWeight)
		{
			//some time delay for dramatic effect
			Invoke ("movePlate", 1.0f);
			pressed = true;
			//movePlate();
		} 
		else if (playersWeight < requiredWeight)
		{
			Invoke ("movePlate", 1.0f);
			InvokeRepeating("moveAlittle", 0.0f, 0.1666f);
			pressed = false;
		}
    }

    //allows the plate to floor to the floor, using Unity physics components 
    private void movePlate()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().isKinematic = false;

    }

	private void moveAlittle()
	{
		if (this.transform.position.y <= -0.52f)
		{
			Debug.Log ("physics sucks");
			this.transform.position = new Vector3(this.transform.position.x, -0.52f, this.transform.position.z);
			this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.GetComponent<Rigidbody>().isKinematic = true;
			CancelInvoke ("moveAlittle");
		}

	}


}
