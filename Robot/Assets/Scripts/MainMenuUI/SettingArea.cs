using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingArea : MonoBehaviour
{
    public GameObject btnlist, textlist;
    public List<Button> playerButtons;
    public Text volumeNum;

    private Button defaultbtn;
    private Toggle tog;
    private bool[] reboundButton;
    private Slider volume;

    private void Awake()
    {
        reboundButton = new bool[24];
    }

    // Use this for initialization
    private void OnEnable()
    {
        playerButtons.Clear();
        foreach (Button btn in this.GetComponentsInChildren<Button>(true))
        {
            if (btn.name != "Default")
            {
                playerButtons.Add(btn);
            }
            else
            {
                defaultbtn = btn;
                defaultbtn.onClick.AddListener(delegate
                {
                    DefaultBtn();
                });
            }
        }
        
        for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
        {
            int num = i;
            reboundButton[num] = false;
            playerButtons[num].onClick.AddListener(delegate
            {
                reBtn(num);
            });
        }

        tog = this.GetComponentInChildren<Toggle>(true);
        tog.isOn = GameManager.Instance.playerSetting.player1Controller;
        tog.onValueChanged.AddListener(delegate
        {
            toggleChange();
        });

        volume = this.GetComponentInChildren<Slider>(true);
        volume.value = GameManager.Instance.playerSetting.volume;
        volumeNum.text = GameManager.Instance.playerSetting.volume.ToString();
        volume.onValueChanged.AddListener(delegate
        {
            sliderChanged();
        });
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
        {
            if (!allFalse())
            {
                playerButtons[i].gameObject.SetActive(true);
                Text text = playerButtons[i].GetComponentInChildren<Text>(true);
                text.text = GameManager.Instance.playerSetting.currentButton[i].ToString();
            }
            else
            {
                playerButtons[i].gameObject.SetActive(false);
                if(reboundButton[i])
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        reboundButton[i] = false;
                    }
                    if (Input.anyKeyDown)
                    {
                        if (!Input.GetKey(KeyCode.Escape) && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
                        {
                            reboundButton[i] = false;
                            GameManager.Instance.playerSetting.currentButton[i] = GetKeyCode();
                            GameManager.Instance.playerSetting.sameButton(i);
                            SaveNLoad.Save(GameManager.Instance.playerSetting);
                        }
                    }
                }
            }
        }

        if(!GameManager.Instance.playerSetting.player1Controller)
        {
            btnlist.SetActive(true);
            textlist.SetActive(true);
        }
        else
        {
            btnlist.SetActive(false);
            textlist.SetActive(false);
        }

        if(!allFalse())
        {
            defaultbtn.gameObject.SetActive(true);
            tog.gameObject.SetActive(true);
        }
        else
        {
            defaultbtn.gameObject.SetActive(false);
            tog.gameObject.SetActive(false);
        }



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

    private void toggleChange()
    {
        GameManager.Instance.playerSetting.player1Controller = tog.isOn;
        SaveNLoad.Save(GameManager.Instance.playerSetting);
    }

    private void DefaultBtn()
    {
        for (int i = 0; i < GameManager.Instance.playerSetting.currentButton.Length; i++)
        {
            GameManager.Instance.playerSetting.currentButton[i] = GameManager.Instance.playerSetting.defaultButton[i];
        }
        GameManager.Instance.playerSetting.volume = 50;
        volume.value = GameManager.Instance.playerSetting.volume;
        volumeNum.text = volume.value.ToString();
        GameManager.Instance.playerSetting.player1Controller = false;
        SaveNLoad.Save(GameManager.Instance.playerSetting);
    }

    private void reBtn(int order)
    {
        reboundButton[order] = true;
    }

    private void sliderChanged()
    {
        GameManager.Instance.playerSetting.volume = (int)volume.value;
        volumeNum.text = GameManager.Instance.playerSetting.volume.ToString();
        SaveNLoad.Save(GameManager.Instance.playerSetting);
    }

    private KeyCode GetKeyCode()
    {
        int n = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < n; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
                return (KeyCode)i;
        }
        return KeyCode.None;
    }
}
