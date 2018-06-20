using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Movable : MonoBehaviour 
{
	public bool pickedUp = false;
	public string playerTag;
	public string movableObjectString;

	GameObject pole;
	Vector3 MaxDistance;
	Vector3 MinDistance;

	public bool Entered = false;

	// Use this for initialization
	void Start () 
	{
		pole = this.gameObject.transform.GetChild (0).gameObject;
		//get the far right and far left bounds of the pole object
		MaxDistance = pole.GetComponent<Collider> ().bounds.max;
		MinDistance = pole.GetComponent<Collider> ().bounds.min;

		//on start, create a new movable object determined by the string in the editor
		GameObject newMovable = Instantiate (Resources.Load ("Prefabs/Light/" + movableObjectString)) as GameObject;

		//force it's name to be "SlideBox"
		newMovable.name = "SlideBox";

		//set it's position to be the same as the placeholder slideBox
		newMovable.transform.position = this.transform.GetChild (1).position;

		//become a child of the "Movable" object
		newMovable.transform.SetParent(this.transform);

		//delete the placeholderbox
		Destroy(this.transform.GetChild(1).gameObject);

	}

	//the far right of the pole
	private bool RightLimit()
	{
		Vector3 movableObjectposition = this.transform.GetChild(1).position;
		return (movableObjectposition.x >= MaxDistance.x);
	}

	//the far left of the pole
	private bool LeftLimit()
	{
		Vector3 movableObjectposition = this.transform.GetChild (1).position;
		return (movableObjectposition.x <= MinDistance.x);
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
				Debug.Log ("right limit");
			}
			else if (LeftLimit ())
			{
				Vector3 position = this.transform.GetChild (1).position;
				position.x = MinDistance.x;
				GameObject.FindGameObjectWithTag (playerTag).GetComponent<PickupAndDropdown> ().LimitDrop ();
				Debug.Log ("left limit");
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
