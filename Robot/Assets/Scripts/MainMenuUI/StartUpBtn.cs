using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUpBtn : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ButtonOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ButtonOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
