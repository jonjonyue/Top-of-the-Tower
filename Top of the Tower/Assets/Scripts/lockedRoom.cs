using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedRoom : MonoBehaviour {

	public bool enemies;
	public bool player;
	public bool open;
	private GameObject[] lockedD;

	// Use this for initialization
	void Start () {
		enemies = false;
		player = false;
		open = true;
		lockedD = GameObject.FindGameObjectsWithTag ("LockedRoom");
//		foreach (GameObject go in lockedD) {
//			go.GetComponent<Collider> ().enabled = false;
//		}
	}
	
	// Update is called once per frame
	void Update () {
		enemies = false;
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10);
		foreach (Collider col in hitColliders) {
			if (col.tag == "Player") {
				player = true;
			} else if (col.tag == "Enemy") {
				EnemyController enemy = col.gameObject.GetComponentInParent<EnemyController> ();
				if(enemy.isAlive){
					enemies = true;
				}
			}
		}
		if (enemies == false && open == false) {
			openRoom ();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			player = true;
			Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 15);
			foreach (Collider col in hitColliders) {
				if (col.gameObject.tag == "LockedRoom") {
					Vector3 temp = col.transform.position;
					for (int i = 0; i < 5; i++) {
						temp.y = temp.y - 1;
						col.transform.position = temp;
					}
					//col.GetComponent<Collider> ().enabled = !col.GetComponent<Collider> ().enabled;
					open = false;
				}
			}
		}
	}

	void openRoom(){
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 15);
		foreach (Collider col in hitColliders) {
			if (col.gameObject.tag == "LockedRoom") {
				Vector3 temp = col.transform.position;
				for (int i = 0; i < 5; i++) {
					temp.y = temp.y + 1;
					col.transform.position = temp;				
				}
				//col.GetComponent<Collider> ().enabled = !col.GetComponent<Collider> ().enabled;
				open = true;
			}
		}
	}
}
