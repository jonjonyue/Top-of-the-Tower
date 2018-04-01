﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : EnemyController {



    override public IEnumerator damage(Collider col)
    {
        Debug.Log("Zombie Attack");
        yield return new WaitForSeconds(.33f);

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
                player.takeDamage(2);
            }
        }

        // we are done attacking
        attacking = false;
    }
}
