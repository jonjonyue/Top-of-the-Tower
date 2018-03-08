using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    public GameObject Boulder;
    public Collider col;
    public PlayerController pc;
    bool activated = false;
    int count = 0;

    // Use this for initialization
    void Start()
    {
		Boulder.SetActive (false);
    }

    private void Update()
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in cols)
        {
            if ((c.tag == "Player") && !activated)
			{
				Boulder.SetActive (true);
				Boulder.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -30, 0);
                pc.takeDamage(3);
				activated = true;
            }
        }
    }
}
