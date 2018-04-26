using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    public Collider col;
    public int damage = 0;
    public bool activated = false;
    private PlayerController pc;

	private void Start()
	{
        initialize();
	}

    public void initialize() {
        pc = GameObject.Find("Hero").GetComponent<PlayerController>();
    }

	private void Update()
    {
        checkColliders(col);
    }

    public void checkColliders(Collider targetCol) {
        Collider[] cols = Physics.OverlapBox(targetCol.bounds.center, targetCol.bounds.extents, targetCol.transform.rotation, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in cols)
        {
            if ((c.tag == "Player") && !activated)
            {
                //Debug.Log(name + " activated");
                activateTrap();
                activated = true;
                StartCoroutine(damageTargets());
            }
        }
    }

    public virtual void activateTrap() {
        // Override to have trap specific functionality.
    }

    public virtual IEnumerator damageTargets() {
        yield return new WaitForSeconds(.25f);
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        foreach (Collider b in cols)
            if (b.tag == "Player" || b.tag == "Enemy")
                b.gameObject.GetComponentInParent<character>().takeDamage(damage);
    }
}
