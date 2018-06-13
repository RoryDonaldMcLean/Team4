using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamOn : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player1")
        {
            target.SetActive(true);
        }

        if (col.gameObject.tag == "Player2")
        {
            target.SetActive(true);
        }

    }
}
