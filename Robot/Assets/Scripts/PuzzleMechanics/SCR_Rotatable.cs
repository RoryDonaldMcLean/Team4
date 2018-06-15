using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Rotatable : MonoBehaviour 
{
	public bool pickedUp = false;
	public string playerTag;
	public string rotatableObjectString;
	public bool Entered = false;

	// Use this for initialization
	void Start () 
	{
		GameObject newRotatable = Instantiate (Resources.Load ("Prefabs/Light/" + rotatableObjectString)) as GameObject;
		newRotatable.name = "RotateBox";
		newRotatable.transform.position = this.transform.GetChild (1).position;
		newRotatable.transform.SetParent (this.transform);
		Destroy (this.transform.GetChild (1).gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (pickedUp)
		{
			//if the player leaves the movable trigger box then drop the box thing
			if (Entered == false)
			{
				GameObject.FindGameObjectWithTag (playerTag).GetComponent<PickupAndDropdown> ().RotateDrop ();
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
