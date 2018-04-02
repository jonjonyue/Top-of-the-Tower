using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour {

    // Use this for initialization
    public void pause()
    {
        Time.timeScale = 0f;
    }

    private void Start()
    {
        
    }
}
