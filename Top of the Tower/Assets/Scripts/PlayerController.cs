using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : character {

	/*
	 * Variables in Character
	 * 
	// Enums
	public enum charType {player, enemy};

 	// Informational
	public string name;

 	// General Stats
	public int health;
	public int defense;
	public int speed;

 	// Code variables
	public bool isAlive = 0;
	*/

	// Player Specific stats
	public int mana;
	private SpriteRenderer sr;
	private Animator anim;
	private bool canMove = true;
//	private bool attacking;

	List<GameObject> nearEnemy = new List<GameObject>();

	public SphereCollider[] attackHitboxes;

	private CharacterController cc;


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		cc = GetComponent < CharacterController> ();
		attacking = false;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		// we should only be able to attack once per press, not continuously
		if (Input.GetKeyDown(KeyCode.Space)){
//			print ("calling attack");
			Attack (attackHitboxes[0]);
		}
	}

	void Attack(Collider col) {
		// can't do 2 attacks at once
//		print("entering attack function");
		if (attacking) {
			return;
		}
		// START THE ATTACK!!!
		attacking = true;
		Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

		//ATTACK ANIMATION
		//Note: this depends on the direction (using Triggers)
		anim.SetTrigger("attack"); 

		foreach(Collider c in cols) {
			if (c.tag == "Enemy") {
				EnemyController enemy = c.gameObject.GetComponentInParent<EnemyController> ();
				StartCoroutine(damage (enemy));
			}
		}

		// we are done attacking
		attacking = false;
	}

	IEnumerator damage(EnemyController Char) {
		Color originalColor = Char.gameObject.GetComponent<SpriteRenderer> ().color;
		yield return new WaitForSeconds (.4f);
		Char.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		yield return new WaitForSeconds (.1f);
		Char.gameObject.GetComponent<SpriteRenderer> ().color = originalColor;
		Char.takeDamage (2);
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

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		cc.Move(move * Time.deltaTime * speed);

		if (Input.GetKey(KeyCode.LeftArrow)) {
			sr.flipX = true;
			anim.SetInteger ("direction", 1);
			attackHitboxes[0].center = new Vector3 (-.5f, .3f, 0f);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			sr.flipX = false;
			anim.SetInteger ("direction", 1);
			attackHitboxes[0].center = new Vector3 (.5f, .3f, 0f);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			anim.SetInteger ("direction", 0);
			attackHitboxes[0].center = new Vector3 (0f, .3f, -.5f);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			anim.SetInteger ("direction", 3);
			attackHitboxes[0].center = new Vector3 (0f, .3f, .5f);
		}
		// check which way he is facing, set idle animation to that one
		if (!Input.anyKey && !attacking) {
			int currAnim = anim.GetInteger ("direction");
			if (currAnim == 3 || currAnim == 4) {
				anim.SetInteger ("direction", 4);
			} else if (currAnim == 1 || currAnim == 5){
				anim.SetInteger ("direction", 5);
			}
			else {
				anim.SetInteger ("direction", 2);
			}

		}
	}

	override public void takeDamage(int damage) {
		health -= damage - defense;

		print ("Hero took " + damage + " damage...");
		if (health <= 0) {
			print ("Hero has died");
		}
	}

//	void OnCollisionEnter(Collision collision) {
//		if (collision.collider.tag == "Environment") {
//			print ("Hitting the environment");
//			canMove = false;
//		}
//	}
//	void OnCollisionExit(Collision collision) {
//		canMove = true;
//	}
}
