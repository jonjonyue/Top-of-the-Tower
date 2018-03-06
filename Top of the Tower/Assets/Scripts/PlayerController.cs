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
	private bool attacking;
	List<GameObject> nearEnemy = new List<GameObject>();
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		attacking = false;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		// we should only be able to attack once per press, not continuously
		if (Input.GetKeyDown(KeyCode.Space)){
			print ("calling attack");
			StartCoroutine(Attack ());
		}
	}
		
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Enemy") {
			nearEnemy.Add(col.gameObject);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Enemy") {
			nearEnemy.Remove(col.gameObject);
		}
	}

	IEnumerator Attack() {
		// can't do 2 attacks at once
		print("entering attack function");
		if (attacking) {
			yield return new WaitForSeconds (0f);
		}
		// START THE ATTACK!!!
		attacking = true;

		//ATTACK ANIMATION
		//Note: this depends on the direction (using Triggers)
		anim.SetTrigger("attack"); 

		//ATTACK MECHANICS
		//put your code here @Eric
		var colors = new Color[2];

		Vector3 pos = transform.position;
		for(int i = 0; i < nearEnemy.Count;i++)
		{
			int damage = 2;
			colors [1] = nearEnemy[i].GetComponent<Renderer>().material.color;
			colors [0] = Color.red;
			Vector3 vec = nearEnemy[i].transform.position;
			Vector3 direction = vec - pos;
			if(Vector3.Dot(direction, transform.forward)<0.7){
				print("attacking");
				character cha = (character)nearEnemy[i].GetComponent<character> ();
				cha.health -= damage;
				nearEnemy[i].GetComponent<Renderer>().material.color = colors[0];
				yield return new WaitForSeconds(0.25f);
				nearEnemy[i].GetComponent<Renderer>().material.color = colors[1];
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
		// 2 - forward idle 
		// 3 - back walk
		// 4 - back idle
		if (Input.GetKey(KeyCode.LeftArrow)) {
			Vector3 position = this.transform.position;
			position.x -= speed * Time.deltaTime;
			this.transform.position = position;
			sr.flipX = true;
			anim.SetInteger ("direction", 1);
			this.GetComponent<SphereCollider> ().center = new Vector3 (-.5f, .3f, 0f);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			Vector3 position = this.transform.position;
			position.x += speed * Time.deltaTime;
			this.transform.position = position;
			sr.flipX = false;
			anim.SetInteger ("direction", 1);
			this.GetComponent<SphereCollider> ().center = new Vector3 (.6f, .3f, 0f);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			Vector3 position = this.transform.position;
			position.z -= speed * Time.deltaTime;
			this.transform.position = position;
			anim.SetInteger ("direction", 0);
			this.GetComponent<SphereCollider> ().center = new Vector3 (.4f, .5f, 0f);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			Vector3 position = this.transform.position;
			position.z+= speed*Time.deltaTime;
			this.transform.position = position;
			anim.SetInteger ("direction", 3);
			this.GetComponent<SphereCollider> ().center = new Vector3 (-.4f, .5f, 0f);
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
		
}
