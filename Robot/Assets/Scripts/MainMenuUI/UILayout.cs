using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILayout : MonoBehaviour {

    private bool settingClicked = false;
    private bool[] reboundButton = new bool[11];
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 11; i++)
        {
            reboundButton[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GeneLabelButton(float x, float y, string labelString, string button, int keyNumber)
    {
        GUI.Label(new Rect(x, y, 100, 20), labelString);
        if (!allFalse())
        {
            if (GUI.Button(new Rect(x + 120, y, 100, 20), button))
            {
                reboundButton[keyNumber] = true;
            }
        }
        else if(reboundButton[keyNumber])
        {
            GUI.Label(new Rect(50, 200, 200, 50), "Press a button to rebound " + labelString + '\n' + "Press ESC to cancle");
            //GUI.Window(0, new Rect(0, 0, Screen.width, Screen.height), null, "Press a button to rebound" + '\n' + "Press ESC to cancle");
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                reboundButton[keyNumber] = false;
            }
           if (Input.anyKeyDown)
            {
                if (!Input.GetKey(KeyCode.Escape) && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                {
                    reboundButton[keyNumber] = false;
                    //Debug.Log(button+keyNumber);
                    GameManager.Instance.playerButtons.currentButton[keyNumber] = Event.current.keyCode;
                    GameManager.Instance.playerButtons.sameButton(Event.current.keyCode, keyNumber);
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

            GeneLabelButton(250 + 50, 40, "Forward", GameManager.Instance.playerButtons.currentButton[0].ToString(), 0);
            GeneLabelButton(250 + 50, 60, "Left", GameManager.Instance.playerButtons.currentButton[1].ToString(), 1);
            GeneLabelButton(250 + 50, 80, "Backward", GameManager.Instance.playerButtons.currentButton[2].ToString(), 2);
            GeneLabelButton(250 + 50, 100, "Right", GameManager.Instance.playerButtons.currentButton[3].ToString(), 3);

        }
    }

    private bool allFalse()
    {
        for(int i = 0; i < 11; i++)
        {
            if (reboundButton[i])
                return true;
        }
        return false;
    }
}
