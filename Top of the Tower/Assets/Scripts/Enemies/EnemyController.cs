using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;
public class EnemyController : character
{

    // Enemy Specific Stats
    public float aggroDistance;

    [HideInInspector]public Transform heroTransform;
    public GameObject alert;
    public GameObject hitbox;
    public Collider[] attackHitboxes;

    public float wanderTimer;
    public float wanderRadius;
    public float attackDistance;

    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent agent;

    [HideInInspector] public float timer = 0;
    [HideInInspector] public Vector3 startPosition;

    // Use this for initialization
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        attacking = false;
        agent = GetComponent<NavMeshAgent>();

        //health bar setup
        healthSlider.maxValue = health;
        healthSlider.value = health;

        heroTransform = GameObject.Find("Hero").transform;

        timer = wanderTimer + Random.Range(0, 3);
        startPosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
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

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public static bool count()
    {

        if (Monitor.TryEnter(locker))
        {
            killed = killed + 1;
            Monitor.Exit(locker);
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void Attack(Collider col)
    {
        // can't do 2 attacks at once
        if (!attacking)
        {

            // START THE ATTACK!!!
            attacking = true;

            //ATTACK ANIMATION
            //Note: this depends on the direction (using Triggers)
            anim.SetTrigger("attack");
            StartCoroutine(wait(col));
        }
    }

    IEnumerator wait(Collider col)
    {
        yield return new WaitForSeconds(Random.Range(.2f, .5f));

        StartCoroutine(damage(col));
    }

    public virtual IEnumerator damage(Collider col)
    {

        // Get the hitboxes hit by the attack
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        bool hit = false;
        //ATTACK MECHANICS
        foreach (Collider c in cols)
        {
            if (c.tag == "Player")
            {
                if (isAlive)
                {
                    PlayerController player = c.gameObject.GetComponentInParent<PlayerController>();
                    Color originalColor = player.GetComponent<SpriteRenderer>().color;
                    player.GetComponent<SpriteRenderer>().color = Color.red;
                    yield return new WaitForSeconds(.2f);
                    player.GetComponent<SpriteRenderer>().color = originalColor;
                    player.takeCombatDamage(strength);
                    yield return new WaitForSeconds(.4f);
                    hit = true;
                }
            }
        }
        if (!hit)
            yield return new WaitForSeconds(.6f);

        // we are done attacking
        attacking = false;
    }

    // Moves the Player (if keys are down)
    // Check key press, move in that direction
    // also changes animation for smooth walk
    // if no key pressed, idle animation
    public virtual void Move()
    {
        // animation codes:
        // 0 - forward walk
        // 1 - side walk
        // 3 - back walk

        Vector3 targetPosition = heroTransform.position;
        Vector3 targetVector = targetPosition - transform.position;

        if (targetVector.magnitude < aggroDistance)
        {
            alert.SetActive(true);
            //if (Mathf.Abs(targetVector.magnitude) > attackDistance && !attacking)
            //{
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
            //}
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
            //if (!attacking) {

            if (agent.velocity.magnitude != 0) // if agent is wandering, walk
            {

                var enemyAngle = Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg;

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
            else // If agent isn't wandering, idle
            {
                int currAnim = anim.GetInteger("direction");
                if (currAnim == 3)
                {
                    anim.SetBool("idle", true);
                }
                else if (currAnim == 1)
                {
                    anim.SetBool("idle", true);
                }
                else
                {
                    anim.SetBool("idle", true);
                }
            }
            //}
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
            //Debug.Log (charName + " took " + damage + " damage...");
            if (health <= 0)
                dead();
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
            //Debug.Log(charName + " took " + damage + " damage...");
            if (health <= 0)
                dead();
            else
                StartCoroutine(flinch());
        }
    }

    IEnumerator flinch()
    {
        int animLayer = 0;
        string stateName = "flinch";
        anim.SetTrigger("flinch");

        //Wait until Animator is done playing
        while (anim.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
            anim.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f)
        {
            //Wait every frame until animation has finished
            yield return null;
        }

        //Done playing. Do something below!
        //Debug.Log("flinched");
    }

    void dead()
    {
        isAlive = false;
        anim.SetTrigger("dead");
        alert.SetActive(false);
        hitbox.SetActive(false);
        healthSlider.gameObject.SetActive(false);
    }
}
