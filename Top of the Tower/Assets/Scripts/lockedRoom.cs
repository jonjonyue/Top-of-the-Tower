using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedRoom : MonoBehaviour {

    [HideInInspector]public bool enemies;
    [HideInInspector]public bool player;
    [HideInInspector]public bool open;
    private Collider col;
	private GameObject[] lockedD;

	// Use this for initialization
	void Start () {
		enemies = false;
		player = false;
		open = true;
        col = GetComponent<Collider>();
		lockedD = GameObject.FindGameObjectsWithTag ("LockedRoom");
        StartCoroutine(trapRoom());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator trapRoom() {
        enemies = false;
        while (open == true) {
            yield return null;
        }
        while (enemies == false && open == false) {
            Collider[] hitColliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in hitColliders)
            {
                if (c.tag == "Player")
                {
                    player = true;
                }
                else if (c.tag == "Enemy")
                {
                    EnemyController enemy = col.gameObject.GetComponentInParent<EnemyController>();
                    if (enemy.isAlive)
                    {
                        enemies = true;
                    }
                }
            }
            yield return new WaitForSeconds(.5f);
        }
        openRoom();
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
