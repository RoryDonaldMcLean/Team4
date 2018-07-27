using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    private bool isSetting = false;
    public List<Button> mainMenuBtn;

    public GameObject settingArea, chapterArea;

    // Use this for initialization
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.setting"))
        {
            SaveNLoad.Save(GameManager.Instance.playerSetting);
        }
        //Debug.Log(GameManager.Instance.playerSetting.currentButton[11]);
        GameManager.Instance.playerSetting = SaveNLoad.Load();
        GameManager.Instance.LevelToGo = "SceneOne";

        foreach (Button btn in this.GetComponentsInChildren<Button>(true))
        {
            mainMenuBtn.Add(btn);
            btn.onClick.AddListener(delegate
            {
                OnClick(btn);
            });
        }

        if(GameManager.Instance.playerSetting.lastLevel == Level.None)
        {
            mainMenuBtn[1].enabled = false;
        }
        else
        {
            mainMenuBtn[1].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnClick(Button btn)
    {
        if (btn == mainMenuBtn[0])
        {
            SceneManager.LoadScene("SceneOne");
        }
        if (btn == mainMenuBtn[3])
        {
            if (!isSetting)
            {
                isSetting = true;
                mainMenuBtn[0].gameObject.SetActive(false);
                mainMenuBtn[1].gameObject.SetActive(false);
                mainMenuBtn[2].gameObject.SetActive(false);
                mainMenuBtn[4].gameObject.SetActive(false);
                mainMenuBtn[3].GetComponent<Text>().text = "Back";
                settingArea.SetActive(true);
            }
            else
            {
                isSetting = false;
                mainMenuBtn[0].gameObject.SetActive(true);
                mainMenuBtn[1].gameObject.SetActive(true);
                mainMenuBtn[2].gameObject.SetActive(true);
                mainMenuBtn[4].gameObject.SetActive(true);
                mainMenuBtn[3].GetComponent<Text>().text = "Options";
                settingArea.SetActive(false);
            }
        }

        if (btn == mainMenuBtn[4])
        {
            Application.Quit();
        }

        if (btn == mainMenuBtn[2])
        {
            chapterArea.SetActive(true);
            mainMenuBtn[0].gameObject.SetActive(false);
            mainMenuBtn[1].gameObject.SetActive(false);
            mainMenuBtn[2].gameObject.SetActive(false);
            mainMenuBtn[3].gameObject.SetActive(false);
            mainMenuBtn[4].gameObject.SetActive(false);

            mainMenuBtn[5].gameObject.SetActive(true);
            if(FindObjectOfType<SwitchSelectController>().enabled)
                FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Chapter1"));
        }

        if (btn == mainMenuBtn[5])
        {
            mainMenuBtn[0].gameObject.SetActive(true);
            mainMenuBtn[1].gameObject.SetActive(true);
            mainMenuBtn[2].gameObject.SetActive(true);
            mainMenuBtn[3].gameObject.SetActive(true);
            mainMenuBtn[4].gameObject.SetActive(true);

            mainMenuBtn[5].gameObject.SetActive(false);
            chapterArea.SetActive(false);
            if (FindObjectOfType<SwitchSelectController>().enabled)
                FindObjectOfType<EventSystem>().SetSelectedGameObject(GameObject.Find("Start"));
        }

        if (btn == mainMenuBtn[1])
        {
            switch(GameManager.Instance.playerSetting.lastLevel)
            {
                case Level.One:
                    SceneManager.LoadScene("SceneOne");
                    break;
                case Level.Two:
                    SceneManager.LoadScene("SceneTwo");
                    break;
                case Level.Three:
                    SceneManager.LoadScene("SceneThree");
                    break;
                case Level.Four:
                    SceneManager.LoadScene("SceneFour");
                    break;
                case Level.Five:
                    SceneManager.LoadScene("SceneFive");
                    break;
            }
        }
    }
}
