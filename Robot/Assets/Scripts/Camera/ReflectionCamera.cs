using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mainCamera = Camera.main.GetComponent<Transform>().position;
        Vector3 mainCamRot = Camera.main.GetComponent<Transform>().rotation.eulerAngles;
        this.GetComponent<Transform>().position = new Vector3(mainCamera.x, -mainCamera.y, mainCamera.z);
        this.GetComponent<Transform>().rotation = Quaternion.Euler(-mainCamRot.x, mainCamRot.y, mainCamRot.z);

    }
}
