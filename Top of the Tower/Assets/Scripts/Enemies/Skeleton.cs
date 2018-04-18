using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : EnemyController
{
    override public IEnumerator damage(Collider col)
    {
        yield return new WaitForSeconds(.625f);

        // Get the hitboxes hit by the attack
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        //ATTACK MECHANICS
        foreach (Collider c in cols)
        {
            if (c.tag == "Player")
            {
                PlayerController player = c.gameObject.GetComponentInParent<PlayerController>();
                Color originalColor = player.GetComponent<SpriteRenderer>().color;
                player.GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(.2f);
                player.GetComponent<SpriteRenderer>().color = originalColor;
                player.takeCombatDamage(strength);
            }
        }

        // we are done attacking
        attacking = false;
    }
}
