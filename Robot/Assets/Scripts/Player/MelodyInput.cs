using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class MelodyInput : MonoBehaviour
{
    public int playerNum;
    GameObject MelodyDoor;
    private Animator anim;
    GameObject CanvasNoteSheet;

    Sprite[] Arrows;
    List<GameObject> Notes = new List<GameObject>();

    public bool isBlue;
    GameObject MelodyDoorContainer;

    GameObject GameController;
    int levelCounter;
    int CodeTotal;


    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();
        MelodyDoorContainer = GameObject.FindGameObjectWithTag("Doors");
        if (MelodyDoorContainer != null) MelodyDoor = MelodyDoorContainer.transform.GetChild(0).gameObject;

        Arrows = new Sprite[8];
        for (int i = 0; i < 8; i++)
        {
            Arrows[i] = Resources.Load<Sprite>("Art/PlaceHolder/" + (i + 1)) as Sprite;
        }

        CanvasNoteSheet = GameObject.FindGameObjectWithTag("CanvasNoteSheet");

        if (CanvasNoteSheet != null)
        {
            for (int i = 0; i < CanvasNoteSheet.transform.childCount; i++)
            {
                Notes.Add(CanvasNoteSheet.transform.GetChild(i).gameObject);
            }
        }

        GameController = GameObject.FindGameObjectWithTag("GameController");

        levelCounter = GameController.GetComponent<LevelController>().currentLevel;

        //change the total number of digits needed depending on the what level it is
        if (levelCounter == 0)
        {
            CodeTotal = 8;
        }
        else if (levelCounter == 1)
        {
            CodeTotal = 7;
        }
        else if (levelCounter == 2)
        {
            CodeTotal = 6;
        }
        else if (levelCounter == 3)
        {
            CodeTotal = 4;
        }
        else if (levelCounter == 4)
        {
            CodeTotal = 2;
        }

    }

    // Update is called once per frame
    void Update()
    {
        var inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;

        if (inputDevice == null)
        {
            ProcessInput();
        }
        else
        {
            ProcessInputIncontrol(inputDevice);
        }
    }


    //keyboard controls
    void ProcessInput()
    {
        if (MelodyDoorContainer != null)
        {
            //player 1
            if ((MelodyDoor.GetComponent<SCR_Door>().Player1enteredBounds == true &&
                MelodyDoor.GetComponent<InControlMelody>().Robotcode.Count < CodeTotal))
            {
                if (isBlue == GameManager.Instance.whichAndroid.player1ControlBlue)
                {
                    if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[4]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[0].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(1);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[0];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player1 has no left arm");
                        }
                    }
                    else if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[5]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(2);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[3];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player 1 has no right arm");
                        }
                    }
                    else if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[6]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[1].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(3);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[1];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            anim.SetBool("IsButtonPressed", true);
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                        }
                        else
                        {
                            Debug.Log("Player 1 has no left leg");
                        }
                    }
                    else if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[7]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[3].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(4);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[2];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player1 has no right leg");
                        }
                    }// clear the code incase you get it wrong
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        Debug.Log("b0ss plz");
                        MelodyDoor.GetComponent<InControlMelody>().CheckCode();
                    }
                    else
                    {
                        Debug.Log(111);
                        anim.SetBool("IsButtonPressed", false);
                    }

                    if (MelodyDoor.GetComponent<InControlMelody>().noteCounter == CodeTotal)
                    {
                        StartCoroutine(UIDelay());
                    }

                }
            }

            //player 2
            if ((MelodyDoor.GetComponent<SCR_Door>().Player2enteredBounds == true &&
                MelodyDoor.GetComponent<InControlMelody>().Robotcode.Count < CodeTotal))
            {
                if (isBlue != GameManager.Instance.whichAndroid.player1ControlBlue)
                {
                    if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[17]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[0].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(5);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[4];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no left arm");
                        }
                    }
                    else if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[18]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(6);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[7];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no right arm");
                        }
                    }
                    else if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[19]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[1].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(7);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[5];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no left leg");
                        }
                    }
                    else if (Input.GetKeyDown(GameManager.Instance.playerSetting.currentButton[20]))
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[3].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(8);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[6];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no right leg");
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Keypad2))
                    {
                        Debug.Log("b0ss plz");
                        MelodyDoor.GetComponent<InControlMelody>().CheckCode();
                    }
                    else
                    {
                        Debug.Log(222);
                        anim.SetBool("IsButtonPressed", false);
                    }

                    if (MelodyDoor.GetComponent<InControlMelody>().noteCounter == CodeTotal)
                    {
                        StartCoroutine(UIDelay());
                    }
                }
            }
        }
    }


    //controller input
    void ProcessInputIncontrol(InputDevice inputDevice)
    {
        if (MelodyDoorContainer != null)
        {
            //player 1
            if ((MelodyDoor.GetComponent<SCR_Door>().Player1enteredBounds == true &&
                MelodyDoor.GetComponent<InControlMelody>().Robotcode.Count < CodeTotal))
            {
                if (playerNum == 0)
                {
                    if (inputDevice.DPadUp.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[0].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(1);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[0];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player1 has no left arm");
                        }
                    }
                    else if (inputDevice.DPadDown.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(2);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[3];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player 1 has no right arm");
                        }
                    }
                    else if (inputDevice.DPadLeft.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[1].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(3);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[1];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player 1 has no left leg");
                        }
                    }
                    else if (inputDevice.DPadRight.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>().limbs[3].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(4);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[2];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteA", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player1 has no right leg");
                        }
                    }
                    else if (inputDevice.Action3.WasPressed)
                    {
                        Debug.Log("b0ss plz");
                        MelodyDoor.GetComponent<InControlMelody>().CheckCode();
                    }
                    else
                    {
                        anim.SetBool("IsButtonPressed", false);
                    }

                    if (MelodyDoor.GetComponent<InControlMelody>().noteCounter == CodeTotal)
                    {
                        StartCoroutine(UIDelay());
                    }

                }
            }

            //player 2
            if ((MelodyDoor.GetComponent<SCR_Door>().Player2enteredBounds == true &&
                MelodyDoor.GetComponent<InControlMelody>().Robotcode.Count < CodeTotal))
            {
                if (playerNum == 1)
                {
                    if (inputDevice.DPadUp.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[0].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(5);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[4];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no left arm");
                        }
                    }
                    else if (inputDevice.DPadDown.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[2].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(6);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[7];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no right arm");
                        }
                    }
                    else if (inputDevice.DPadLeft.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[1].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(7);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[5];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no left leg");
                        }
                    }
                    else if (inputDevice.DPadRight.WasPressed)
                    {
                        if (GameObject.FindGameObjectWithTag("Player2").GetComponent<SCR_TradeLimb>().limbs[3].activeSelf)
                        {
                            MelodyDoor.GetComponent<InControlMelody>().Robotcode.Add(8);
							Notes[MelodyDoor.GetComponent<InControlMelody>().noteCounter].GetComponent<Image>().sprite = Arrows[6];
                            MelodyDoor.GetComponent<InControlMelody>().noteCounter += 1;
                            AkSoundEngine.PostEvent("GateNoteB", gameObject);
                            anim.SetBool("IsButtonPressed", true);
                        }
                        else
                        {
                            Debug.Log("Player2 has no right leg");
                        }
                    }
                    else if (inputDevice.Action3.WasPressed)
                    {
                        Debug.Log("b0ss plz");
                        MelodyDoor.GetComponent<InControlMelody>().CheckCode();
                    }
                    else
                    {
                        anim.SetBool("IsButtonPressed", false);
                    }

                    if (MelodyDoor.GetComponent<InControlMelody>().noteCounter == CodeTotal)
                    {
                        StartCoroutine(UIDelay());
                    }
                }
            }

        }
    }

    IEnumerator UIDelay()
    {
        yield return new WaitForSeconds(1);
        MelodyDoor.GetComponent<InControlMelody>().CheckCode();
    }
}
