using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using InControl;

public class SwitchSelectController : MonoBehaviour {

    public GameObject selectObj;
    public GameObject canvas;
    
	// Use this for initialization
	void OnEnable () {
        this.GetComponent<EventSystem>().firstSelectedGameObject = canvas.GetComponentInChildren<Button>().gameObject;
        selectObj = this.GetComponent<EventSystem>().currentSelectedGameObject;
    }

    void OnDisable()
    {
         
    }
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<EventSystem>().currentSelectedGameObject != selectObj)
        {
            if (this.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {
                this.GetComponent<EventSystem>().SetSelectedGameObject(selectObj);
            }
            else
            {
                selectObj = this.GetComponent<EventSystem>().currentSelectedGameObject;
            }
        }
	}
}
