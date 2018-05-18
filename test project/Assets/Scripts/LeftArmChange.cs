using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmChange : MonoBehaviour {


    public Rigidbody player1, player2;
    public Transform player1t, player2t;
    private Component jointComponent = null;
    SoftJointLimitSpring test;
    // Use this for initialization
    void Start () {
        test.damper = 1000.0f;
        test.spring = 0;
        this.transform.position = player1t.position;
        jointComponent = this.gameObject.AddComponent<CharacterJoint>();
        Debug.Log(this.GetComponent<CharacterJoint>());
        this.GetComponent<CharacterJoint>().connectedBody = player1;
        this.GetComponent<CharacterJoint>().autoConfigureConnectedAnchor = false;
        this.GetComponent<CharacterJoint>().anchor = new Vector3(0, 0.77f, 0);
        this.GetComponent<CharacterJoint>().axis = new Vector3(0, 1, 0);
        this.GetComponent<CharacterJoint>().connectedAnchor = new Vector3(0.71f, 0.53f, 0);
        this.GetComponent<CharacterJoint>().swingAxis = new Vector3(1, 0, 0);
        this.GetComponent<CharacterJoint>().swingLimitSpring = test;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Destroy(this.GetComponent<CharacterJoint>());
            this.transform.position = player2t.position;
            //jointComponent = this.gameObject.AddComponent<CharacterJoint>();
            Debug.Log(this.GetComponent<CharacterJoint>());
            this.GetComponent<CharacterJoint>().connectedBody = player2;
            this.GetComponent<CharacterJoint>().autoConfigureConnectedAnchor = false;
            this.GetComponent<CharacterJoint>().anchor = new Vector3(0, 0.77f, 0);
            this.GetComponent<CharacterJoint>().connectedAnchor = new Vector3(0.71f, 0.53f, 0);
            this.GetComponent<CharacterJoint>().swingAxis = new Vector3(1, 0, 0);
            this.GetComponent<CharacterJoint>().axis = new Vector3(0, 1, 0);
            this.GetComponent<CharacterJoint>().swingLimitSpring = test;
        }
    }
}
