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

	public bool isAllowedToTrigger;
	public bool Correct = false;

	GameObject GameController;

	public bool SpawnWalkway = false;

	// Use this for initialization
	void Start ()
	{
		GameController = GameObject.FindGameObjectWithTag ("GameController");
		AkSoundEngine.SetState ("Environment", "None");

		isAllowedToTrigger = true;
		Arrows = new Material[8];

		//get all the materials to be used for arrows
		Arrows [0] = Resources.Load<Material>("Materials/1") as Material;
		Arrows [1] = Resources.Load<Material>("Materials/2") as Material;
		Arrows [2] = Resources.Load<Material>("Materials/3") as Material;
		Arrows [3] = Resources.Load<Material>("Materials/4") as Material;

		Arrows [4] = Resources.Load<Material> ("Materials/5") as Material;
		Arrows [5] = Resources.Load<Material> ("Materials/6") as Material;
		Arrows [6] = Resources.Load<Material> ("Materials/7") as Material;
		Arrows [7] = Resources.Load<Material> ("Materials/8") as Material;

		for(int i = 0; i < 8; i++)
		{
			Arrows[i] = Resources.Load<Material>("Materials/" + (i+1)) as Material;
		}

		//loop through all the child objects of the door
		for (int i = 0; i < this.transform.childCount; i++)
		{
			//add each Panel it finds to the list of panels
			Panels.Add (this.transform.GetChild (i).gameObject);
		}

		//defined in the inspector

		for(int i = 0; i < Doorcode.Count; i++)
		{
			//setting the materials on the panels of the door to match the door code
			rend = Panels[i].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Resources.Load<Material>("Materials/" + Doorcode[i]);
		}
		/*
		for (int i = 0; i < Doorcode.Count; i++)
		{
			//setting the materials on the panels of the door to match the door code
			rend = Panels [0].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows[i] = Resources.Load<Material>("Materials/" + Doorcode[0]);

			rend = Panels [1].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[1]);

			rend = Panels [2].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[2]);

			rend = Panels [3].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[3]);



			rend = Panels [4].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[4]);

			rend = Panels [5].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[5]);

			rend = Panels [6].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[6]);

			rend = Panels [7].GetComponent<Renderer> ();
			rend.enabled = true;
			rend.sharedMaterial = Arrows [i] = Resources.Load<Material>("Materials/" + Doorcode[7]);

		}
*/

	}
	
	// Update is called once per frame
	void Update () 
	{
		//used for level transitions?
		if (Correct == true)
		{
			//Debug.Log ("don't you dare go hollow");
			//check to see what level your in and position the floor accordingly.
			//GameController.GetComponent<LevelController> ().NextLevel ();
			SpawnWalkway = true;
		}

	}

	void OnTriggerStay(Collider col)
	{
		if (isAllowedToTrigger == true)
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
	}

	void OnTriggerExit(Collider col)
	{
		if (isAllowedToTrigger == true)
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
}
