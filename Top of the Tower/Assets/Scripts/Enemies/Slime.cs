using UnityEngine;
using System.Collections;

public class Slime : EnemyController
{
    private Vector3 attackStartPosition;
    private Vector3 target;
    private bool didHitWhileDashing = false;
    public float dashSpeed;



    protected override void Start()
    {
        base.Start();
        attackStartPosition = transform.position;
        target = heroTransform.position;
    }

    protected override void Update()
    {
        if (isAlive)
        {
            if (Vector3.Distance(transform.position, heroTransform.position) < attackDistance && !attacking)
            {
                attacking = true;
                Attack(attackHitboxes[0]);
            }
            else if (!attacking)
            {
                Move();
            }
        }
        else if (!counted)
        {
            counted = count();
        }
    }

    public override void Attack(Collider col)
    {
        StartCoroutine(attack(col));
    }

    public override IEnumerator attack(Collider col)
    {
        anim.SetTrigger("attack");
        attackStartPosition = transform.position;
        Vector3 targetVector = (transform.position - heroTransform.position) * -1.5f;
        targetVector.y = targetVector.y / 1.5f;
        target = attackStartPosition + targetVector;
        agent.speed = 8;
        agent.SetDestination(target);


        while (agent.remainingDistance > 0)
        {
            //Debug.Log("Remaining dash distance = " + agent.remainingDistance);
            if (!didHitWhileDashing)
            {
                Collider coll = attackHitboxes[0];
                Collider[] cols = Physics.OverlapBox(coll.bounds.center, coll.bounds.extents, coll.transform.rotation, LayerMask.GetMask("Hitbox"));

                //ATTACK MECHANICS
                foreach (Collider c in cols)
                {
                    if (c.tag == "Player")
                    {
                        Debug.Log("Hit!");
                        didHitWhileDashing = true;
                        StartCoroutine(damage());
                    }
                }
            }
            yield return null;
        }

        Debug.Log("Attack End");
        agent.speed = 1.5f;
        yield return new WaitForSeconds(.5f);
        attacking = false;
        didHitWhileDashing = false;
    }

    public override IEnumerator damage()
    {
        PlayerController player = GameObject.Find("Hero").GetComponent<PlayerController>();
        Color originalColor = player.GetComponent<SpriteRenderer>().color;

        player.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.2f);
        player.GetComponent<SpriteRenderer>().color = originalColor;
        player.takeCombatDamage(strength);
    }

    public override void Move()
    {
        Vector3 targetVector = heroTransform.position - transform.position;

        if (targetVector.magnitude < aggroDistance)
        {
            alert.SetActive(true);
            //Vector3 unitVector = targetVector / targetVector.magnitude;

            agent.SetDestination(heroTransform.position);

            var enemyAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

            if (enemyAngle < 0.0f)
                enemyAngle += 360;

            if (enemyAngle >= 315f || enemyAngle < 45f)
            { /* Right */
                sr.flipX = false;
            }
            else if (enemyAngle >= 135f && enemyAngle < 225f)
            { /* Left */
                sr.flipX = true;
            }
            anim.SetBool("idle", false);
        }
        else
        {
            alert.SetActive(false);
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(startPosition, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
            // check which way he is facing, set idle animation to that one
            if (!attacking)
            {

                if (agent.velocity.magnitude != 0) // if agent is wandering, walk
                {

                    var enemyAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

                    if (enemyAngle >= 315f || enemyAngle < 45f)
                    { /* Right */
                        sr.flipX = false;
                    }
                    else if (enemyAngle >= 135f && enemyAngle < 225f)
                    { /* Left */
                        sr.flipX = true;
                    }
                    anim.SetBool("idle", false);
                }
                else // If agent isn't wandering, idle
                {
                    anim.SetBool("idle", true);
                }
            }
        }
    }
}
