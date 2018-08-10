using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialOne : TutorialBaseClass
{
    private SCR_TradeLimb player;
    private bool checkOnce = true;
    //Used as reference point for the base class
    //allowing various operations to stay dynamic.
    void Awake()
    {
        tutorialIdentifier = "TutorialOne";
    }

    protected override void TextPromptTutorialSetup()
    {
        player = GameObject.FindGameObjectWithTag("Player1").GetComponent<SCR_TradeLimb>();

        tutorialPrompt = Instantiate(Resources.Load("Prefabs/UI/TradeLimbTutorial")) as GameObject;
        tutorialPrompt.transform.SetParent(GameObject.FindGameObjectWithTag("TutorialUI").transform, false);

        Transform textParent = tutorialPrompt.transform.GetChild(0).GetChild(1);

        textParent.GetChild(0).GetComponent<Text>().text = GameManager.Instance.playerSetting.currentButton[10].ToString();
        textParent.GetChild(1).GetComponent<Text>().text = GameManager.Instance.playerSetting.currentButton[23].ToString();
    }

    protected override void TextPromptTutorial()
    {
        if((checkOnce) && (player.LimbActiveCheck("LeftArm")))
        {
            checkOnce = false;
            Destroy(tutorialPrompt);
        }
    }
}
