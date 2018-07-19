using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour 
{

	public bool Player1enteredBounds = false;
	public bool Player2enteredBounds = false;

	GameObject GameController;

	// Use this for initialization
	void Start () 
	{
		GameController = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (Player1enteredBounds == true && Player2enteredBounds == true)
		{
			StopAllCoroutines();
			GameController.GetComponent<LevelController>().NextLevel ();
		}
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
}
