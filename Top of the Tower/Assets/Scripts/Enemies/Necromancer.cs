using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Necromancer : EnemyController {

    // Necromancer stuff
    public GameObject explosion;
    public Transform[] skeletonSpawns;

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
	}

	protected override void Update()
	{
        //if(isAlive) {
        //    Move();
        //}
        if (timer > 3f) {
            timer = 0;
            spawnExplosion();
        }
        timer += Time.deltaTime;
	}

	private void spawnExplosion() {
        anim.SetTrigger("attack");
        GameObject explo = Instantiate(explosion, heroTransform.position, heroTransform.rotation) as GameObject;
    }

    private void spawnSkeleton() {
        for (int i = 0; i < skeletonSpawns.Length; ++i) {
            
        }
    }

    public override void Move()
    {
        
    }

    override public void takeDamage(int damage)
    {
        if (isAlive)
        {
            health -= damage;
            healthSlider.value = health;
            var clone = (GameObject)Instantiate(damageNumber, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), transform);
            clone.GetComponent<FloatingText>().damageNumber = -1 * damage;

            if (health <= 0) {
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

            if (health <= 0) {
                // kill necro
                isAlive = false;
            }
        }
    }
}
