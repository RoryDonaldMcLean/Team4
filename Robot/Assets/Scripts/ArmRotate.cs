using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotate : MonoBehaviour {
    //float angle;
    public Transform parent;
	// Use this for initialization
	void Start () {
        //angle = 0;
        
	}
	
	// Update is called once per frame
	void Update () {
        parent = GetComponentsInParent<Transform>()[1];
        this.transform.position = parent.position + new Vector3(0.66f, 0.374f, 0);
        //angle += 0.001f;
        //this.transform.RotateAround(this.transform.position, this.transform.right, angle);

        //this.transform.parent = other;
	}
}
