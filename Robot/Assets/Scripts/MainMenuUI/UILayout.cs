using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class UILayout : MonoBehaviour
{

    private bool settingClicked = false;
    private bool[] reboundButton;
    // Use this for initialization

    private void Awake()
    {
        reboundButton = new bool[12];
        //if (!File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.txt"))
        //{
        //    SaveNLoad.Save(GameManager.Instance.playerButtons);
        //}
        //GameManager.Instance.playerButtons = SaveNLoad.Load();
        //Debug.Log(GameManager.Instance.playerButtons.currentButton[0]);
    }

    void Start()
    {
        //Debug.Log(GameManager.Instance.playerSetting.currentButton.Length);
        for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
        {
            reboundButton[i] = false;
        }

        if (!File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.setting"))
        {
            SaveNLoad.Save(GameManager.Instance.playerSetting);
        }
        //Debug.Log(GameManager.Instance.playerSetting.currentButton[11]);
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

    private int GeneLabelSlider(float x, float y, ref int volume)
    {
        //volume = GameManager.Instance.playerSetting.volume;
        GUI.Label(new Rect(x, y + 30, 100, 20), "Volume");
        volume = (int)GUI.HorizontalSlider(new Rect(x + 150, y + 30, Screen.width - x - 50 - 100 - 100, 20), volume, 0.0f, 100.0f);
        GUI.Label(new Rect(x + 100, y + 30, 50, 20), volume.ToString());
        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
            && GameManager.Instance.playerSetting.volume != volume)
        {
            SaveNLoad.Save(GameManager.Instance.playerSetting);
        }
        return volume;
    }

    private bool allFalse()
    {
        for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
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
                for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
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
            float heightRemain = Screen.height - 40 - 140;
            int amount = GameManager.Instance.playerSetting.currentButton.Length;
            GUI.Label(new Rect(250 + 50, 20, 200, 20), "Player1");
            GUI.Label(new Rect(250 + 50 + widthRemain / 2, 20, 200, 20), "Player2 -> Controller");


            if (!GameManager.Instance.playerSetting.player1Controller)
            {
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 1, "Forward", GameManager.Instance.playerSetting.currentButton[0].ToString(), 0);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 2, "Left", GameManager.Instance.playerSetting.currentButton[1].ToString(), 1);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 3, "Backward", GameManager.Instance.playerSetting.currentButton[2].ToString(), 2);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 4, "Right", GameManager.Instance.playerSetting.currentButton[3].ToString(), 3);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 5, "Emotion1", GameManager.Instance.playerSetting.currentButton[4].ToString(), 4);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 6, "Emotion2", GameManager.Instance.playerSetting.currentButton[5].ToString(), 5);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 7, "Emotion3", GameManager.Instance.playerSetting.currentButton[6].ToString(), 6);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 8, "Emotion4", GameManager.Instance.playerSetting.currentButton[7].ToString(), 7);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 9, "Jump", GameManager.Instance.playerSetting.currentButton[8].ToString(), 8);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 10, "Interact/Reattach", GameManager.Instance.playerSetting.currentButton[9].ToString(), 9);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 11, "Limbos Panel", GameManager.Instance.playerSetting.currentButton[10].ToString(), 10);
                GeneLabelButton(250 + 50, 60 + heightRemain / (amount + 2) * 12, "Dunno What Is Used For", GameManager.Instance.playerSetting.currentButton[11].ToString(), 11);
            }
            if (!allFalse())
            {
                bool player1Controller = GameManager.Instance.playerSetting.player1Controller;
                GameManager.Instance.playerSetting.player1Controller = GUI.Toggle(new Rect(380, 20, 100, 30), GameManager.Instance.playerSetting.player1Controller, "Controller");

                if (player1Controller != GameManager.Instance.playerSetting.player1Controller)
                {
                    SaveNLoad.Save(GameManager.Instance.playerSetting);
                }

                GeneLabelSlider(250 + 50, Screen.height - 140, ref GameManager.Instance.playerSetting.volume);
                if (GUI.Button(new Rect(250 + 50, Screen.height - 40, 200, 20), "Default"))
                {
                    for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
                    {
                        GameManager.Instance.playerSetting.currentButton[i] = GameManager.Instance.playerSetting.defaultButton[i];
                    }
                    GameManager.Instance.playerSetting.volume = 50;
                    GameManager.Instance.playerSetting.player1Controller = false;
                    SaveNLoad.Save(GameManager.Instance.playerSetting);
                }
            }

        }
    }
}
