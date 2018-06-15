using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//allows you to see the contents of the list in the editor at runtime
[System.Serializable]
public class SCR_Door : MonoBehaviour 
{
	public bool Player1enteredBounds = false;
	public bool Player2enteredBounds = false;

	List<GameObject> Panels = new List<GameObject>();

	Renderer rend;
	Material[] Arrows;

	public List<int> Doorcode = new List<int> ();

	// Use this for initialization
	void Start ()
	{
		Arrows = new Material[4];
		//get all the materials to be used for arrows
		Arrows [0] = Resources.Load<Material>("Materials/UpArrow") as Material;
		Arrows [1] = Resources.Load<Material>("Materials/LeftArrow") as Material;
		Arrows [2] = Resources.Load<Material> ("Materials/RightArrow") as Material;
		Arrows [3] = Resources.Load<Material> ("Materials/DownArrow") as Material;

		//loop through all the child objects of the door
		for (int i = 0; i < this.transform.childCount; i++)
		{
			//add each Panel it finds to the list of panels
			Panels.Add (this.transform.GetChild (i).gameObject);
		}

		//code for the door (1,2,3,4)

		//setting the materials on the panels of the door to match the door code
		rend = Panels [0].GetComponent<Renderer> ();
		rend.enabled = true;
		rend.sharedMaterial = Arrows [0];

		rend = Panels [1].GetComponent<Renderer> ();
		rend.enabled = true;
		rend.sharedMaterial = Arrows [1];

		rend = Panels [2].GetComponent<Renderer> ();
		rend.enabled = true;
		rend.sharedMaterial = Arrows [2];

		rend = Panels [3].GetComponent<Renderer> ();
		rend.enabled = true;
		rend.sharedMaterial = Arrows [3];
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Player1")
		{
			Player1enteredBounds = true;
		}

		if (col.gameObject.tag == "Player2")
		{
			Player2enteredBounds = true;
		}

	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player1")
		{
			Player1enteredBounds = false;
		}


		if (col.gameObject.tag == "Player2")
		{
			Player2enteredBounds = false;
		}
	}
}
