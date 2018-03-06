using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : character {

	// Enemy Specific Stats
	public float aggroDistance;

	public Transform heroTransform;
	public GameObject alert;

	private SpriteRenderer sr;
	private Animator anim;
	private bool attacking;
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		attacking = false;
	}

	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Attack() {
		// can't do 2 attacks at once
		if (attacking) {
			return;
		}
		// START THE ATTACK!!!
		attacking = true;

		//ATTACK ANIMATION
		//Note: this depends on the direction (using Triggers)
		anim.SetTrigger("attack"); 

		//ATTACK MECHANICS
		//put your code here @Eric

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
		// 2 - forward idle 
		// 3 - back walk
		// 4 - back idle

		Vector3 targetPosition = heroTransform.position;
		Vector3 targetVector = targetPosition - transform.position;

		if (targetVector.magnitude < aggroDistance) {
			alert.SetActive (true);
			if (targetVector.magnitude > 1) {
				Vector3 unitVector = targetVector / targetVector.magnitude;

				if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BackAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ForwardAttack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("HeroSideAttack")) {
					transform.Translate (speed * Time.deltaTime * unitVector);
				}

				var enemyAngle = Mathf.Atan2 (targetVector.z, targetVector.x) * Mathf.Rad2Deg;

				if (enemyAngle < 0.0f)
					enemyAngle += 360;

				Debug.Log ("Angle from the player is: " + enemyAngle);

				if (enemyAngle >= 315f || enemyAngle < 45f) { /* Right */
//					Debug.Log ("Going Right!");
					sr.flipX = false;
					anim.SetInteger ("direction", 1);
				} else if (enemyAngle >= 45f && enemyAngle < 135f) { /* Up */
//					Debug.Log ("Going Up!");
					anim.SetInteger ("direction", 3);
				} else if (enemyAngle >= 135f && enemyAngle < 225f) { /* Left */
					sr.flipX = true;
					anim.SetInteger ("direction", 1);
//					Debug.Log ("Going Left");
				} else if (enemyAngle >= 225f && enemyAngle < 315f) { /* Down */
//					Debug.Log ("Going Down!");
					anim.SetInteger ("direction", 0);
				}
			} else {
				Attack ();
			}
		} else {
			alert.SetActive (false);

			// check which way he is facing, set idle animation to that one
			if (!attacking) {
				int currAnim = anim.GetInteger ("direction");
				if (currAnim == 3 || currAnim == 4) {
					anim.SetInteger ("direction", 4);
				} else if (currAnim == 1 || currAnim == 5) {
					anim.SetInteger ("direction", 5);
				} else {
					anim.SetInteger ("direction", 2);
				}
			}
		}
	}
}
