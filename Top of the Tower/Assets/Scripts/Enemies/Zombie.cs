using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : EnemyController {



    override public IEnumerator attack(Collider col)
    {
        yield return new WaitForSeconds(Random.Range(.2f, .5f));

        //Debug.Log("Zombie Attack");

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(.33f);

        // Get the hitboxes hit by the attack
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        //ATTACK MECHANICS
        foreach (Collider c in cols)
        {
            if (c.tag == "Player")
            {
                StartCoroutine(damage());
            }
        }

        while (anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            yield return null;
        }

        yield return new WaitForSeconds(.1f);

        // we are done attacking
        attacking = false;
    }
}
