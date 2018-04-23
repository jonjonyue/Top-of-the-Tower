using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockedRoom : MonoBehaviour {

    [HideInInspector]public bool enemiesDead;
    [HideInInspector]public bool playerInRoom;
    [HideInInspector]public bool doorIsOpen;
    public GameObject[] spawnPoints;
    public GameObject spawnSkeleton;
    bool isTripped = false;
    private Collider col;
	private GameObject[] lockedD;

	// Use this for initialization
	void Start () {
		enemiesDead = false;
		playerInRoom = false;
		doorIsOpen = true;
        col = GetComponent<Collider>();
		lockedD = GameObject.FindGameObjectsWithTag ("LockedRoom");
        StartCoroutine(trapRoom());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator trapRoom() {
        enemiesDead = false;
        while (doorIsOpen) {
            yield return null;
        }
        isTripped = true;
        for (int i = 0; i < spawnPoints.Length; ++i) {
            Instantiate(spawnSkeleton, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            yield return new WaitForSeconds(1f);
        }
        while (enemiesDead == false) {
            enemiesDead = true;
            Collider[] hitColliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            int deadCount = 0;
            foreach (Collider c in hitColliders)
            {
                if (c.tag == "Enemy")
                {
                    SkeletonSpawn[] spawns = GameObject.FindObjectsOfType<SkeletonSpawn>();
                    for (int i = 0; i < spawns.Length; ++i) {
                        if (spawns[0].isAlive)
                            enemiesDead = false;
                    }
                }
            }
            yield return new WaitForSeconds(.5f);
        }
        openRoom();
    }

	void OnTriggerEnter(Collider other)
	{
        if (!isTripped)
        {
            if (other.gameObject.tag == "Player" && doorIsOpen)
            {
                playerInRoom = true;
                Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 15);
                foreach (Collider col in hitColliders)
                {
                    if (col.gameObject.tag == "LockedRoom")
                    {
                        Vector3 temp = col.transform.position;
                        for (int i = 0; i < 5; i++)
                        {
                            temp.y = temp.y - 1;
                            col.transform.position = temp;
                        }
                        //col.GetComponent<Collider> ().enabled = !col.GetComponent<Collider> ().enabled;
                        doorIsOpen = false;
                    }
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
				doorIsOpen = true;
			}
		}
	}
}
