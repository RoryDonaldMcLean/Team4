using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
    public Transform P;
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
        P.position = this.transform.position + new Vector3(0.62f, 0.37f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.01f);

        
	}
}
