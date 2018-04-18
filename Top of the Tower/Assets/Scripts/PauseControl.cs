using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseControl : MonoBehaviour
{

    [Header("UI References")]
    public Text enemiesSlainText;
    public Text healthText;
    public Text timeText;

    GameObject player;

    // Time Variables
    int seconds;
    int min;
    int h;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.Find("Hero");
        //player.GetComponent<PlayerController>();
        enemiesSlainText.text = "You have slain " + "0" + " enemies!"; // default value?;
        healthText.text = "Your current health is " + "10!"; // default value?;
    }

    private void Update()
    {
        // Current Health
        healthText.text = "Your current health is " + player.GetComponent<PlayerController>().curHP() + "!";

        // Enemies Slain
        int numSlain = player.GetComponent<PlayerController>().slayed();
        if (numSlain == 1)
            enemiesSlainText.text = "You have slain " + numSlain + " enemy!";
        else
            enemiesSlainText.text = "You have slain " + numSlain + " enemies!";

        // Time Update
        seconds = (int)Time.realtimeSinceStartup;

        if (seconds > 3600)
        {
            h = seconds / 3600;
            seconds = seconds - h * 3600;
            min = seconds / 60;
            seconds = seconds - min * 60;
            timeText.text = "You have played for " + h + "h " + min + "m " + seconds + "s!";


        }
        else if (seconds > 60)
        {
            min = seconds / 60;
            seconds = seconds - min * 60;
            timeText.text = "You have played for " + min + "m " + seconds + "s!";
        }
        else
        {
            timeText.text = "You have played for " + seconds + "s!";
        }
    }

    public void pause()
    {
        Time.timeScale = 0f;
    }

    public void continueGame()
    {
        Time.timeScale = 1.0f;
    }

    public void LoadByIndex(int sceneIndex)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
