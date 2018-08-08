using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboardtest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.D))
        {
            Debug.Log("D");
        }
        if(Input.GetKey(KeyCode.W))
        {
            Debug.Log("W");
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up");
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Right");
        }
	}
}
