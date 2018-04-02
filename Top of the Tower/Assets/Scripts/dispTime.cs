using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dispTime : MonoBehaviour {
    int seconds;
    int min;
    int h;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        seconds = (int)Time.realtimeSinceStartup;

        if (seconds >3600)
        {
            h = seconds / 3600;
            seconds = seconds - h * 3600;
            min = seconds / 60;
            seconds = seconds - min * 60;
            gameObject.GetComponent<Text>().text = "You have played for " + h+ "h "+ min + "m " + seconds + "s!"; 
        

        }else if(seconds >60)
        {
            min = seconds / 60;
            seconds = seconds - min * 60;
            gameObject.GetComponent<Text>().text = "You have played for " + min + "m " + seconds + "s!";
        }
        else
        {
            gameObject.GetComponent<Text>().text = "You have played for " + seconds + "s!"; 
        }
        
    }
}
