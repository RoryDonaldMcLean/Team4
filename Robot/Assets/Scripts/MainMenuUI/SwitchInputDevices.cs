using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchInputDevices : MonoBehaviour {
    EventSystem es;
    // Use this for initialization
    void Start () {
         es = FindObjectOfType<EventSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            es.SetSelectedGameObject(null);
            es.GetComponent<SwitchSelectController>().enabled = false;
            Cursor.visible = true;
        }
        if(Input.GetButton("Submit"))
        {
            es.SetSelectedGameObject(GameObject.FindObjectOfType<Button>().gameObject);
            es.GetComponent<SwitchSelectController>().enabled = true;
            Cursor.visible = false;
        }
    }
}
