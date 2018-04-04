using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : TrapBase {

    public GameObject boulder;
    public GameObject glow;

	// Use this for initialization
	void Start () {
        boulder.SetActive(false);
        glow.SetActive(true);
	}
	
    public override void activateTrap() {
        //Debug.Log("Boulder activated");
        boulder.SetActive(true);
        glow.SetActive(false);
        boulder.GetComponent<Rigidbody>().velocity = new Vector3(0, -30, 0);
    }
}
