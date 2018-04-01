using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : character {

	// Enemy Specific Stats
	public float aggroDistance;

	public Transform heroTransform;
	public GameObject alert;
	public GameObject hitbox;
	public Collider[] attackHitboxes;

	private SpriteRenderer sr;
	private Animator anim;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		attacking = false;
		agent = GetComponent<NavMeshAgent> ();

        //health bar setup
        healthSlider.maxValue = health;
        healthSlider.value = health;
	}

	// Update is called once per frame
	void Update () {
		if (isAlive) {
			Move ();
		}
	}

	void Attack(Collider col) {
		// can't do 2 attacks at once
		if (!attacking) {

			// START THE ATTACK!!!
			attacking = true;

			//ATTACK ANIMATION
			//Note: this depends on the direction (using Triggers)
			anim.SetTrigger ("attack");

			StartCoroutine (damage (col));
		}
	}

	public virtual IEnumerator damage(Collider col) {
		
		yield return new WaitForSeconds (0f);

		// Get the hitboxes hit by the attack
		Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

		//ATTACK MECHANICS
		foreach (Collider c in cols) {
			if (c.tag == "Player") {
                if (isAlive)
                {
                    PlayerController player = c.gameObject.GetComponentInParent<PlayerController>();
                    Color originalColor = player.GetComponent<SpriteRenderer>().color;
                    player.GetComponent<SpriteRenderer>().color = Color.red;
                    yield return new WaitForSeconds(.2f);
                    player.GetComponent<SpriteRenderer>().color = originalColor;
                    player.takeDamage(2);
                }
			}
		}

		// we are done attacking
		attacking = false;
	}

	// Moves the Player (if keys are down)
	// Check key press, move in that direction
	// also changes animation for smooth walk
	// if no key pressed, idle animation
	void Move() {
		// animation codes:
		// 0 - forward walk
		// 1 - side walk
		// 3 - back walk

		Vector3 targetPosition = heroTransform.position;
		Vector3 targetVector = targetPosition - transform.position;

		if (targetVector.magnitude < aggroDistance) {
			alert.SetActive (true);
			if (Mathf.Abs(targetVector.magnitude) > 1) {
				Vector3 unitVector = targetVector / targetVector.magnitude;

				agent.SetDestination(heroTransform.position);

				var enemyAngle = Mathf.Atan2 (targetVector.z, targetVector.x) * Mathf.Rad2Deg;

				if (enemyAngle < 0.0f)
					enemyAngle += 360;

				if (enemyAngle >= 315f || enemyAngle < 45f) { /* Right */
					sr.flipX = false;
					anim.SetBool ("idle", false);
					anim.SetInteger ("direction", 1);
				} else if (enemyAngle >= 135f && enemyAngle < 225f) { /* Left */
					sr.flipX = true;
					anim.SetBool ("idle", false);
					anim.SetInteger ("direction", 1);
				} else if (enemyAngle >= 45f && enemyAngle < 135f) { /* Up */
					anim.SetBool ("idle", false);
					anim.SetInteger ("direction", 3);
				} else if (enemyAngle >= 225f && enemyAngle < 315f) { /* Down */
					anim.SetBool ("idle", false);
					anim.SetInteger ("direction", 0);
				}
			} else if (!attacking) {
				agent.SetDestination(transform.position);
				Attack (attackHitboxes[0]);
			}
		} else {
			alert.SetActive (false);

			agent.SetDestination(transform.position);
			// check which way he is facing, set idle animation to that one
			if (!attacking) {
				int currAnim = anim.GetInteger ("direction");
				if (currAnim == 3) {
					anim.SetBool ("idle", true);
				} else if (currAnim == 1) {
					anim.SetBool ("idle", true);
				} else {
					anim.SetBool ("idle", true);
				}
			}
		}
	}

	override public void takeDamage(int damage) {
		if (isAlive) {
			health -= damage;
            healthSlider.value = health;
			//print (charName + " took " + damage + " damage...");
			if (health <= 0)
				dead ();
		}
	}

	void dead() {
		isAlive = false;
		anim.SetTrigger ("dead");
		alert.SetActive (false);
		hitbox.SetActive (false);
        healthSlider.gameObject.SetActive(false);
	}
}
