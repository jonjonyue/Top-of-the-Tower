using UnityEngine;
using System.Collections;

public class Slime : EnemyController
{
    private Vector3 attackStartPosition;
    private Vector3 target;
    private bool didHitWhileDashing = false;
    public float dashSpeed;

    private float attackTimer = 0;


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
            if (Vector3.Distance(transform.position, heroTransform.position) < attackDistance || attacking)
            {
                Attack(attackHitboxes[0]);
            }
            else
            {
                Move();
            }
        }

        if (!counted)
        {
            if (health <= 0)
            {
                counted = count();
            }
        }
    }

    public override void Attack(Collider col)
    {
        if (attacking)
        {
            launchAttack();

        }
        else
        {
            anim.SetTrigger("attack");
            //agent.updatePosition = false;
            attackStartPosition = transform.position;
            Vector3 targetVector = (transform.position - heroTransform.position) * -1.5f;
            targetVector.y = targetVector.y / 1.5f;
            target = attackStartPosition + targetVector;
            agent.speed = 8;
            agent.SetDestination(target);
            attacking = true;
        }
    }

    /*
     *  Make sure to set initial position and target before calling this function
     * 
     */
    void launchAttack()
    {
        if (attackTimer > 1.5f)
        {
            Debug.Log("Attack End");
            agent.speed = 1.5f;
            StartCoroutine(wait());
            //agent.updatePosition = true;
        }
        else
        {
            //transform.position = Vector3.Lerp(attackStartPosition, target, dashSpeed * Time.deltaTime);
            //agent.nextPosition = transform.position;
            //Debug.Log("Checking for hit");
            StartCoroutine(damage(attackHitboxes[0]));
        }
    }

    public override IEnumerator damage(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        //ATTACK MECHANICS
        foreach (Collider c in cols)
        {
            if (c.tag == "Player")
            {
                if (!didHitWhileDashing)
                {
                    Debug.Log("Hit!");
                    PlayerController player = c.gameObject.GetComponentInParent<PlayerController>();
                    Color originalColor = player.GetComponent<SpriteRenderer>().color;
                    player.GetComponent<SpriteRenderer>().color = Color.red;
                    yield return new WaitForSeconds(.2f);
                    attackTimer += .2f;
                    player.GetComponent<SpriteRenderer>().color = originalColor;
                    player.takeCombatDamage(strength);
                    yield return new WaitForSeconds(.4f);
                    attackTimer += .4f;
                    didHitWhileDashing = true;
                }
            }
        }
        attackTimer += Time.deltaTime;
    }

    IEnumerator wait()
    {
        attackTimer = 0;
        while(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
        {
            attackTimer = 0;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        attackTimer = 0;
        attacking = false;
        didHitWhileDashing = false;
    }

    public override void Move()
    {

        Vector3 targetPosition = heroTransform.position;
        Vector3 targetVector = targetPosition - transform.position;

        if (targetVector.magnitude < aggroDistance)
        {
            alert.SetActive(true);
            if (Mathf.Abs(targetVector.magnitude) > attackDistance && !attacking)
            {
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
                Attack(attackHitboxes[0]);
            }
        }
        else
        {
            alert.SetActive(false);
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(startPosition, wanderRadius, -1);
                targetPosition = newPos;
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
