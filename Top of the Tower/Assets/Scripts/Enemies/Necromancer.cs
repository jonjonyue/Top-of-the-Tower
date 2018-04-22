using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Necromancer : EnemyController
{

    // Necromancer stuff
    [Header("Necromancer Stuff")]
    public GameObject explosion;
    public Transform[] skeletonSpawns;
    public GameObject skeleton;
    public float spawnRate;
    public int maxSpawn;
    public float attackRate;
    private float spawnTimer = 0;
    private bool port = false;
    private int teleHP = 0;

    protected override void Start()
    {
        heroTransform = GameObject.Find("Hero").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //health bar setup
        healthSlider.maxValue = health;
        healthSlider.value = health;

        alert.SetActive(false);

        attacking = false;
        teleHP = health - 5;
    }

    protected override void Update()
    {
        if (isAlive && !port)
        {
            if (timer > attackRate)
            {
                timer = 0;
                spawnExplosion();
            }

            if (spawnTimer > spawnRate)
            {
                spawnTimer = 0;
                spawnSkeleton();
            }
            timer += Time.deltaTime;
            spawnTimer += Time.deltaTime;
        }
    }

    private void spawnExplosion()
    {
        anim.SetTrigger("attack");
        GameObject explo = Instantiate(explosion, heroTransform.position, heroTransform.rotation) as GameObject;
    }

    private void spawnSkeleton()
    {
        SkeletonSpawn[] spawns = GameObject.FindObjectsOfType<SkeletonSpawn>();
        int spawnsAlive = 0;
        foreach (SkeletonSpawn spawn in spawns) {
            if (spawn.isAlive)
                spawnsAlive++;
        }
        if (spawnsAlive <= maxSpawn)
        {
            anim.SetTrigger("summon");
            int numSpawn = Mathf.RoundToInt(Random.Range(.5f, 4.4999f));

            for (int i = 0; i < numSpawn; ++i)
            {
                if (spawnsAlive <= maxSpawn)
                {
                    summon(skeleton, skeletonSpawns[i]);
                }
            }
        }
    }

    private void summon(GameObject spawn, Transform location)
    {
        if (Random.Range(0,1) < .66f)
            Instantiate(spawn, location.position, location.rotation);
    }

    override public void takeDamage(int damage)
    {
        if (isAlive)
        {
            health -= damage;
            healthSlider.value = health;
            var clone = (GameObject)Instantiate(damageNumber, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), transform);
            clone.GetComponent<FloatingText>().damageNumber = -1 * damage;

            if (health < teleHP)
            {
                port = true;
                StartCoroutine(teleport());
                teleHP -= Mathf.RoundToInt(Random.Range(1, 7));
            }

            if (health <= 0)
            {
                // kill necro
                isAlive = false;
            }
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

            if (health < teleHP) {
                port = true;
                StartCoroutine(teleport());
                teleHP -= Mathf.RoundToInt(Random.Range(1, 7));
            }

            if (health <= 0)
            {
                // kill necro
                isAlive = false;
            }
        }
    }

    IEnumerator teleport()
    {
        anim.SetTrigger("teleport");
        yield return new WaitForSeconds(.1f);
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("NecroLeave") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }
        yield return new WaitForSeconds(.2f);
        agent.Warp(RandomNavSphere(transform.position, 20, -1));
        anim.SetTrigger("arrive");
        yield return new WaitForSeconds(.1f);
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("NecroArrive") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        port = false;
    }
}
