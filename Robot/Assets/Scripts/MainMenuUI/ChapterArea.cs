using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterArea : MonoBehaviour {

    public List<Button> chapterBtn;

	// Use this for initialization
	private void OnEnable () {
		foreach(Button btn in this.GetComponentsInChildren<Button>(true))
        {
            chapterBtn.Add(btn); btn.onClick.AddListener(delegate
            {
                chapter(btn);
            });
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void chapter(Button btn)
    {
        if(btn == chapterBtn[0])
        {
            SceneManager.LoadScene("SceneOne");
        }
        if (btn == chapterBtn[1])
        {
            SceneManager.LoadScene("SceneTwo");
        }
        if (btn == chapterBtn[2])
        {
            SceneManager.LoadScene("SceneThree");
        }
        if (btn == chapterBtn[3])
        {
            SceneManager.LoadScene("SceneFour");
        }
        if (btn == chapterBtn[4])
        {
            SceneManager.LoadScene("SceneFive");
        }
    }
}
