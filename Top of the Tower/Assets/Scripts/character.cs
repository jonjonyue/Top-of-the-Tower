using UnityEngine;
using System.Collections;

public class character : MonoBehaviour
{
	// Enums
	public enum charType {player, enemy};

	// Informational
	public string charName;

	// General Stats
	public int health;
	public int defense;
	public int speed;

	// Code variables
	public bool isAlive = true;

	// Colliders
	public attackCollider attackCollider;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	virtual public void takeDamage(int damage) {
		health -= damage;

		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}

