using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterArea : MonoBehaviour
{

    public List<Button> chapterBtn;
    //private bool firstEnable = false;
    
    // Use this for initialization
    private void Start()
    {
        chapterBtn.Clear();
        foreach (Button btn in this.GetComponentsInChildren<Button>(true))
        {
            chapterBtn.Add(btn);

            btn.onClick.AddListener(delegate
            {
                chapter(btn);
            });
        }
    }

    private void chapter(Button btn)
    {
        if (btn == chapterBtn[0])
        {
            PlayerPrefs.SetInt("level", 0);
            SceneManager.LoadScene("SceneOne");
        }
        if (btn == chapterBtn[1])
        {
            PlayerPrefs.SetInt("level", 1);
            SceneManager.LoadScene("SceneTwo");
        }
        if (btn == chapterBtn[2])
        {
            PlayerPrefs.SetInt("level", 2);
            SceneManager.LoadScene("SceneThree");
        }
        if (btn == chapterBtn[3])
        {
            PlayerPrefs.SetInt("level", 3);
            SceneManager.LoadScene("SceneFour");
        }
        if (btn == chapterBtn[4])
        {
            PlayerPrefs.SetInt("level", 4);
            SceneManager.LoadScene("SceneFive");
        }
    }
}
