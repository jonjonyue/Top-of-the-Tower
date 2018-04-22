using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	private int maxHealth;
	private int level;
	private SpriteRenderer sr;
	private Animator anim;

	public BoxCollider[] attackHitboxes;

	private CharacterController cc;


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		cc = GetComponent < CharacterController> ();
		attacking = false;
		healthSlider.value = health;
		maxHealth = health;
		level = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		// we should only be able to attack once per press, not continuously
		if (Input.GetKeyDown(KeyCode.Space)){
//			print ("calling attack");
			if (!attacking) 
				Attack (attackHitboxes[0]);
		}
	}

    // return how many enemies killed so faar
    public int slayed()
    {
        return killed;
    }

    void Attack(Collider col) {
		// can't do 2 attacks at once
		// START THE ATTACK!!!
		attacking = true;

		//ATTACK ANIMATION
		//Note: this depends on the direction (using Triggers)
		anim.SetTrigger ("attack"); 

		// still wait to attack again if we miss everything
		StartCoroutine(damage(col));
	}

	IEnumerator damage(Collider col) {
		yield return new WaitForSeconds (.05f);

		// Get the hitboxes hit by the attack
		Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

		//ATTACK MECHANICS
		foreach (Collider c in cols) {
			if (c.tag == "Enemy") {
				EnemyController enemy = c.gameObject.GetComponentInParent<EnemyController> ();
                enemy.GetComponent<SpriteRenderer> ().color = Color.red;
                enemy.takeCombatDamage(strength);
			}
		}

		// Wait before turning white again
		yield return new WaitForSeconds (.2f);

		foreach (Collider c in cols) {
			if (c.tag == "Enemy") {
				EnemyController enemy = c.gameObject.GetComponentInParent<EnemyController> ();
				enemy.GetComponent<SpriteRenderer> ().color = Color.white;
			}
		}

        yield return new WaitForSeconds(.41f);

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

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		cc.Move(move * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			sr.flipX = true;
			anim.SetInteger ("direction", 1);
			attackHitboxes[0].center = new Vector3 (-.6f, .3f, 0f);
		}
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			sr.flipX = false;
			anim.SetInteger ("direction", 1);
			attackHitboxes[0].center = new Vector3 (.6f, .3f, 0f);
		}
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			anim.SetInteger ("direction", 0);
			attackHitboxes[0].center = new Vector3 (0f, .3f, -.6f);
		}
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			anim.SetInteger ("direction", 3);
			attackHitboxes[0].center = new Vector3 (0f, .3f, .6f);
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
		health -= damage;
		healthSlider.value = health;
		//print ("Hero took " + (damage - defense) + " damage...");
        var clone = (GameObject)Instantiate(damageNumber, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), transform);
        clone.GetComponent<FloatingText>().damageNumber = -1 * damage;
		if (health <= 0) {
			losing ();
		}
		GetComponent<SpriteRenderer> ().color = Color.white;
	}

    override public void takeCombatDamage(int damage) {
        health -= damage - defense;
        healthSlider.value = health;

        var clone = (GameObject)Instantiate(damageNumber, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(Vector3.zero), transform);
        clone.GetComponent<FloatingText>().damageNumber = defense - damage;
        //print ("Hero took " + (damage - defense) + " damage...");
        if (health <= 0)
        {
			losing ();
        }
        GetComponent<SpriteRenderer>().color = Color.white;
    }

	public void losing(){
		GameObject upgrade = GameObject.Find("GUI");
		GameObject up = upgrade.transform.Find ("Lose").gameObject;
		up.SetActive(true);
		Time.timeScale = 0f;
	}

	public void heal(int hpRestored) {
		if (health + hpRestored > maxHealth)
			health = maxHealth;
		else
			health += hpRestored;
		
		healthSlider.value = health;
		print ("Hero healed " + hpRestored + " damage...");
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "NextLevel") {
            respawn();
			GameObject upgrade = GameObject.Find("GUI");
			GameObject up = upgrade.transform.Find ("Upgrade").gameObject;
			if (level != 4) {
				up.SetActive(true);
				Time.timeScale = 0f;
			}

		}
		if (collider.tag == "Respawn") {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

    void respawn() {
        maxHealth += 20;
        healthSlider.maxValue += 20;
        health = maxHealth;
        healthSlider.value = health;
		if (level == 1) {
			Vector3 move = new Vector3 (43.34f, .91f, -106.46f);
			cc.gameObject.transform.position = move;
			level = 2;
		} else if (level == 2) {
			Vector3 move = new Vector3 (151f, .91f, -183.8f);
			cc.gameObject.transform.position = move;
			level = 3;
		} else if (level == 3) {
			//Adrien code here
			Vector3 move = new Vector3 (240.3f, .91f, -260.7f);
			cc.gameObject.transform.position = move;
			level = 4;
		} else if (level == 4) {
			GameObject upgrade = GameObject.Find ("GUI");
			GameObject up1 = upgrade.transform.Find ("Win").gameObject;
			//GameObject up2 = upgrade.transform.Find ("Upgrade").gameObject;
			up1.SetActive (true);
			//up2.SetActive(false);
			Time.timeScale = 0f;
		}

    }
}
