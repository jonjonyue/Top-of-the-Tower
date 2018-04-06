using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class startover : MonoBehaviour {

	// Use this for initialization
	public void beginagain(){
		Time.timeScale = 1.0f;
		SceneManager.LoadSceneAsync (1);
	}
}
