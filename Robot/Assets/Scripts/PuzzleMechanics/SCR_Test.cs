using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Test : MonoBehaviour 
{
	public bool pickedUp = false;
	public string playerTag;
	GameObject pole;
	Vector3 MaxDistance;
	Vector3 MinDistance;

	public bool Entered = false;

	// Use this for initialization
	void Start () 
	{
		pole = this.gameObject.transform.GetChild (0).gameObject;
		MaxDistance = pole.GetComponent<Collider> ().bounds.max;
		MinDistance = pole.GetComponent<Collider> ().bounds.min;
	}

	//the far right of the pole
	private bool RightLimit()
	{
		Vector3 position = this.transform.GetChild(1).position;
		return (position.x >= MaxDistance.x);
	}

	//the far left of the pole
	private bool LeftLimit()
	{
		Vector3 position = this.transform.GetChild (1).position;
		return (position.x <= MinDistance.x);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (pickedUp)
		{
			//if the movable box thing reaches the left side or right side of the pole then drop it
			if (RightLimit())
			{
				//past its limit
				Vector3 position = this.transform.GetChild(1).position;
				position.x = MaxDistance.x;
				GameObject.FindGameObjectWithTag (playerTag).GetComponent<PickupAndDropdown>().LimitDrop();
			}
			else if (LeftLimit ())
			{
				Vector3 position = this.transform.GetChild (1).position;
				position.x = MinDistance.x;
				GameObject.FindGameObjectWithTag (playerTag).GetComponent<PickupAndDropdown> ().LimitDrop ();
			}

			//if the player leaves the movable trigger box then drop the box thing
			if (Entered == false)
			{
				GameObject.FindGameObjectWithTag (playerTag).GetComponent<PickupAndDropdown> ().LimitDrop ();
			}
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name.Contains("Player"))
		{
			Entered = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.name.Contains ("Player"))
		{
			Entered = false;
		}
	}
}
