using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [Header("UI Objects")]
    public GameObject menuCanvas;
    public GameObject tutorialCanvas;
    public GameObject storyCanvas;
    public Image tutorialImage;
    public Image storyImage;
    public Sprite[] sprites;
    public Sprite[] story;

    private bool inTutorial = false;
    private bool inStory = false;

    private int spritesCounter = 0;
    private int storyCounter = 0;

	// Use this for initialization
	void Start () {
        menuCanvas.SetActive(true);
        tutorialCanvas.SetActive(false);
        storyCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if(inTutorial) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                spritesCounter++;

                if (spritesCounter >= sprites.Length){
                    storyCanvas.SetActive(true);
                    tutorialCanvas.SetActive(false);
                    inTutorial = false;
                    inStory = true;
                } else
                    tutorialImage.sprite = sprites[spritesCounter];
            }
        }
        if(inStory) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                storyCounter++;

                if (storyCounter >= story.Length)
                    play();
                else
                    storyImage.sprite = story[storyCounter];
            }
        }
	}

    public void startStory() {
        inStory = true;
        menuCanvas.SetActive(false);
        storyCanvas.SetActive(true);
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
