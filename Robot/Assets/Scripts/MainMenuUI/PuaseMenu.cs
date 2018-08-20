using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PuaseMenu : MonoBehaviour
{

    private List<Button> pauseBtn = new List<Button>();
    public GameObject settingArea;

    private GameObject red, blue;
    private bool isSetting = false;
    private GameObject pauseMenuWwise;
    //private bool firstEnable = false;

    private void Awake()
    {
       // pauseMenuWwise = GameObject.Find("PauseMenuWwise");

        red = GameObject.FindGameObjectWithTag("Player1");
        blue = GameObject.FindGameObjectWithTag("Player2");
    }

    // Use this for initialization
    private void Start()
    {
        pauseBtn.Clear();
        isSetting = false;

        foreach (Button btn in this.GetComponentsInChildren<Button>(true))
        {
            pauseBtn.Add(btn);

            btn.onClick.AddListener(delegate
            {
                pauseButton(btn);
            });
        }
    }

    private void OnEnable()
    {
        red.GetComponent<InControlMovement>().enabled = false;
        red.GetComponent<SCR_TradeLimb>().enabled = false;
        red.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = false;
        red.GetComponent<Chirps>().enabled = false;
        red.GetComponent<MelodyInput>().enabled = false;

        blue.GetComponent<InControlMovement>().enabled = false;
        blue.GetComponent<SCR_player2Initalise>().enabled = false;
        blue.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = false;
        blue.GetComponent<Chirps>().enabled = false;
        blue.GetComponent<MelodyInput>().enabled = false;


        //FindObjectOfType<EventSystem>().SetSelectedGameObject(pauseBtn[0].gameObject);

        //FindObjectOfType<SwitchSelectController>().enabled = true;
        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        FindObjectOfType<EventSystem>().GetComponent<SwitchSelectController>().enabled = false;
        Cursor.visible = true;
        FindObjectOfType<SwitchInputDevices>().cover.SetActive(false);

    }

 

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && this.GetComponentInParent<Canvas>().gameObject.activeSelf)
        {
            this.GetComponentInParent<Canvas>().gameObject.SetActive(false);

            red.GetComponent<InControlMovement>().enabled = true;
            red.GetComponent<SCR_TradeLimb>().enabled = true;
            red.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = true;
            red.GetComponent<Chirps>().enabled = true;
            red.GetComponent<MelodyInput>().enabled = true;

            blue.GetComponent<InControlMovement>().enabled = true;
            blue.GetComponent<SCR_player2Initalise>().enabled = true;
            blue.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = true;
            blue.GetComponent<Chirps>().enabled = true;
            blue.GetComponent<MelodyInput>().enabled = true;

            settingArea.SetActive(false);
            pauseBtn[0].GetComponent<Transform>().parent.gameObject.SetActive(true);
            pauseBtn[2].GetComponent<Transform>().parent.gameObject.SetActive(true);
            pauseBtn[3].GetComponent<Transform>().parent.gameObject.SetActive(true);
            pauseBtn[1].GetComponent<Text>().text = "Options";

            AkSoundEngine.SetState("UI", "Off");
            AkSoundEngine.PostEvent("Menu_Exit", gameObject);


            FindObjectOfType<EventSystem>().firstSelectedGameObject = null;
            FindObjectOfType<SwitchSelectController>().selectObj = null;
            FindObjectOfType<SwitchSelectController>().enabled = false;
            //

        }
    }

    private void pauseButton(Button btn)
    {
        if (btn == pauseBtn[0])
        {
            this.GetComponentInParent<Canvas>().gameObject.SetActive(false);

            red.GetComponent<InControlMovement>().enabled = true;
            red.GetComponent<SCR_TradeLimb>().enabled = true;
            red.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = true;
            red.GetComponent<Chirps>().enabled = true;
            red.GetComponent<MelodyInput>().enabled = true;

            blue.GetComponent<InControlMovement>().enabled = true;
            blue.GetComponent<SCR_player2Initalise>().enabled = true;
            blue.GetComponentInChildren<PickupAndDropdown_Trigger>().enabled = true;
            blue.GetComponent<Chirps>().enabled = true;
            blue.GetComponent<MelodyInput>().enabled = true;

            AkSoundEngine.SetState("UI", "Off");
			AkSoundEngine.PostEvent("Menu_Exit", gameObject);

        }
        if (btn == pauseBtn[1])
        {
            //Debug.Log("Options Button Press");
            if (!isSetting)
            {
                settingArea.SetActive(true);
                pauseBtn[0].GetComponent<Transform>().parent.gameObject.SetActive(false);
                pauseBtn[2].GetComponent<Transform>().parent.gameObject.SetActive(false);
                pauseBtn[3].GetComponent<Transform>().parent.gameObject.SetActive(false);
                pauseBtn[1].GetComponent<Text>().text = "Back";
                isSetting = true;
				AkSoundEngine.PostEvent("Menu_Select", gameObject);


            }
            else
            {
                settingArea.SetActive(false);
                pauseBtn[0].GetComponent<Transform>().parent.gameObject.SetActive(true);
                pauseBtn[2].GetComponent<Transform>().parent.gameObject.SetActive(true);
                pauseBtn[3].GetComponent<Transform>().parent.gameObject.SetActive(true);
                pauseBtn[1].GetComponent<Text>().text = "Options";
                isSetting = false;
				AkSoundEngine.PostEvent("Menu_Select", gameObject);

            }


        }
        if (btn == pauseBtn[2])
        {
            SceneManager.LoadScene("MainMenu");
			AkSoundEngine.PostEvent("Menu_Select", gameObject);

        }
        if (btn == pauseBtn[3])
        {
            Application.Quit();
			AkSoundEngine.PostEvent("Menu_Select", gameObject);
        }
    }
}
