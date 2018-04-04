using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hiddenDoor : MonoBehaviour {

	public GameObject button;
	public GameObject hDoor;
	//public Collider col;
	//public PlayerController pc;
	bool activated = false;

	// Use this for initialization
	void Start () {
		hDoor = GameObject.FindGameObjectWithTag ("HiddenDoor");
		button = GameObject.FindGameObjectWithTag ("button");
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			activated = true;
			TouchObj ();
		}
	}

	private void TouchObj(){
		if (activated) {
			hDoor.GetComponent<Renderer> ().enabled = !hDoor.GetComponent<Renderer> ().enabled;
			hDoor.GetComponent<Collider> ().enabled = !hDoor.GetComponent<Collider> ().enabled;
			activated = false;
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
