using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Necromancer : EnemyController {

    // Necromancer stuff

	protected override void Start()
	{
        heroTransform = GameObject.Find("Hero").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //health bar setup
        healthSlider.maxValue = health;
        healthSlider.value = health;


        attacking = false;
	}

	protected override void Update()
	{
        if(isAlive) {
            Move();
        }
	}

    override public void takeDamage(int damage)
    {
        if (isAlive)
        {
            health -= damage;
            healthSlider.value = health;
            var clone = (GameObject)Instantiate(damageNumber, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), transform);
            clone.GetComponent<FloatingText>().damageNumber = -1 * damage;

            if (health <= 0) {}
                // kill necro
        }
    }

    override public void takeCombatDamage(int damage)
    {
        if (isAlive)
        {
            health -= damage - defense;
            healthSlider.value = health;
            var clone = (GameObject)Instantiate(damageNumber, gameObject.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), transform);
            clone.GetComponent<FloatingText>().damageNumber = defense - damage;

            if (health <= 0) {}
                // kill necro
        }
    }

    void Move()
    {
        // animation codes:
        // 0 - forward walk
        // 1 - side walk
        // 3 - back walk

        Vector3 targetPosition = heroTransform.position;
        Vector3 targetVector = targetPosition - transform.position;

        if (targetVector.magnitude < aggroDistance)
        {
            //alert.SetActive(true);
            if (Mathf.Abs(targetVector.magnitude) > 1)
            {
                //Vector3 unitVector = targetVector / targetVector.magnitude;

                agent.SetDestination(heroTransform.position);

                var enemyAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

                if (enemyAngle < 0.0f)
                    enemyAngle += 360;

                if (enemyAngle >= 315f || enemyAngle < 45f)
                { /* Right */
                    sr.flipX = false;
                    anim.SetBool("idle", false);
                    anim.SetInteger("direction", 1);
                }
                else if (enemyAngle >= 135f && enemyAngle < 225f)
                { /* Left */
                    sr.flipX = true;
                    anim.SetBool("idle", false);
                    anim.SetInteger("direction", 1);
                }
                else if (enemyAngle >= 45f && enemyAngle < 135f)
                { /* Up */
                    anim.SetBool("idle", false);
                    anim.SetInteger("direction", 3);
                }
                else if (enemyAngle >= 225f && enemyAngle < 315f)
                { /* Down */
                    anim.SetBool("idle", false);
                    anim.SetInteger("direction", 0);
                }
            }
            else if (!attacking)
            {
                agent.SetDestination(transform.position);
                //Attack(attackHitboxes[0]);
            }
        }
        //else
        //{
        //    alert.SetActive(false);
        //    //timer += Time.deltaTime;

        //    //if (timer >= wanderTimer)
        //    //{
        //    //    Vector3 newPos = RandomNavSphere(startPosition, wanderRadius, -1);
        //    //    agent.SetDestination(newPos);
        //    //    timer = 0;
        //    //}
        //    // check which way he is facing, set idle animation to that one
        //    if (!attacking)
        //    {

        //        if (agent.velocity.magnitude != 0) // if agent is wandering, walk
        //        {

        //            var enemyAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

        //            if (enemyAngle >= 315f || enemyAngle < 45f)
        //            { /* Right */
        //                sr.flipX = false;
        //                anim.SetBool("idle", false);
        //                anim.SetInteger("direction", 1);
        //            }
        //            else if (enemyAngle >= 135f && enemyAngle < 225f)
        //            { /* Left */
        //                sr.flipX = true;
        //                anim.SetBool("idle", false);
        //                anim.SetInteger("direction", 1);
        //            }
        //            else if (enemyAngle >= 45f && enemyAngle < 135f)
        //            { /* Up */
        //                anim.SetBool("idle", false);
        //                anim.SetInteger("direction", 3);
        //            }
        //            else if (enemyAngle >= 225f && enemyAngle < 315f)
        //            { /* Down */
        //                anim.SetBool("idle", false);
        //                anim.SetInteger("direction", 0);
        //            }

        //        }
        //        else // If agent isn't wandering, idle
        //        {
        //            int currAnim = anim.GetInteger("direction");
        //            if (currAnim == 3)
        //            {
        //                anim.SetBool("idle", true);
        //            }
        //            else if (currAnim == 1)
        //            {
        //                anim.SetBool("idle", true);
        //            }
        //            else
        //            {
        //                anim.SetBool("idle", true);
        //            }
        //        }
        //    }
        //}
    }
}
