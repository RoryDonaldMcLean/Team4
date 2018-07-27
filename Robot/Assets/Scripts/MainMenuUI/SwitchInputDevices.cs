using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchInputDevices : MonoBehaviour
{
    public GameObject cover;
    public bool isC;
    EventSystem es;
    GameObject canvas;
    // Use this for initialization
    void Start()
    {
        es = FindObjectOfType<EventSystem>();
        canvas = es.GetComponent<SwitchSelectController>().canvas;
        isC = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)) && !Cursor.visible)
        {
            es.SetSelectedGameObject(null);
            es.GetComponent<SwitchSelectController>().enabled = false;
            Cursor.visible = true;
            cover.SetActive(false);
            isC = false;
        }
        if (Input.GetButton("Submit") && Cursor.visible)
        {
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(canvas.GetComponentsInChildren<Button>()[0].gameObject);
            es.GetComponent<SwitchSelectController>().enabled = true;
            Cursor.visible = false;
            cover.SetActive(true);
            isC = true;
        }

        if (Cursor.visible && Input.GetMouseButtonDown(0))
        {
            es.SetSelectedGameObject(null);
        }
    }
}
