using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : TrapBase {

    [Header("Trap Options")]
    public bool movingTrap;
    public Transform firstPos;
    public Transform secondPos;
    public float speed = 1;
    public bool sanity = true;

    // Update is called once per frame
    void Update()
    {
        activated = false;
        transform.position = Vector3.Lerp(firstPos.position, secondPos.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
        checkColliders(this.col);
    }

    override public IEnumerator damageTargets()
    {
        if (sanity)
        {
            sanity = false;
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

            foreach (Collider b in cols)
            {
                if (b.tag == "Player" || b.tag == "Enemy")
                    b.gameObject.GetComponentInParent<character>().takeDamage(damage);
                //Debug.Log(b.tag);
            }

            yield return new WaitForSeconds(.25f);
            sanity = true;
        }
    }
}
