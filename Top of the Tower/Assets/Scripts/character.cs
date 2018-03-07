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
	public bool attacking = false;

	// Colliders
//	public GameObject attackCollider;

//	public IEnumerator damage(character Char) {
//		Color originalColor = Char.gameObject.GetComponent<SpriteRenderer> ().color;
//		yield return new WaitForSeconds (.4f);
//		Char.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
//		yield return new WaitForSeconds (.1f);
//		Char.gameObject.GetComponent<SpriteRenderer> ().color = originalColor;
//		Char.takeDamage (2);
//	}

	virtual public void takeDamage(int damage) {
		health -= damage;

		print (gameObject.name + " taking " + damage + " damage.");
		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}

