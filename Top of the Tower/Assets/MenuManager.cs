using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [Header("UI Objects")]
    public GameObject menuCanvas;
    public GameObject tutorialCanvas;
    public Image tutorialImage;
    public Sprite[] sprites;

    private bool inTutorial = false;
    private int spritesCounter = 0;

	// Use this for initialization
	void Start () {
        menuCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if(inTutorial) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                spritesCounter++;

                if (spritesCounter >= sprites.Length)
                    play();
                else
                    tutorialImage.sprite = sprites[spritesCounter];
            }
        }
	}

    public void play() {
        SceneManager.LoadSceneAsync(1);
    }

    public void playTutorial() {
        tutorialCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        inTutorial = true;
    }
}
