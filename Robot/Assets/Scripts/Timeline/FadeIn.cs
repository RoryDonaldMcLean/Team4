using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    private float timer = 0; 

	GameObject NarrativeCanvas;

    // Use this for initialization
    void Start()
    {
		NarrativeCanvas = GameObject.Find ("CanvasNarrative");
        //AkSoundEngine.SetState("Environment", "Credits");
    }

    // Update is called once per frame
    void Update()
    {
		if (NarrativeCanvas.GetComponent<NarrativeText> ().TextDone == true)
		{
			Debug.Log ("b0ss");
			if (this.GetComponent<Image> ().color.a > 0)
				this.GetComponent<Image> ().color = new Color (this.GetComponent<Image> ().color.r, this.GetComponent<Image> ().color.g,
					this.GetComponent<Image> ().color.b, this.GetComponent<Image> ().color.a - 0.01f);

			if (timer < 10)
				timer += Time.deltaTime;
			else
				ReturnMain ();
		}
    }

    private void ReturnMain()
    {
        SceneManager.LoadScene("MainMenu");
    } 
}
