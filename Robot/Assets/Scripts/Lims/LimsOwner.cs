using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LimsOwners{ Player1, Player2};

public class LimsOwner : MonoBehaviour 
{

	public LimsOwners lim;

	//public Transform HingesP1;
	//public Transform HingesP2;


	//public GameObject LimbAreaP1, LimbAreaP2;


	void Start()
	{
		//HingesP1 = GameObject.Find ("Player/limb area/Hinge1").GetComponent<Transform>();
		//HingesP2 = GameObject.Find ("Player2/limb area/Hinge1").GetComponent<Transform>();

		//LimbAreaP1 = GameObject.Find ("Player/limb area");
		//LimbAreaP2 = GameObject.Find ("Player2/limb area");

	}
	public void limbTrade(Transform limbLocation, Transform parentTransform)
	{
		this.transform.position = limbLocation.position;

		this.transform.parent = parentTransform;
	}

	private void Update()
	{
		//if(this.lim == LimsOwners.Player2)
		//{
		//	this.transform.position = HingesP2.transform.position;
		//
		//	//make the left arm a child of the player
		//	this.transform.parent = LimbAreaP2.transform;
		//
		//
		//}
	}

}
