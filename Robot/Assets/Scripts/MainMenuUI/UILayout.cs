using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILayout : MonoBehaviour {

    private bool settingClicked = false;
    private bool reboundButton = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void geneLabelButton(float x, float y, string labelString, string button, int keyNumber)
    {
       
        GUI.Label(new Rect(x, y, 100, 20), labelString);
        if (!reboundButton)
        {
            if (GUI.Button(new Rect(x + 120, y, 100, 20), button))
            {
                reboundButton = true;
            }
        }
        else
        {
            //GUI.Window(0, new Rect(0, 0, Screen.width, Screen.height), null, "Press a button to rebound" + '\n' + "Press ESC to cancle");
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                reboundButton = false;
            }
           if (Input.anyKeyDown)
            {
                if(!Input.GetKey(KeyCode.Escape))
                {
                    reboundButton = false; 
                    GameManager.Instance.playerButtons.currentButton[keyNumber] = Event.current.keyCode;
                }
            }
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 100, 200, 20), "Setting"))
        {
            if (!settingClicked)
                settingClicked = true;
            else
                settingClicked = false;
        }
        if (!settingClicked)
        {
            if (GUI.Button(new Rect(50, 50, 200, 20), "Start"))
            {
                SceneManager.LoadScene("Combined");
            }

            if (GUI.Button(new Rect(50, 150, 200, 20), "Quit"))
            {
                Application.Quit();
            }
        }
        else
        {
            float widthRemain = Screen.width - 250;
            GUI.Label(new Rect(250 + 50, 20, 200, 20), "Player1");
            GUI.Label(new Rect(250 + 50 + widthRemain / 2, 20, 200, 20), "Player2");

            geneLabelButton(250 + 50, 40, "Forward", GameManager.Instance.playerButtons.currentButton[0].ToString(), 0);
            if(reboundButton)
            {
                GUI.Label(new Rect(50, 200, 200, 50), "Press a button to rebound" + '\n' + "Press ESC to cancle");
            }
        }
    }
}
