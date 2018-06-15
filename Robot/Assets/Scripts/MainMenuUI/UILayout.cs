using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UILayout : MonoBehaviour
{

    private bool settingClicked = false;
    private bool[] reboundButton = new bool[11];
    // Use this for initialization

    private void Awake()
    {
        //if (!File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.txt"))
        //{
        //    SaveNLoad.Save(GameManager.Instance.playerButtons);
        //}
        //GameManager.Instance.playerButtons = SaveNLoad.Load();
        //Debug.Log(GameManager.Instance.playerButtons.currentButton[0]);
    }

    void Start()
    {
        for (int i = 0; i < 11; i++)
        {
            reboundButton[i] = false;
        }

        if (!File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.txt"))
        {
            SaveNLoad.Save(GameManager.Instance.playerSetting);
        }
        GameManager.Instance.playerSetting = SaveNLoad.Load();
    }

    // Update is called once per frame
    void Update()
    {

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
        else if (reboundButton[keyNumber])
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
                    GameManager.Instance.playerSetting.currentButton[keyNumber] = Event.current.keyCode;
                    GameManager.Instance.playerSetting.sameButton(keyNumber);
                    SaveNLoad.Save(GameManager.Instance.playerSetting);
                }
            }
        }
    }

    private void geneLabelSlider(float x, float y)
    {
        GameManager.Instance.playerSetting.volumn = (int)GUI.HorizontalSlider(new Rect(x + 50, y, Screen.width - x - 50 - 100, 20), GameManager.Instance.playerSetting.volumn, 0.0f, 100.0f);
        GUI.Label(new Rect(x, y, 50, 20), GameManager.Instance.playerSetting.volumn.ToString());
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            SaveNLoad.Save(GameManager.Instance.playerSetting);
        }
        return;
    }

    private bool allFalse()
    {
        for (int i = 0; i < 11; i++)
        {
            if (reboundButton[i])
                return true;
        }
        return false;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 100, 200, 20), "Setting"))
        {
            if (!settingClicked)
                settingClicked = true;
            else
            {
                settingClicked = false;
                for (int i = 0; i < 11; i++)
                {
                    reboundButton[i] = false;
                }
            }
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

            GeneLabelButton(250 + 50, 40, "Forward", GameManager.Instance.playerSetting.currentButton[0].ToString(), 0);
            GeneLabelButton(250 + 50, 60, "Left", GameManager.Instance.playerSetting.currentButton[1].ToString(), 1);
            GeneLabelButton(250 + 50, 80, "Backward", GameManager.Instance.playerSetting.currentButton[2].ToString(), 2);
            GeneLabelButton(250 + 50, 100, "Right", GameManager.Instance.playerSetting.currentButton[3].ToString(), 3);
            GeneLabelButton(250 + 50, 120, "Emotion1", GameManager.Instance.playerSetting.currentButton[4].ToString(), 4);
            GeneLabelButton(250 + 50, 140, "Emotion2", GameManager.Instance.playerSetting.currentButton[5].ToString(), 5);
            GeneLabelButton(250 + 50, 160, "Emotion3", GameManager.Instance.playerSetting.currentButton[6].ToString(), 6);
            GeneLabelButton(250 + 50, 180, "Emotion4", GameManager.Instance.playerSetting.currentButton[7].ToString(), 7);
            GeneLabelButton(250 + 50, 200, "Jump", GameManager.Instance.playerSetting.currentButton[8].ToString(), 8);
            GeneLabelButton(250 + 50, 220, "Interact/Reattach", GameManager.Instance.playerSetting.currentButton[9].ToString(), 9);
            GeneLabelButton(250 + 50, 240, "Limbos Panel", GameManager.Instance.playerSetting.currentButton[10].ToString(), 10);

            if (!allFalse())
            {
                geneLabelSlider(250 + 50, 300);
                if(GUI.Button(new Rect(250 + 50, 330, 200,20), "Default"))
                {
                    for (int i = 0; i < 11; i++)
                    {
                        GameManager.Instance.playerSetting.currentButton[i] = GameManager.Instance.playerSetting.defaultButton[i];
                    }
                    SaveNLoad.Save(GameManager.Instance.playerSetting);
                }
            }

        }
    }
}
