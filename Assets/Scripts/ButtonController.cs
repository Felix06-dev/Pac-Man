using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(() => OnButtonClicked("playButton"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonClicked(string buttonName)
    {
        Debug.Log(buttonName + " was clicked!");

        switch (buttonName)
        {
            case "playButton":
                Debug.Log("Action for Button 1");
                // Example: Change colour
                SceneManager.LoadScene("Game");
                Time.timeScale = 1f;
                break;
        }
    }
}
